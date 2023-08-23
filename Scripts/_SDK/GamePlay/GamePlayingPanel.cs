using Assets._SDK.Game;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Assets._SDK.Input;
using UniRx;
using Assets._GAME.Scripts.Skills;
using Assets._GAME.Scripts.GamePlay;
using _GAME.Scripts.Inventory;
using Assets._SDK.GamePlay;
using Sirenix.OdinInspector;
using System;
using Assets._SDK.Ads;
using Assets._SDK.Analytics;

namespace _SDK.UI
{
    public class GamePlayingPanel : AbstractPanel
    {
        [SerializeField]
        [BoxGroup("Component")] protected SurvivalTargetHandle SurvivalTargetHandle;
        [SerializeField]
        [BoxGroup("Component")] protected TreasureTargetHandle TreasureTargetHandle;
        [SerializeField]
        [BoxGroup("Component")] protected SeekAndKillTargetHandle SeekAndKillTarget;
		[SerializeField]
		[BoxGroup("Component")] protected SeekAndKillTargetCollectHandle SeekAndKillCollectTarget;

		[BoxGroup("Button")] public Button PauseButton;
        [BoxGroup("Button")] public Button HomeButton;
		[BoxGroup("Button")] public Button BuffMaxHPButton;
		[BoxGroup("Button")] public Button BuffMaxStaminaButton;
		[BoxGroup("Button")] public Button BuffInvisibleButton;
		[BoxGroup("Button")] public Button BuffScanleButton;

		[BoxGroup("HealthPoint")] public Slider HealthSlider;
        [BoxGroup("HealthPoint")] public TextMeshProUGUI HealthText;
		[BoxGroup("HealthPoint")] public GameObject HPMaxSprite;

		[BoxGroup("StaminaPoint")] public Slider StaminaSlider;
        [BoxGroup("StaminaPoint")] public TextMeshProUGUI StaminaText;
		[BoxGroup("StaminaPoint")] public GameObject StaminaMaxSprite;

        [BoxGroup("Animation")] public Animator BloodAnimator;
        [BoxGroup("Animation")] private const string NAME_ANIMATOR_BLOOD = "BloodAnimation";
		[BoxGroup("Animation")] public Image LowHPImage;

		[BoxGroup("Text")] public TMP_Text Tutorial;
        [BoxGroup("Text")] public TMP_Text TutorialImportant;
		[BoxGroup("Text")] public TMP_Text ValueCoinUI;

		[BoxGroup("Image")] public Image ImgAds;
		private const string NAME_ANIMATOR_TUTORIAL = "TutorialTextAnimation";

        [SerializeField]
        [BoxGroup("Object")] private GameObject _runner;

        [SerializeField]
        [BoxGroup("Object")] public GameObject TutorialGuide;

        CompositeDisposable _disposables;

        Animator TutorialImportantAnimator;

		RunnerObservable runnerObservable;

		public void Init()
        {

			UpdateMaxHP(false);
			UpdateMaxStamina(false);

			runnerObservable = _runner.GetComponent<RunnerSkillSystem>().RunnerObservable;
						_disposables = new CompositeDisposable();
			runnerObservable.HealthStream.Subscribe((healthPoint) =>
            {
                HealthPointUI(healthPoint);
            }).AddTo(_disposables);

			runnerObservable.StaminaStream.Subscribe((staminaPoint) =>
			{
				StaminaPointUINew(staminaPoint);
			}).AddTo(_disposables);

			runnerObservable.IsRunMaxStamina.Subscribe((isMaxStamina) =>
			{
				StaminaText.text = "";
                UpdateMaxStamina(isMaxStamina);
			}).AddTo(_disposables);

			runnerObservable.IsRunnerMaxHP.Subscribe((isMaxHP) =>
			{
				HealthText.text = "";
				UpdateMaxHP(isMaxHP);
		    }).AddTo(_disposables);

		GameManager.Instance.Wallet.DefaultAccount.Balance.Subscribe((value) =>
			{
				ValueCoinUI?.SetText(value.ToString());
			}).AddTo(this);
        }

        private void Start()
        {
            TutorialImportantAnimator = TutorialImportant.GetComponent<Animator>();

			HomeButton.onClick.AddListener(Lobby);
            PauseButton.onClick.AddListener(Pause);

			BuffMaxHPButton.onClick.AddListener(BuffMaxHP);
			BuffMaxStaminaButton.onClick.AddListener(BuffMaxStamina);
			BuffInvisibleButton.onClick.AddListener(BuffInvisible);
			BuffScanleButton.onClick.AddListener(BuffScan);
			
			ReloadUI();
        }

        public void ReloadUI()
        {
			ImgAds.enabled = false;
			Init();
            ShowTargetModeMission();
            UpdateTutorial();
        }

        private void Lobby()
        {
            GameManager.Instance.Fire(GameTrigger.End);
        }
        private void Pause()
        {
            GamePlayLoader.Instance.CurrentGamePlay.Fire(GamePlayTrigger.ShowPausePanel);
        }

