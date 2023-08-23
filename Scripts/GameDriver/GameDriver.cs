using _SDK.Money;
using Assets._SDK.Game;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using Assets._GAME.Scripts.Skills.Live;
using _GAME.Scripts.Inventory;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEditor;
using Assets._SDK.GamePlay;
using Assets._GAME.Scripts.Skills;
using Button = UnityEngine.UI.Button;
using Assets._GAME.Scripts.GamePlay;

namespace Assets._GAME.Scripts.Game
{
    [DefaultExecutionOrder(1)]
    public class GameDriver : AbstractGameDriver
    {
        public static GameDriver I => (GameDriver)Instance;

        public GameObject Runner;

        public Button HackButton;
        private bool _isActiveHackButton = false;

        private void Start()
        {
            HackButton?.onClick.AddListener(() => { HideSkillButton(_isActiveHackButton); });
        }

        private void HideSkillButton(bool isActive)
        {
            var GameplayingPanel = GamePlayLoader.Instance.GameSceneUI.GamePlayingMainPanelUI;
            var gameInputPanel = GamePlayLoader.Instance.GameInputPanel;
            if (GameplayingPanel == null || gameInputPanel == null) return;

            GameplayingPanel.transform.Find("ButtonBuffHP").gameObject.SetActive(isActive);
            GameplayingPanel.transform.Find("ButtonBuffStamina").gameObject.SetActive(isActive);
            GameplayingPanel.transform.Find("ButtonBuffInvisible").gameObject.SetActive(isActive);
            GameplayingPanel.transform.Find("ButtonBuffScan").gameObject.SetActive(isActive);
            gameInputPanel.transform.Find("ButtonRun").gameObject.SetActive(isActive);
            gameInputPanel.transform.Find("ButtonJump").gameObject.SetActive(isActive);
            gameInputPanel.transform.Find("ButtonLookBehind").gameObject.SetActive(isActive);

            _isActiveHackButton = !isActive;
        }

		[BoxGroup("Player Data")]
		[Button("Deposit 100 Coin", ButtonSizes.Medium)]
		public void Deposit100Coin()
        {
            GameManager.Instance.Wallet.Deposit(new Price(Currency.Coin, 1000));
            Debug.Log(GameManager.Instance.Wallet.GetAccountBy(Currency.Coin).Balance.Value);
        }

        [Button("Decrease 10 HealthPoint Runner", ButtonSizes.Medium)]
        public void DecreaseHealthPoint()
        {
            Runner = GameObject.Find("Runner");
            LiveRunnerSkillBehavior liveRunnerSkillBehavior = Runner.GetComponent<LiveRunnerSkillBehavior>();
            liveRunnerSkillBehavior.OnDecreaseHealth(10);

            _SDK.Logger.DebugPro.RedItalic("Decrease 10 HP");
        }

		[BoxGroup("Mission Data")]
        public int OrderMission = 1;
		[BoxGroup("Mission Data")]
		[Button("Active Mission", ButtonSizes.Medium)]
		public void ActiveMisson()
		{
			GameManager.Instance.MissionInventory.SetActiveMission(OrderMission - 1);
		}
		[BoxGroup("Mission Data")]
		[Button("Own Mission", ButtonSizes.Medium)]
		public void OwnMisson()
		{
			GameManager.Instance.MissionInventory.SetOwnMission(OrderMission - 1);
		}

        [BoxGroup("Mission Data")]
        [Button("Win Mission", ButtonSizes.Medium)]
        public void WinMisson()
        {
            var missionOrder = GameManager.Instance.MissionInventory;

            if (missionOrder.GetIndexActiveMisionSetting() < GameManager.Instance.Resources.AllMissionSettings.Count)
            {
                GameManager.Instance.MissionInventory.SetWinMission(missionOrder.GetIndexActiveMisionSetting());
                missionOrder.SetActiveMission(missionOrder.GetIndexActiveMisionSetting());
            }
            GameManager.Instance.Fire(GameTrigger.End);
        }


        [Button("Lose", ButtonSizes.Medium)]
        public void Lose()
        {
            if (GamePlay.GamePlayLoader.Instance.CurrentGamePlay.CanFire(GamePlayTrigger.Lose))
            {
                GamePlay.GamePlayLoader.Instance.CurrentGamePlay.Fire(GamePlayTrigger.Lose);
                Debug.Log("Lose");
            }
        }

        [Button("Win", ButtonSizes.Medium)]
        public void Win()
        {
            if (GamePlay.GamePlayLoader.Instance.CurrentGamePlay.CanFire(GamePlayTrigger.Win))
            {
                GamePlay.GamePlayLoader.Instance.CurrentGamePlay.Fire(GamePlayTrigger.Win);
                Debug.Log("Win");
            }
        }

        [Button("Pause", ButtonSizes.Medium)]
        public void Pause()
        {
            GamePlay.GamePlayLoader.Instance.CurrentGamePlay.Fire(GamePlayTrigger.Pause);
        }

        [Button("UnPause", ButtonSizes.Medium)]
        public void UnPause()
        {
            GamePlay.GamePlayLoader.Instance.CurrentGamePlay.Fire(GamePlayTrigger.Play);
            GameManager.Instance.OnUnPause();
        }

		[Button("Visible", ButtonSizes.Medium)]
		public void Visible()
		{
            GamePlay.GamePlayLoader.Instance.CurrentGamePlay.Runner?.GetComponent<RunnerSkillSystem>()?.RunnerObservable?.InvisibleRunner?.Invoke(true);
		}

		[Button("UnVisible", ButtonSizes.Medium)]
		public void UnVisible()
		{
			GamePlay.GamePlayLoader.Instance.CurrentGamePlay.Runner?.GetComponent<RunnerSkillSystem>()?.RunnerObservable?.InvisibleRunner?.Invoke(false);
		}
	}
}