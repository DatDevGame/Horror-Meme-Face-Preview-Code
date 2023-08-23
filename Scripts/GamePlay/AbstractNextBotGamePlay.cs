using _GAME.Scripts;
using _GAME.Scripts.Inventory;
using Assets._SDK.GamePlay;
using System;
using System.Collections.Generic;
using UnityEngine;
using Assets._GAME.Scripts.Enemies;
using Assets._GAME.Scripts.Skills;
using Assets._SDK.Toolkit;
using Assets._SDK.Analytics;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections;
using Assets._SDK.Ads;
using UniRx;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Sirenix.OdinInspector;
using UnityEngine.Animations;
using static UnityEngine.GraphicsBuffer;

namespace Assets._GAME.Scripts.GamePlay
{
	public abstract class AbstractNextBotGamePlay : AbstractGamePlay
	{
		const float PERCENT_RE_SLEEP = 20f;
		const float PERCENT_RE_PATROL = 20f;

		private AsyncOperation _sysUnloader;
		public AsyncOperation SysUnloader => _sysUnloader;

		MissionInventory _missionInventory;

		private const string ITEMS = "Items";

		public ModeGameMission ModeGameMission { get => playingMission.ModeGamePlay; }

		public int PrizeMission => playingMission.Prize;

		public GameObject Runner;
		public GameObject MapHolder;
		public List<RegionalEnemySlot> ListRegionalEnemySlots => RegionalEnemySlots;
		public List<RegionalEnemySlot> ListRegionalEnemyHasKeySlots => regionalEnemyKeySlots;

        protected RunnerSkillSystem RunnerSkillSystem;

		private GameInputPanel _gameInputPanel;
        protected List<RegionalEnemySlot> RegionalEnemySlots;
        protected List<RegionalEnemySlot> regionalEnemyKeySlots;
		protected List<GlobalEnemySlot> GlobalEnemySlots;
		protected List<GameObject> RegionalEnemySpawnPoints, GlobalEnemySpawnPoints, ShuffledRegionalEnemySpawnPoints, SupportItemSpawnPoints;
		protected List<EnemyGroup> EnemyGroups;
		List<EnemyGroup> EnemyGroupsRE;
		protected List<GameObject> TrapEnemies;
		protected List<GameObject> SkillSupportItems;
		MaxStaminaSlot maxStaminaSlot;

		protected RegionalEnemySlot RETutorialSlot;

		protected int amountRE, amountGE;
		protected int AmountRESleep;

		protected Map _playingMap => playingMission?.MapSetting.map;
		protected Mission playingMission => (Mission)_missionInventory?.PlayingMission;

		protected GamePlayingMainPanel gamePlayingMainPanel => GamePlayLoader.Instance.GameSceneUI.GamePlayingMainPanelUI;

		private int randomNumber;
		protected bool IsEnemySpawnIgnored(int index) => playingMission.MustReduceHalfEnemy && (index % 2) == randomNumber;

		Scene sceneMapCurrent;
		float percentSleep = PERCENT_RE_SLEEP;
		float percentPatrol = PERCENT_RE_PATROL;

		protected AudioSource audioSource;
		protected AudioClip soundBreakTime;

		private UniversalRendererData _universalRendererData;
		public UniversalRendererData UniversalRendererData => _universalRendererData;
		public bool IsMissionTutorial => playingMission.TutorialUIEnemy != null;

		private void Start()
		{
			GamePlayLoader.Instance.GameSceneUI.SkillSupportItemUI.InitByMissionMaxLength();
			StartCoroutine(ChangeStateUpgradeSkill());
			OnStart();

			var soundManager = GameManager.Instance.SoundManager;
			soundManager.SetUpSound(soundManager.IsOnSound);
		}

		protected abstract void OnStart();

		protected override void Pause()
		{
			base.Pause();
			GameManager.Instance.OnPause();
			GameManager.Instance.SoundManager.PauseSound(false);
		}
		protected override void UnPause()
		{
			base.UnPause();
			GameManager.Instance.OnUnPause();
			var soundManager = GameManager.Instance.SoundManager;
			soundManager.PauseSound(soundManager.IsOnSound);
		}

