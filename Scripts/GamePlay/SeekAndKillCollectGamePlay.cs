using _GAME.Scripts.Inventory;
using Assets._GAME.Scripts.Enemies;
using Assets._SDK.Analytics;
using Assets._GAME.Scripts.Skills;
using Assets._SDK.GamePlay;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Assets._SDK.Ads;
using Unity.VisualScripting;
using System.Linq;
using Assets._GAME.Scripts.Enemies.Skills;
using NSubstitute;
using Sirenix.OdinInspector;
using Assets._SDK.Input;

namespace Assets._GAME.Scripts.GamePlay
{
	public class SeekAndKillCollectGamePlay : AbstractNextBotGamePlay
	{
        public static SeekAndKillCollectGamePlay Instance => (SeekAndKillCollectGamePlay)GamePlayLoader.Instance.CurrentGamePlay;

        public SeekAndSkillCollectObservable SeekAndSkillCollectObservable;
        public Transform ExitDoor => _exitDoorGameObject.transform;
        public float TargetKeyToWin => _targetToWin;
        List<GameObject> CoinOnMapGameItems;
        GameObject _exitDoorGameObject;

        private const int MISSION_OVER_TIME = 40;
        private const int TIME_DELAY_SPAWN_CHASING = 50;
        private const int TIME_SPAWN_CHASING_EVERY_SECOND = 10;
        private float _targetToWin;
        private CompositeDisposable _disposablesObservable;
        IEnumerator countDownUpgradeSupportItem;
        CompositeDisposable _disposables;

        bool isWin;

        public bool HasTriedOneTimeGetWeapon { get; set; }
        protected override void OnStart()
        {
            gamePlayingMainPanel?.StartUISeekAndKillCollect(_targetToWin);

            SeekAndSkillCollectObservable = gameObject.GetComponent<SeekAndSkillCollectObservable>();
            if (SeekAndSkillCollectObservable == null)
                SeekAndSkillCollectObservable = gameObject.AddComponent<SeekAndSkillCollectObservable>();

            if (countDownUpgradeSupportItem != null)
                StopCoroutine(countDownUpgradeSupportItem);
            countDownUpgradeSupportItem = CountDownUpgradeSupportItem(MISSION_OVER_TIME);
            StartCoroutine(countDownUpgradeSupportItem);

            audioSource = gameObject.GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();

            soundBreakTime = GameManager.Instance.SoundManager?.GetSoundBreakTime();

            isWin = false;
            HasTriedOneTimeGetWeapon = false;

            RunnerSkillSystem.RunnerObservable.OnFoundKeyStream.Subscribe((TotalKeysFound) =>
            {
                if (TotalKeysFound <= 0) return;

				if (TotalKeysFound <= playingMission.TargetWin)
                    gamePlayingMainPanel?.UpdateKeysFoundSeekAndKill(TotalKeysFound);

                CheckConditionOpenDoor(TotalKeysFound);
            }).AddTo(gameObject);

            RunnerSkillSystem.RunnerObservable.OnCompeleteMissionStream.Subscribe(isWin =>
            {
                StartWin(isWin);
            }).AddTo(gameObject);

            Subcribe();
        }
        private void Subcribe()
        {
            _disposablesObservable = new CompositeDisposable();

            SeekAndSkillCollectObservable.OnRemoveSlotStream
                .Subscribe(nameMonster =>
                {
                    ClearMonsterHasKeyDeadInRegionalKeySlot(nameMonster);
                }).AddTo(_disposablesObservable);

            SeekAndSkillCollectObservable.OnMonsterHasKeyDeadStream
                .Subscribe(Monster =>
                {
                    ClearMonsterHasKeyDeadInRegionalKeySlot(Monster.name);
                    if(Monster.GetComponent<OffScreenTargetIndicator>() != null)
                        RunnerSkillSystem.RunnerObservable.SetChangeNearTarget(Monster.name);
                }).AddTo(_disposablesObservable);
        }
        private void ClearMonsterHasKeyDeadInRegionalKeySlot(string name)
        {
            regionalEnemyKeySlots = regionalEnemyKeySlots.Where(v => v.Enemy.name != name).ToList();
        }
        private IEnumerator CountDownUpgradeSupportItem(int timeStartCountDown)
        {
            const int TIME_STEP = 1;
            const int TIME_COUNTDOWN = 5;
            int timeCountDown = (int)timeStartCountDown;
            WaitForSeconds timePerSecond = new WaitForSeconds(TIME_STEP);
            while (timeCountDown >= 0)
            {
                timeCountDown -= TIME_STEP;
                ;
                if (timeCountDown <= TIME_COUNTDOWN)
                    gamePlayingMainPanel?.SetTextUgradingCountdown("Break Time In ", timeCountDown);

                if (timeCountDown <= 0 && soundBreakTime != null)
                {
                    audioSource.PlayOneShot(soundBreakTime);
                    break;
                }
                yield return timePerSecond;
            }

            yield return timePerSecond;
            ShowPopupSupportItem();
        }

