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
using Assets._SDK.Input;

namespace Assets._GAME.Scripts.GamePlay
{
	public class SeekAndKillGamePlay : AbstractNextBotGamePlay
	{
		public static SeekAndKillGamePlay Instance => (SeekAndKillGamePlay)GamePlayLoader.Instance.CurrentGamePlay;

		List<GameObject> CoinOnMapGameItems;

		private const int MISSION_OVER_TIME = 40;

		public int QualityKill => _qualityKill;

		private int _qualityKill;
		private float _targetToWin; 
		public float TargetToWin => _targetToWin;

		IEnumerator countDownUpgradeSupportItem;
		CompositeDisposable _disposables;
		CompositeDisposable _disposablesObservable;

		public Skin CollectingSkin { get; set; }

		bool isWin;

		public bool HasTriedOneTimeGetWeapon { get; set; }

		public SeekAndKillObservable SeekAndKillObservable;
		protected override void OnStart()
		{
			gamePlayingMainPanel?.StartUISeekAndKill(_targetToWin);

            SeekAndKillObservable = gameObject.GetComponent<SeekAndKillObservable>();
            if (SeekAndKillObservable == null)
                SeekAndKillObservable = gameObject.AddComponent<SeekAndKillObservable>();

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

			Subcribe();
        }
		private void Subcribe()
        {
            float missionOrderTutorial = 3f;

            _disposablesObservable = new CompositeDisposable();

            SeekAndKillObservable.OnRemoveSlotStream
				.Subscribe(nameMonster =>
				{
					ClearMonsterDeadInRegionalSlot(nameMonster);
				}).AddTo(_disposablesObservable);

            SeekAndKillObservable.OnMonsterDeadStream
                .Subscribe(nameMonster =>
                {
					ClearMonsterDeadInRegionalSlot(nameMonster);
                    RunnerSkillSystem.RunnerObservable.SetRegionalSlot(nameMonster);
                }).AddTo(_disposablesObservable);
        }
		private void ClearMonsterDeadInRegionalSlot(string name)
		{
			RegionalEnemySlots = RegionalEnemySlots.Where(v => v.Enemy.name != name).ToList();
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
					GameObject SupportItemsPrefab = UnityEngine.Object.Instantiate(WeaponsSpawn[i%WeaponsSpawn.Count].ModelItemSupport, SupportItemSpawnPoints[i].transform.position, Quaternion.identity, MapHolder.transform);

					SupportItemsPrefab.GetComponent<ItemSupportSkillUpgrade>().InitData(WeaponsSpawn[i % WeaponsSpawn.Count]);

					SkillSupportItems.Add(SupportItemsPrefab);
				}
				SupportItemSpawnPoints.Clear();
			}
			else{
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

			RunnerSkillSystem.RunnerObservable.SetRegionalSlot("");
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
		public void ReceiveKilledEnemy(int value, Skin Skin = null)
		{
			_qualityKill += value;

			if (CurrentState.Value != GamePlayState.Running)
				return;

			if (_qualityKill >= _targetToWin)
			{
				_qualityKill = (int)_targetToWin;
				RunnerSkillSystem.UnSubscribeSkillWhenWin();
				RunnerSkillSystem.RunnerObservable.ClearObservers();
				isWin = true;
				StartCoroutine(DelayToWin());
			}
			gamePlayingMainPanel?.UpdateTargetSeekAndKill(_qualityKill, Skin);

			if (playingMission.TutorialUIEnemy != null)
			{
				RETutorialSlot?.DisableLineDirectionTutorial();
			}
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

		public void OnEnemyDeath(Skin skin)
        {
			ReceiveKilledEnemy(1, skin);

			if (skin.IsOwned == false)
			{
				skin.Own();
				gamePlayingMainPanel?.UpdateTargetGallerySeekAndKill(_qualityKill, skin);
			}
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
				{
					tutorial?.SetActive(false);
					_disposables.Clear();
				}


			}).AddTo(_disposables);

			RunnerSkillSystem?.RunnerObservable.RunStream.Subscribe((arrayTouch) =>
			{
				if (arrayTouch != Vector3.zero)
				{
					tutorial.SetActive(false);
					_disposables.Clear();
				}
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

			_disposablesObservable.Clear();
        }

		protected override void Reload()
		{
			_targetToWin = 0;
			isWin = false;
			HasTriedOneTimeGetWeapon = false;
			base.Reload();
		}

		private void OnDestroy()
		{
			_disposables?.Clear();
			_disposablesObservable.Clear();

        }
	}
}