		public void Init(GameInputPanel gameInput, GameObject mapHolder, GameObject runner, UniversalRendererData URendererData)
		{
			_universalRendererData = URendererData;
			_missionInventory = GameManager.Instance.MissionInventory;
			_gameInputPanel = gameInput;

			MapHolder = mapHolder;
			Runner = runner;
			percentSleep = playingMission.PercentSleepEnemy;
			percentPatrol = playingMission.PercentPatrolEnemy;

			GlobalEnemySlots = new List<GlobalEnemySlot>();
			RegionalEnemySlots = new List<RegionalEnemySlot>();
            regionalEnemyKeySlots = new List<RegionalEnemySlot>();


            InitMap();
            InitRunner();
            InitSkillSupportItemSettings();
			InitMaxStaminaSettings();
			StartCoroutine(DalayforInitEnemies());
			OnInitGamePlay();
			//OnInitGamePlay();
			//GameSceneUI.GetComponentInChildren<GamePlayingMainPanel>().Init();
		}

		protected IEnumerator ChangeStateUpgradeSkill()
		{
			yield return new WaitForEndOfFrame();
			Fire(GamePlayTrigger.UpgradeSkill);
		}

		IEnumerator DalayforInitEnemies()
		{
			yield return new WaitForEndOfFrame();
			InitEnemies();
		}

		protected abstract void OnInitGamePlay();

		protected virtual void InitMap()
		{
			var parameters = new LoadSceneParameters(LoadSceneMode.Additive);
			sceneMapCurrent = SceneManager.LoadScene(_playingMap.Name, parameters);
			InitJumpScareForMap();
		}

		private void InitJumpScareForMap()
		{
			TrapEnemies = playingMission.InitJumpScare();
		}
		protected virtual void InitEnemies()
		{
			randomNumber = UnityEngine.Random.Range(0, 2);
			RegionalEnemySpawnPoints = new List<GameObject>();
			GlobalEnemySpawnPoints = new List<GameObject>();
			EnemyGroups = new List<EnemyGroup>();
			EnemyGroupsRE = new List<EnemyGroup>();

			if (playingMission.RESpawnPoints.Count > 0)
			{
				for (int i = 0; i < playingMission.RESpawnPoints.Count; i++)
					RegionalEnemySpawnPoints.Add(_playingMap.regionalEnemyPositions[playingMission.RESpawnPoints[i] - 1]);
			}

			for (int i = 0; i < playingMission.EnemyGroups.Count; i++)
			{
				var enemyGroup = new EnemyGroup();
				enemyGroup.Amount = playingMission.EnemyGroups[i].Amount;
				enemyGroup.Type = playingMission.EnemyGroups[i].Type;
				enemyGroup.level = playingMission.EnemyGroups[i].level;
				enemyGroup.EnemySettings = playingMission.EnemyGroups[i].EnemySettings;
				enemyGroup.HasKey = playingMission.EnemyGroups[i].HasKey;
                if (enemyGroup.Type == EnemyType.Regional)
					EnemyGroupsRE.Add(enemyGroup);

				EnemyGroups.Add(enemyGroup);
			}

			InitRESleepAndPatrol();

			if (playingMission.GESpawnPoints.Count > 0)
			{
				for (int i = 0; i < playingMission.GESpawnPoints.Count; i++)
					GlobalEnemySpawnPoints.Add(_playingMap.globalEnemyPositions[playingMission.GESpawnPoints[i] - 1]);
			}

			amountRE = amountGE = 0;
		}