        private void BuffMaxHP()
        {
			AdsManager.Instance.ShowRewarded(isSuccess =>
			{
				if (isSuccess == AdsResult.Success)
				{
					runnerObservable?.MaxHP.Invoke(true);
				}
			}, GamePlayLoader.Instance.PlayingMission.Order, AnalyticParamKey.REWARD_HP);
		}

		private void BuffMaxStamina()
		{
			AdsManager.Instance.ShowRewarded(isSuccess =>
			{
				if (isSuccess == AdsResult.Success)
				{
					runnerObservable?.MaxStamina.Invoke(true);
				}
			}, GamePlayLoader.Instance.PlayingMission.Order, AnalyticParamKey.REWARD_STAMINA);
		}

		private void BuffInvisible()
		{
			AdsManager.Instance.ShowRewarded(isSuccess =>
			{
				if (isSuccess == AdsResult.Success)
				{
					runnerObservable?.InvisibleRunner.Invoke(true);
				}
			}, GamePlayLoader.Instance.PlayingMission.Order, AnalyticParamKey.REWARD_HP);
		}

		private void BuffScan()
		{
			AdsManager.Instance.ShowRewarded(isSuccess =>
			{
				if (isSuccess == AdsResult.Success)
				{
					runnerObservable?.ScanAllEnemies.Invoke(true);
				}
			}, GamePlayLoader.Instance.PlayingMission.Order, AnalyticParamKey.REWARD_HP);
		}

		private void ShowTargetModeMission()
        {
            DisableTarget();
            var Mission = GamePlayLoader.Instance.CurrentGamePlay.ModeGameMission;
            switch (Mission)
            {
                case ModeGameMission.Survival:
                    SurvivalTargetHandle.gameObject.SetActive(true);
                    break;

                case ModeGameMission.Treasure:
                    TreasureTargetHandle.gameObject.SetActive(true);
                    break;

                case ModeGameMission.SeekAndKill:
                    SeekAndKillTarget.gameObject.SetActive(true);
                    break;
				case ModeGameMission.SeekAndKillCollect:
					SeekAndKillCollectTarget.gameObject.SetActive(true);
					break;
			}
        }
        private void DisableTarget()
        {
            SurvivalTargetHandle.gameObject.SetActive(false);
            TreasureTargetHandle.gameObject.SetActive(false);
            SeekAndKillTarget.gameObject.SetActive(false);
			SeekAndKillCollectTarget.gameObject.SetActive(false);

		}

        public void TriggerBloodAnimation()
        {
            BloodAnimator.SetTrigger(NAME_ANIMATOR_BLOOD);
        }

        private void UpdateTutorial()
        {
            HandleTextTutorials();
			TutorialImportantAnimator?.SetTrigger("TutorialAnimation");
            Tutorial.GetComponent<Animator>().SetTrigger("TutorialAnimation");
        }
        private void HandleTextTutorials()
        {
            var currentMission = GamePlayLoader.Instance.PlayingMission;

			TutorialImportant.SetText(currentMission.TutorialStringImportant.Replace("XX", currentMission.TargetWin.ToString()));

            Tutorial.SetText(currentMission.TutorialString);
        }

		public void SetTextUgradingCountdown(string content, int TargetValue)
		{
			ImgAds.enabled = TargetValue > 0;
			TutorialImportantAnimator?.SetBool("IsEnable", TargetValue > 0);
			TutorialImportant.SetText(content + $"<color=red>{TargetValue}</color>");
		}

		#region HealthPoint
		public void HealthPointUI(UpdateHPStaminaSetting healthPoint)
        {
            HealthText.text = ((int)healthPoint.CurrentValue).ToString();
            HealthSlider.maxValue = healthPoint.MaxValue;
            HealthSlider.value = healthPoint.CurrentValue;
			var tempColor = LowHPImage.color;
			tempColor.a = 1 - (healthPoint.CurrentValue/ (healthPoint.MaxValue * 0.75f)) ;
			LowHPImage.color = tempColor;

		}
		public void StaminaPointUINew(UpdateHPStaminaSetting StaminaPoint)
		{
			StaminaText.text = ((int)StaminaPoint.CurrentValue).ToString();
            StaminaSlider.maxValue = StaminaPoint.MaxValue;
            StaminaSlider.value = StaminaPoint.CurrentValue;
        }
		#endregion

		public void UpdateMaxStamina(bool isActive)
        {
			if(StaminaMaxSprite == null) return;

			StaminaMaxSprite.SetActive(isActive);
            StaminaSlider.gameObject.SetActive(!isActive);
        }

		public void UpdateMaxHP(bool isActive)
		{
			if (HPMaxSprite == null) return;

			HPMaxSprite.SetActive(isActive);
			HealthSlider.gameObject.SetActive(!isActive);
		}


		private void OnDestroy()
        {
            _disposables?.Clear();
        }
    }
}