        private void ShowPopupSupportItem()
        {
            if (isWin == true) return;

            if (countDownUpgradeSupportItem != null)
                StopCoroutine(countDownUpgradeSupportItem);
            countDownUpgradeSupportItem = CountDownUpgradeSupportItem(MISSION_OVER_TIME);
            StartCoroutine(countDownUpgradeSupportItem);

            Debug.Log("Inter In HERE");
            if (GamePlayLoader.Instance.CurrentGamePlay.IsMissionTutorial != true)
                AdsManager.Instance.ShowInterstitial(playingMission.Order, AnalyticParamKey.MISSION_MAX_LENGTH);

            bool isHP = (UnityEngine.Random.Range(0, 2) == 1);
            var SkillSupportItemUI = GamePlayLoader.Instance.GameSceneUI.SkillSupportItemUI;
            if (isHP)
                SkillSupportItemUI.InitHPItemsByMissionMaxLength();
            else
                SkillSupportItemUI.InitStaminaItemsByMissionMaxLength();
            StartCoroutine(ChangeStateUpgradeSkill());
        }

        protected override void InitMap()
        {
            base.InitMap();
            InitExitDoor();
        }

		private void InitExitDoor()
        {
            var door = _playingMap.ExitDoors[playingMission.ExitDoorPosition - 1].transform;
            _exitDoorGameObject = Instantiate(_playingMap.ExitDoorGameObject, door.position, door.rotation, MapHolder.transform);
            _exitDoorGameObject.transform.localScale = door.localScale;
            var doorBehavior = _exitDoorGameObject.AddComponent<DoorExitBehavior>();
            doorBehavior.InitAttribute(_exitDoorGameObject.transform);
        }

        protected override void InitRunner()
        {
            base.InitRunner();
            var playingWeapon = GameManager.Instance.WeaponInventory.PlayingWeapon;
            RunnerSkillSystem.AttachSkillAttack((Weapon)playingWeapon);
        }

        protected override void InitSkillSupportItemSettings()
        {
            base.InitSkillSupportItemSettings();

            //SkillSupportItems = playingMission.InitGameObjectSupportItems(SupportItemSpawnPoints, MapHolder.transform);

            var WeaponsSpawn = GameManager.Instance.WeaponInventory.GetWeaponsUnOwn();

            if (WeaponsSpawn?.Count > 0)
            {
                for (int i = 0; i < SupportItemSpawnPoints.Count; i++)
                {
                    GameObject SupportItemsPrefab = UnityEngine.Object.Instantiate(WeaponsSpawn[i % WeaponsSpawn.Count].ModelItemSupport, SupportItemSpawnPoints[i].transform.position, Quaternion.identity, MapHolder.transform);

                    SupportItemsPrefab.GetComponent<ItemSupportSkillUpgrade>().InitData(WeaponsSpawn[i % WeaponsSpawn.Count]);

                    SkillSupportItems.Add(SupportItemsPrefab);
                }
                SupportItemSpawnPoints.Clear();
            }
            else
            {
                while (SupportItemSpawnPoints.Count > 0 && playingMission.SupportItemsSettings?.Count > 0)
                {
                    SkillSupportItems.AddRange(playingMission.InitGameObjectSupportItems(SupportItemSpawnPoints, MapHolder.transform));
                }
            }
        }