		void InitRESleepAndPatrol()
		{
			ShuffledRegionalEnemySpawnPoints = new List<GameObject>();
			ShuffledRegionalEnemySpawnPoints.Clear();

			if (RegionalEnemySpawnPoints.Count > 0)
			{
				for (int i = 0; i < RegionalEnemySpawnPoints.Count; i++)
					ShuffledRegionalEnemySpawnPoints.Add(RegionalEnemySpawnPoints[i]);
			}
			ShuffledRegionalEnemySpawnPoints.Shuffle();

			AmountRESleep = 0;
			//Init RE0
			AmountRESleep =(int) (percentSleep * RegionalEnemySpawnPoints.Count /100);

			InitRESpecal(AmountRESleep, RegionalEnemyType.RE0);

			//Init RE1
			int AmountREPatrol = (int) (percentPatrol * (RegionalEnemySpawnPoints.Count) /100);
			InitRESpecal(AmountREPatrol, RegionalEnemyType.RE1);


			if (playingMission.TutorialUIEnemy != null)
			{
				RETutorialSlot = new RegionalEnemySlot(MapHolder);
				RETutorialSlot?.AddDirectionTutorial(playingMission, Runner);
				RETutorialSlot?.DisableLineDirectionTutorial();
				RETutorialSlot.REObservable.SetKey(true);
				RegionalEnemySlots.Add(RETutorialSlot);

                if (RETutorialSlot.REObservable.HasKey)
                {
                    GameObject keyPrefab = Instantiate(GameManager.Instance.Resources.Key, Vector3.zero, Quaternion.identity, RETutorialSlot.Enemy.transform);
                    RectTransform keyRect = keyPrefab.GetComponent<RectTransform>();
                    keyRect.SetLocalPositionAndRotation(new Vector3(-0.15f, 1.5f, 0.25f), Quaternion.Euler(Vector3.zero));
                    regionalEnemyKeySlots.Add(RETutorialSlot);

                    //Key Look At Runner
                    KeyLookAtTargetHandle(keyPrefab, Runner.transform);
                }
            }
		}

        protected void KeyLookAtTargetHandle(GameObject keyPrefab, Transform targetLookAt)
        {
            ConstraintSource constraintSource = new ConstraintSource();
            constraintSource.sourceTransform = targetLookAt;
			constraintSource.weight = 1f;

            LookAtConstraint lookAtConstraint = keyPrefab.AddComponent<LookAtConstraint>();
			lookAtConstraint.constraintActive = true;
            lookAtConstraint.AddSource(constraintSource);
        }

        protected void InitRESpecal(int AmountRE, RegionalEnemyType typeRE = RegionalEnemyType.RE2)
		{
			for (int reIndex = 0; reIndex < AmountRE; reIndex++)
			{
				int indexTypeEnemy = UnityEngine.Random.Range(0, EnemyGroupsRE.Count - 1);

				if (IsEnemySpawnIgnored(reIndex))
					continue;

				RegionalEnemySlot RESlot = new RegionalEnemySlot(MapHolder);

				GameObject randomRegionalPoint = ShuffledRegionalEnemySpawnPoints.First();

				RESlot.AddBehaviorIfNull(randomRegionalPoint, EnemyGroupsRE[indexTypeEnemy].level, typeRE);
				RESlot.AddSkinIfNull(EnemyGroupsRE[indexTypeEnemy].EnemySettings.skin);
				RESlot.REObservable.SetKey(EnemyGroupsRE[indexTypeEnemy].HasKey);

                if (RESlot.REObservable.HasKey)
                {
                    GameObject keyPrefab = Instantiate(GameManager.Instance.Resources.Key, Vector3.zero, Quaternion.identity, RESlot.Enemy.transform);
                    RectTransform keyRect = keyPrefab.GetComponent<RectTransform>();
                    keyRect.SetLocalPositionAndRotation(new Vector3(-0.15f, 1.5f, 0.25f), Quaternion.Euler(Vector3.zero));
					regionalEnemyKeySlots.Add(RESlot);

					//Key Look At Runner
					KeyLookAtTargetHandle(keyPrefab, Runner.transform);
                }

                EnemyGroupsRE[indexTypeEnemy].Amount--;
				if (EnemyGroupsRE[indexTypeEnemy].Amount == 0)
				{
					EnemyGroups.Remove(EnemyGroupsRE[indexTypeEnemy]);
					EnemyGroupsRE.RemoveAt(indexTypeEnemy);
				}

				RegionalEnemySpawnPoints.Remove(randomRegionalPoint);
				ShuffledRegionalEnemySpawnPoints.Remove(randomRegionalPoint);

				RegionalEnemySlots.Add(RESlot);
				//if (playingMission.TutorialUIEnemy != null)
				//	RESlot.AddTutorialUI(playingMission.TutorialUIEnemy);
			}
		}
		protected virtual void InitRunner()
		{
			List<GameObject> listPointSpawnRunner = _playingMap.runerPositions;
			Vector3 runnerPosition = listPointSpawnRunner[playingMission.RunnerPosition-1].transform.position;
			Vector3 runnerRotation = listPointSpawnRunner[playingMission.RunnerPosition - 1].transform.eulerAngles;
			Runner.GetComponent<CharacterController>().enabled = false;
			Runner.transform.position = runnerPosition;
			Runner.GetComponent<CharacterController>().enabled = true;
			RunnerSkillSystem = Runner.GetComponent<RunnerSkillSystem>();
			RunnerSkillSystem.Init(_gameInputPanel);
			Runner.transform.localRotation = Quaternion.Euler(0, runnerRotation.y, 0);
		}

