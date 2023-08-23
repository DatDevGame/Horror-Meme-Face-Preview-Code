using _GAME.Scripts.Shop;
using Assets._GAME.GameItems.DoorExit;
using Assets._GAME.Scripts.GamePlay;
using Assets._SDK.Game;
using Assets._SDK.GamePlay;
using Assets._SDK.Skills;
using System;
using UniRx;
using UnityEngine;

namespace Assets._SDK.Input
{
	public struct HitAction {
		public float Damage;
		public string KillName;
	}
	public struct ItemUpgradeSkillSetting
	{
		public AbstractSkillSettings SkillSeting;
		public int Index;
	}
	public struct UpdateHPStaminaSetting
	{
		public float CurrentValue;
		public float MaxValue;
	}

	public class RunnerObservable : IObservable, IFoundKeyObservable
	{
		public GameInputPanel GameInputPanel => _gameInputPanel;
		private static GameInputPanel _gameInputPanel;

		bool _isRunActive = false;
		public Action<HitAction> HitAction;
		public Action<string> EnemyNameAction;
		public Action<UpdateHPStaminaSetting> HeathPointAction;
		public Action<UpdateHPStaminaSetting> StaminaPointAction;
		public Action<bool> MaxStamina;
		public Action<bool> MaxHP;
		public Action<bool> IsPause;
		public Action<ItemUpgradeSkillSetting> AttachSkill;
		public Action<bool> InvisibleRunner;
		public Action<bool> ScanAllEnemies;

		public bool IsDeath = false;
		public bool IsGround = false;
		private CompositeDisposable _disposables;
		private bool _isGoExitDoor = false;
		private bool _canNotControl => IsDeath || _isGoExitDoor;

		public ReactiveProperty<int> TotalKeysFound { get; private set; }
		public IObservable<int> OnFoundKeyStream => TotalKeysFound;
		public void SetTotalKeysFound(int totalKeysBonus)
		{
			TotalKeysFound.Value += totalKeysBonus;
		}
		public int GetTotalKey() => TotalKeysFound.Value;


		public Subject<Transform> ExitDoor { get; private set; }
		public IObservable<Transform> OnExitDoorStream => ExitDoor;
		public void TryExitDoor(Transform positionTarget)
		{
			ExitDoor.OnNext(positionTarget);
			_isGoExitDoor = true;
		}

		private Subject<bool> _compeleteMission;
		public IObservable<bool> OnCompeleteMissionStream => _compeleteMission;
		public void SetCompeleteMissionStream(bool isDone)
		{
			_compeleteMission.OnNext(isDone);

		}

		private Subject<string> _regionalMonsters;
		public IObservable<string> OnRegionalStream => _regionalMonsters;
		public void SetRegionalSlot(string detectNearestSkill)
		{
			_regionalMonsters.OnNext(detectNearestSkill);
		}

		private Subject<string> _changeNearTarget;
		public IObservable<string> OnChangeNearTarget => _changeNearTarget;
		public void SetChangeNearTarget(string detectNearestSkill)
		{
			_changeNearTarget.OnNext(detectNearestSkill);
		}

		private Subject<string> _treasureNearest;
		public IObservable<string> OnFindTreasurelStream => _treasureNearest;
		public void SetTreasureSlot(string nameTreasure)
		{
			_treasureNearest.OnNext(nameTreasure);
		}


		private Subject<bool> _exitDoorOpened;
		public IObservable<bool> OnExitDoorOpened => _exitDoorOpened;
		public void SetExitDoorOpened(bool isOpened)
		{
			_exitDoorOpened.OnNext(isOpened);
		}
		public RunnerObservable(GameInputPanel gamePlayingPanel)
		{
			_regionalMonsters = new Subject<string>();
			_changeNearTarget = new Subject<string>();
			_treasureNearest = new Subject<string>();
			_gameInputPanel = gamePlayingPanel;
			_disposables = new CompositeDisposable();
			_compeleteMission = new Subject<bool>();
			ExitDoor = new Subject<Transform>();
			TotalKeysFound = new ReactiveProperty<int>();
			_exitDoorOpened = new Subject<bool>();

			GamePlayLoader.Instance.CurrentGamePlay.CurrentState.Where((value) => value == GamePlayState.PauseUpgradingSkill)
							.Subscribe(_ => ResetInput()).AddTo(_disposables);
			GamePlayLoader.Instance.CurrentGamePlay.CurrentState.Where((value) => value == GamePlayState.PauseCollectingSkin)
										.Subscribe(_ => ResetInput()).AddTo(_disposables);
		}

		static IObservable<bool> CheckIsMovement = Observable.EveryUpdate()
					.Where(_ => ((Vector3.forward * _gameInputPanel.Joystick.Vertical + Vector3.right * _gameInputPanel.Joystick.Horizontal) != Vector3.zero))
					.Select(_ => true)
					.TakeWhile(_ => GamePlayLoader.Instance.CurrentGamePlay.CurrentState.Value == GamePlay.GamePlayState.Running);

		IObservable<bool> isMove = Latch(Observable.EveryLateUpdate(), CheckIsMovement, false);

		public IObservable<Vector3> RunStream => isMove
							.Where(_ => _isRunActive == true && !_canNotControl)
							.Select(_ => Vector3.forward * GameInputPanel.Joystick.Vertical + Vector3.right * GameInputPanel.Joystick.Horizontal);

		public IObservable<Vector3> WalkStream => isMove
					.Where(_ => _isRunActive == false && !_canNotControl)
					.Select(_ => Vector3.forward * GameInputPanel.Joystick.Vertical + Vector3.right * GameInputPanel.Joystick.Horizontal);

