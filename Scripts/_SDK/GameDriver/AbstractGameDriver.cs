using System;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using _SDK.Money;
using UniRx;
using UnityEngine.UI;
using TMPro;

namespace Assets._SDK.Game
{
    public class AbstractGameDriver : GameSingleton<AbstractGameDriver>
    {
        [GUIColor(252f / 255f, 186f / 255f, 3f / 255f), ReadOnly]
        public string previousState, currentState;

        public bool StartGameImmediately = true;

        public GameObject DriverPanel;
        public Button GameHackerButton;
        public TextMeshProUGUI GameHackerButtonText;

        protected override void OnAwake()
        {
            GameHackerButton?.onClick.AddListener(SwitchDriverPanel);
        }

        private IEnumerator Start()
        {
            GameManager.Instance.CurrentState.Subscribe( value =>
            {
                currentState  = value.ToString();
            });

            GameManager.Instance.PreviousState.Subscribe(value =>
            {
                previousState = value.ToString();
            });

            if (StartGameImmediately)
            {
                yield return new WaitForSeconds(1);
                StartGame();
            }

           DriverPanel.SetActive(false);
           yield return null;
        }

        [BoxGroup("Navigation")]
        [Button("Start Game", ButtonSizes.Medium)]
        public void StartGame()
        {
            GameManager.Instance.Fire(GameTrigger.Play);
        }

        [BoxGroup("Navigation")]
        [Button("Go Shopping", ButtonSizes.Medium)]
        public void GoShopping()
        {
            GameManager.Instance.Fire(GameTrigger.GoShopping);
        }
        [BoxGroup("Navigation")]
        [Button("Back To Lobby", ButtonSizes.Medium)]
        public void BackToLobby()
        {
            if(GameManager.Instance.CurrentState.Value == GameState.ReviewingEnemy)
                GameManager.Instance.Fire(GameTrigger.BackToLobbyHome);
            else
				GameManager.Instance.Fire(GameTrigger.End);
		}

        void SwitchDriverPanel()
        {
           DriverPanel.SetActive(!DriverPanel.activeSelf);
           GameHackerButtonText.text = DriverPanel.activeSelf ? "Hide Game Hacker" : "Show Game Hacker";

        }    

    }
}