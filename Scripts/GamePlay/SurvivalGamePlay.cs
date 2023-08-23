using Assets._GAME.Scripts.Enemies;
using Assets._SDK.GamePlay;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Assets._GAME.Scripts.GamePlay
{
    public class SurvivalGamePlay : AbstractNextBotGamePlay
	{
		public static SurvivalGamePlay Instance => (SurvivalGamePlay)GamePlayLoader.Instance.CurrentGamePlay;

        public GameObject ExitDoor { get; private set; }

        private CompositeDisposable _disposables;
		private float _timeStartGame;
		private double _timeSecondsCurrent;
		private float _timeTargetToWin;
		private bool _isExit = false;
		protected override void OnStart()
		{
			_disposables = new CompositeDisposable();

            _timeStartGame = Time.time;
			_timeSecondsCurrent = Time.time - _timeStartGame;

			RunnerSkillSystem.RunnerObservable.OnCompeleteMissionStream.Subscribe((positionTagert) =>
			{
				StartCoroutine(DelayToWin());
			}).AddTo(_disposables);

			RunnerSkillSystem.RunnerObservable.OnExitDoorStream
				.Subscribe(_ => _isExit = true)
				.AddTo(_disposables);
        }

        private IEnumerator DelayToWin()
		{
			yield return new WaitForEndOfFrame();
			RunnerSkillSystem.GetComponentInChildren<Animator>().SetTrigger("StartViewWin");
			yield return new WaitForSeconds(1f);
			if (CanFire(GamePlayTrigger.Win))
				Fire(GamePlayTrigger.Win);
			Debug.Log("Win");

			RunnerSkillSystem.UnSubscribeSkillWhenWin();
			RunnerSkillSystem.RunnerObservable.ClearObservers();

		}

		private void FixedUpdate()
		{
			if (CurrentState.Value != GamePlayState.Running || _isExit)
				return;

			_timeSecondsCurrent = Time.time - _timeStartGame;
			if (_timeSecondsCurrent >= _timeTargetToWin && CanFire(GamePlayTrigger.Win))
			{
				gamePlayingMainPanel?.UpdateTargetSurvival((float)0.00);
				Fire(GamePlayTrigger.Lose);
				Debug.Log("Lose");
				return;
			}
			gamePlayingMainPanel?.UpdateTargetSurvival((float)Math.Round(_timeTargetToWin - _timeSecondsCurrent, 2));
		}

		protected override void InitMap()
		{
			base.InitMap();
			InitExitDoor();
		}

		private void InitExitDoor()
		{
			var door = _playingMap.ExitDoors[playingMission.ExitDoorPosition - 1].transform;
            ExitDoor = Instantiate(_playingMap.ExitDoorGameObject, door.position, door.rotation, MapHolder.transform);
            ExitDoor.transform.localScale = door.localScale;
			var doorBehavior = ExitDoor.AddComponent<DoorExitBehavior>();
			doorBehavior.InitAttribute(ExitDoor.transform);

			doorBehavior.SetDetectOpenDoor(true);
		}

		protected override void InitEnemies()
		{
			base.InitEnemies();
			for (int i = 0; i < EnemyGroups.Count; i++)
			{
				LoadEnemyGroup(GlobalEnemySpawnPoints, RegionalEnemySpawnPoints, EnemyGroups[i]);
			}
        }

        private void LoadEnemyGroup(List<GameObject> globalEnemyPositions, List<GameObject> regionalEnemyPositions, EnemyGroup enemyGroup)
		{
			if (enemyGroup.Type == EnemyType.Regional)
			{
				for (int i = 0; i < enemyGroup.Amount; i++)
				{
					RegionalEnemySpawnPoints.Add(_playingMap.regionalEnemyPositions[playingMission.RESpawnPoints[i] - 1]);
					if (IsEnemySpawnIgnored(i))
						continue;

					RegionalEnemySlot RESlot = new RegionalEnemySlot(MapHolder);
					RESlot.AddBehaviorIfNull(regionalEnemyPositions[amountRE++], enemyGroup.level);
					RESlot.AddSkinIfNull(enemyGroup.EnemySettings.skin);

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
		protected override void OnInitGamePlay()
		{
			_timeTargetToWin = playingMission.TargetWin;

			StateMachine.Activate();

			StateMachine.Fire(GamePlayTrigger.Play);
		}

		protected override void Reload()
		{
			_timeTargetToWin = 0;
			_timeSecondsCurrent = 0;
			_timeStartGame = Time.time;
			base.Reload();
		}

		protected override void End()
		{
			base.End();
			_disposables.Clear();
            Destroy(ExitDoor);
		}
        private void OnDestroy()
        {
			_disposables.Clear();
        }
    }
}