		protected virtual void InitSkillSupportItemSettings()
		{
			SkillSupportItems = new List<GameObject>();
			SupportItemSpawnPoints = new List<GameObject>();

			foreach (var point in _playingMap.SupportItems)
				SupportItemSpawnPoints.Add(point);
			SupportItemSpawnPoints.Shuffle();

			SkillSupportItems = playingMission.InitGameObjectSupportItems(SupportItemSpawnPoints, MapHolder.transform);
		}

		protected virtual void InitMaxStaminaSettings()
		{
			if (playingMission.MaxStaminaSettings == null) return;

			maxStaminaSlot = new MaxStaminaSlot(playingMission.MaxStaminaSettings.SupportItem);
			maxStaminaSlot.AttachTo(MapHolder);
			maxStaminaSlot.Model.transform.position = Runner.transform.position + Runner.transform.forward * 6;

			maxStaminaSlot.Model.transform.position += new Vector3(0, -0.5f, 0);
		}
		protected override void Reload()
		{
			_gameInputPanel.ResetUI();
			GamePlayLoader.Instance.LoadGamePlay();
		}

		protected override void JumpScare()
		{
			RunnerSkillSystem?.RemoveSkill();
			RunnerSkillSystem?.RunnerObservable?.ClearObservers();
			GamePlayLoader.Instance.GameSceneUI.JumpScarePanelUI.ShowJumpScare(() => End());
		}

		protected override void End()
		{
			if (maxStaminaSlot != null)
				Destroy(maxStaminaSlot.Model);
			maxStaminaSlot = null;

			DestroySkillSupportItems();

			DestroyTrapEmemies();
			DestroyEnemies();
			RunnerSkillSystem?.RemoveSkill();
			RunnerSkillSystem?.RunnerObservable?.ClearObservers();

			if (GamePlayLoader.Instance.CurrentGamePlay.IsMissionTutorial != true)
				ShowInterstitialAd();

			StartCoroutine(DelayToUnloadAsset(1f));
		}

		IEnumerator DelayToUnloadAsset(float timeDelay = 0)
		{
			yield return new WaitForSeconds(timeDelay);
			if (sceneMapCurrent != null)
				SceneManager.UnloadSceneAsync(sceneMapCurrent);
			GC.Collect(2, GCCollectionMode.Forced);
			_sysUnloader = Resources.UnloadUnusedAssets();

			if (_sysUnloader != null)
			{
				yield return new WaitUntil(() => _sysUnloader.isDone);
			}
		}

		private void ShowInterstitialAd()
        {
			string placement = CurrentState.Value == GamePlayState.Won ? AnalyticParamKey.MISSION_WIN : AnalyticParamKey.MISSION_LOSE;

			AdsManager.Instance.ShowInterstitial(playingMission.Order, placement);

			Debug.Log("Show Ads");
        }

		public void DestroyEnemies()
		{
			foreach (var Enemy in RegionalEnemySlots)
			{
				if (Enemy?.Enemy != null)
					Destroy(Enemy.Enemy);
			}
			RegionalEnemySlots.Clear();

			foreach (var Enemy in GlobalEnemySlots)
			{
				if(Enemy?.Enemy != null)
					Destroy(Enemy.Enemy);
			}
			GlobalEnemySlots.Clear();
		}

		public void DestroyTrapEmemies()
		{
			if(TrapEnemies == null)	return;

			foreach (var Trap in TrapEnemies)
			{
				Destroy(Trap);
			}
			TrapEnemies.Clear();
		}

		public void DestroySkillSupportItems()
		{
			if (SkillSupportItems == null) return;

			foreach (var Item in SkillSupportItems)
			{
				Destroy(Item);
			}
			SkillSupportItems.Clear();
		}

	}
}