        protected override void InitEnemies()
        {
            base.InitEnemies();
            if (AmountRESleep < playingMission.TargetWin)
                InitRESpecal((playingMission.TargetWin - AmountRESleep), RegionalEnemyType.RE0);

            for (int i = 0; i < EnemyGroups.Count; i++)
                LoadEnemyGroup(GlobalEnemySpawnPoints, RegionalEnemySpawnPoints, EnemyGroups[i]);

            RunnerSkillSystem.RunnerObservable.SetChangeNearTarget("");

            StartCoroutine(SpawnChasingEnemyPeriodically());
        }

        private IEnumerator SpawnChasingEnemyPeriodically()
        {
            yield return new WaitForSeconds(TIME_DELAY_SPAWN_CHASING);

            List<RegionalEnemySlot> listEnemyHasKey = ListRegionalEnemyHasKeySlots;
            listEnemyHasKey = listEnemyHasKey.Where(v => v.Enemy.GetComponent<MoveEnemySkillBehavior>() != null).ToList();

            System.Random random = new System.Random();
            List<RegionalEnemySlot> shuffleEnemyList = listEnemyHasKey.OrderBy(x => random.Next()).ToList();

            WaitForSeconds timeSpawnChasing = new WaitForSeconds(TIME_SPAWN_CHASING_EVERY_SECOND);
            while (shuffleEnemyList.Count > 0)
            {
                var enemyslot = shuffleEnemyList.FirstOrDefault();

				var enemy = enemyslot.Enemy;
                if (enemy == null) break;

                var enemyObservable =  enemy.GetComponent<RegionalEnemyObservable>();
                enemyObservable.SetChasing(Runner.transform);
                shuffleEnemyList.Remove(enemyslot);
                yield return timeSpawnChasing;
            }
        }

        void LoadEnemyGroup(List<GameObject> globalEnemyPositions, List<GameObject> regionalEnemyPositions, EnemyGroup enemyGroup)
        {
            if (enemyGroup.Type == EnemyType.Regional)
            {
                for (int i = 0; i < enemyGroup.Amount; i++)
                {
                    if (IsEnemySpawnIgnored(i))
                        continue;

                    RegionalEnemySlot RESlot = new RegionalEnemySlot(MapHolder);
                    RESlot.AddBehaviorIfNull(regionalEnemyPositions[amountRE++], enemyGroup.level);
                    RESlot.AddSkinIfNull(enemyGroup.EnemySettings.skin);
                    RESlot.REObservable.SetKey(enemyGroup.HasKey);

					if (RESlot.REObservable.HasKey)
					{
						GameObject keyPrefab = Instantiate(GameManager.Instance.Resources.Key, Vector3.zero, Quaternion.identity, RESlot.Enemy.transform);
						RectTransform keyRect = keyPrefab.GetComponent<RectTransform>();
						keyRect.SetLocalPositionAndRotation(new Vector3(-0.15f, 1.5f, 0.25f), Quaternion.Euler(Vector3.zero));
						regionalEnemyKeySlots.Add(RESlot);

                        KeyLookAtTargetHandle(keyPrefab, Runner.transform);
                    }
					//RESlot.Enemy.GetComponent<RegionalEnemySkillSystem>().LoadSkillSleep();
					RegionalEnemySlots.Add(RESlot);
                }
            }

            if (enemyGroup.Type == EnemyType.Global)
            {
                for (int i = 0; i < enemyGroup.Amount; i++)
                {
                    GlobalEnemySlot GESlot = new GlobalEnemySlot(MapHolder);
                    GESlot.AddBehaviorIfNull(globalEnemyPositions[amountGE++], enemyGroup.level);
                    GESlot.AddSkinIfNull(enemyGroup.EnemySettings.skin);
                    GlobalEnemySlots.Add(GESlot);
                }
            }
        }

