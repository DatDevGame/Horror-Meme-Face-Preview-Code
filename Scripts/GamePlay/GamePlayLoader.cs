using _GAME.Scripts;
using _GAME.Scripts.Inventory;
using Assets._SDK.Game;
using Assets._SDK.GamePlay;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Assets._GAME.Scripts.GamePlay
{
	public class GamePlayLoader : SceneSingleton<GamePlayLoader>
	{
		#region Attributes
		public GameInputPanel GameInputPanel => _gameInputPanel;
        [SerializeField]
		private GameInputPanel _gameInputPanel;

		[SerializeField]
		private GameSceneUI _gameSceneUI;

		public GameSceneUI GameSceneUI => _gameSceneUI;

		MissionInventory _missionInventory;
		public Mission PlayingMission => (Mission)_missionInventory?.PlayingMission;

		public GameObject Runner;
		public GameObject mapHolder;

		[HideInInspector]
		public AbstractNextBotGamePlay CurrentGamePlay => _currentGamePlay;

		private AbstractNextBotGamePlay _currentGamePlay;

		public UniversalRendererData UniversalRendererData;
		#endregion

		// Use this for initialization
		void Start()
		{
			_missionInventory = GameManager.Instance.MissionInventory;
			LoadGamePlay();
			Application.targetFrameRate = 60;
		}

		public void LoadGamePlay()
		{
			_missionInventory.LoadPlayingMission();

			_currentGamePlay = gameObject.GetComponent<AbstractNextBotGamePlay>();

			DestroyGamePlay();

			switch (PlayingMission.ModeGamePlay)
			{
				case ModeGameMission.Survival:
					{
						_currentGamePlay = gameObject.AddComponent<SurvivalGamePlay>();
						break;
					}

				case ModeGameMission.SeekAndKill:
					{
						_currentGamePlay = gameObject.AddComponent<SeekAndKillGamePlay>();
						break;
					}

				case ModeGameMission.Treasure:
					{
						_currentGamePlay = gameObject.AddComponent<TreasureGamePlay>();
						break;
					}
				case ModeGameMission.SeekAndKillCollect:
					{
						_currentGamePlay = gameObject.AddComponent<SeekAndKillCollectGamePlay>();
						break;
					}

				default:
					_currentGamePlay = gameObject.AddComponent<SurvivalGamePlay>();
					break;
			}


			_currentGamePlay?.Init(_gameInputPanel, mapHolder, Runner, UniversalRendererData);
			_gameSceneUI?.Reload(_currentGamePlay);
		}
		private void DestroyGamePlay()
		{
			Destroy(_currentGamePlay);
			_gameSceneUI?.UnSubcribe();
		}
		private void OnApplicationFocus(bool focus)
		{
            if (!focus && CurrentGamePlay.CanFire((GamePlayTrigger.Pause)))
                CurrentGamePlay.Fire(GamePlayTrigger.Pause);
        }
	}

}