		public IObservable<Touch[]> RotationStream => Observable.EveryLateUpdate()
					.Where(_ => UnityEngine.Input.touchCount > 0 
					&& GamePlayLoader.Instance.CurrentGamePlay.CurrentState.Value == GamePlay.GamePlayState.Running
					&& !_canNotControl)
					.Select(_ => UnityEngine.Input.touches)
					.TakeWhile(_ => AbstractGameManager.Instance.CurrentState.Value == GameState.Playing);

		public IObservable<Vector3> LookBehindStream => GameInputPanel.BtnLookBehind.OnClickAsObservable()
			.Where(_ => GamePlayLoader.Instance.CurrentGamePlay.CurrentState.Value == GamePlayState.Running)
			.Select(_ => Vector3.zero)
			.TakeWhile(_ => AbstractGameManager.Instance.CurrentState.Value == GameState.Playing);

		public IObservable<Vector3> JumpStream => GameInputPanel.BtnJump.OnClickAsObservable()
					.Select(_ => Vector3.up)
					.TakeWhile(_ => AbstractGameManager.Instance.CurrentState.Value == GameState.Playing);

		public IObservable<bool> IsRunActiveStream => GameInputPanel.BtnRun.OnClickAsObservable()
					.Select(_ => _isRunActive = !_isRunActive)
					.TakeWhile(_ => AbstractGameManager.Instance.CurrentState.Value == GameState.Playing);

		public IObservable<HitAction> GetHit => Observable.FromEvent<HitAction>(
							handler => HitAction += handler,
							handler => HitAction -= handler)
			.Where(_ => !_canNotControl);

		public IObservable<bool> StartPause => Observable.FromEvent<bool>(
									handler => IsPause += handler,
									handler => IsPause -= handler);

		public IObservable<string> JumpScare => Observable.FromEvent<string>(
									handler => EnemyNameAction += handler,
									handler => EnemyNameAction -= handler);

		public IObservable<UpdateHPStaminaSetting> HealthStream => Observable.FromEvent<UpdateHPStaminaSetting>(
									handler => HeathPointAction += handler,
									handler => HeathPointAction -= handler);

		public IObservable<UpdateHPStaminaSetting> StaminaStream => Observable.FromEvent<UpdateHPStaminaSetting>(
											handler => StaminaPointAction += handler,
											handler => StaminaPointAction -= handler);

		public IObservable<bool> IsRunMaxStamina => Observable.FromEvent<bool>(
									handler => MaxStamina += handler,
									handler => MaxStamina -= handler);

		public IObservable<bool> IsRunnerMaxHP => Observable.FromEvent<bool>(
											handler => MaxHP += handler,
											handler => MaxHP -= handler);

		public IObservable<ItemUpgradeSkillSetting> AttackSkillFromItem => Observable.FromEvent<ItemUpgradeSkillSetting>(
									handler => AttachSkill += handler,
									handler => AttachSkill -= handler);

		public IObservable<bool> IsInvisible => Observable.FromEvent<bool>(
											handler => InvisibleRunner += handler,
											handler => InvisibleRunner -= handler);

		public IObservable<bool> IsScanAllEnemies => Observable.FromEvent<bool>(
													handler => ScanAllEnemies += handler,
													handler => ScanAllEnemies -= handler);

		public void AddDisposable(IDisposable disposable)
		{
			disposable.AddTo(_disposables);
		}

		public void ClearObservers()
		{
			_isRunActive = false;
			_disposables.Clear();
		}

		public void ResetInput()
		{
			UnityEngine.Input.ResetInputAxes();
			IsPause.Invoke(true);
		}

		static IObservable<bool> Latch(IObservable<long> tick, IObservable<bool> latchTrue, bool initialValue)
		{
			// Create a custom Observable, whose behavior is determined by our calls to the provided 'observable'
			return Observable.Create<bool>(observer => {
				// Our state value.
				var value = initialValue;

				// Create an inner subscription to latch:
				// Whenever latch fires, store true.
				var latchSub = latchTrue.Subscribe(_ => value = true);

				// Create an inner subscription to tick:
				var tickSub = tick.Subscribe(
					// Whenever tick fires, send the current value and reset state.
					_ => {
						observer.OnNext(value);
						value = false;
					},
					observer.OnError, // pass through tick's errors (if any)
					observer.OnCompleted); // complete when tick completes

				// If we're disposed, dispose inner subscriptions too.
				return Disposable.Create(() => {
					latchSub.Dispose();
					tickSub.Dispose();
				});
			});
		}
	}

/*
	public static class CustomObservables
	{
		public static IObservable<T> SelectRandom<T>(this IObservable<Unit> eventObs, T[] items)
		{
			// Edge-cases:
			var n = items.Length;
			if (n == 0)
			{
				// No items!
				return Observable.Empty<T>();
			}
			else if (n == 1)
			{
				// Only one item!
				return eventObs.Select(_ => items[0]);
			}

			var myItems = (T[])items.Clone();
			return Observable.Create<T>(observer => {
				var sub = eventObs.Subscribe(_ => {
					// Select any item after the first.
					var i = UnityEngine.Random.Range(1, n);
					var value = myItems[i];
					// Swap with value at index 0 to avoid selecting an item twice in a row.
					var temp = myItems[0];
					myItems[0] = value;
					myItems[i] = temp;
					// Finally emit the selected value.
					observer.OnNext(value);
				},
				observer.OnError,
				observer.OnCompleted);

				return Disposable.Create(() => sub.Dispose());
			});
		}
	}
*/
}