        private void CheckConditionOpenDoor(int TotalKeysGet)
        {
            if (TotalKeysGet >= _targetToWin)
            {
                _exitDoorGameObject.GetComponent<DoorExitBehavior>().SetDetectOpenDoor(true);
                RunnerSkillSystem.RunnerObservable.SetExitDoorOpened(true);
				
			}
        }

        private void StartWin(bool isWinActive)
        {
            RunnerSkillSystem.UnSubscribeSkillWhenWin();
            RunnerSkillSystem.RunnerObservable.ClearObservers();
            isWin = isWinActive;
            StartCoroutine(DelayToWin());
        }

        IEnumerator DelayToWin()
        {
            yield return new WaitForEndOfFrame();
            RunnerSkillSystem.GetComponentInChildren<Animator>().SetTrigger("StartViewWin");
            yield return new WaitForSeconds(3f);
            if (CanFire(GamePlayTrigger.Win))
                Fire(GamePlayTrigger.Win);
            Debug.Log("Win");

        }

        public void ResetData()
        {
            _targetToWin = 0;
            isWin = false;
        }

        protected override void OnInitGamePlay()
        {
            InitCoinOnMapGameItems();

            _targetToWin = playingMission.TargetWin;

            StateMachine.Activate();

            StateMachine.Fire(GamePlayTrigger.Play);


            TutorialHandle();
        }

        private void TutorialHandle()
        {
            var tutorial = GamePlayLoader.Instance.GameSceneUI.GamePlayingMainPanelUI.TutorialGuide;
            if (playingMission.TutorialUIEnemy != null && !tutorial.activeSelf)
            {
                tutorial.SetActive(true);
                tutorial.GetComponentInChildren<Animator>().SetTrigger("StartTutorialAnimation");
                SubcribeTutorial(tutorial);
            }
            else
                tutorial.SetActive(false);
        }
        private void SubcribeTutorial(GameObject tutorial)
        {
            _disposables = new CompositeDisposable();

            RunnerSkillSystem?.RunnerObservable.WalkStream.Subscribe((arrayTouch) =>
            {
                if (arrayTouch != Vector3.zero)
                    tutorial.GetComponentInChildren<Animator>().SetTrigger("NextTutorialTrigger");
            }).AddTo(_disposables);

            RunnerSkillSystem?.RunnerObservable.RunStream.Subscribe((arrayTouch) =>
            {
                if (arrayTouch != Vector3.zero)
                    tutorial.GetComponentInChildren<Animator>().SetTrigger("NextTutorialTrigger");
            }).AddTo(_disposables);

            RunnerSkillSystem?.RunnerObservable.OnFoundKeyStream
                .Where(keyCout => keyCout >= _targetToWin)
                .Subscribe(_ => 
                {
                    tutorial.SetActive(false);
                }).AddTo(_disposables);
        }

        private void InitCoinOnMapGameItems()
        {
            CoinOnMapGameItems = new List<GameObject>();
            GameObject coinGameItem = GameManager.Instance.Resources.CoinGameItem;
            if (coinGameItem == null) return;
            for (int i = 0; i < _playingMap.TreasurePositions.Count; i++)
            {
                CoinOnMapGameItems.Add(Instantiate(coinGameItem, _playingMap.TreasurePositions[i].transform.position, Quaternion.identity, MapHolder.transform));
            }

        }

        protected override void End()
        {
            base.End();
            foreach (var coinObject in CoinOnMapGameItems)
            {
                Destroy(coinObject);
            }
            CoinOnMapGameItems.Clear();

            if (countDownUpgradeSupportItem != null)
                StopCoroutine(countDownUpgradeSupportItem);

            Destroy(_exitDoorGameObject);

            _disposablesObservable.Clear();
        }

        protected override void Reload()
        {
            _targetToWin = 0;
            isWin = false;
            HasTriedOneTimeGetWeapon = false;
            base.Reload();
        }

        private void OnDisable()
        {
            _disposables?.Clear();
        }
        private void OnDestroy()
        {
            _disposables?.Clear();
        }

    }
}