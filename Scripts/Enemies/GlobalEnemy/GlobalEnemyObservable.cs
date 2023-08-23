using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Assets._SDK.Game;
using Assets._GAME.Scripts.GamePlay;
using Assets._SDK.GamePlay;
using _GAME.Scripts.Inventory;

public class GlobalEnemyObservable : IObservable
{
	private CompositeDisposable _disposables;
	public Action<float> GetHit;
	public Action<Vector3> MoveAction;
	public Action<Collider> Attack;

	public MonsterForm MonsterForm => Skin.MonsterForm;
	public Skin Skin { get; set; }

	private Subject<Skin> _deadEnemy;
	public IObservable<Skin> OnDeadEnemy => _deadEnemy;
	public void SetDeadEnemy(Skin skin)
	{
		_deadEnemy.OnNext(skin);
		if (GamePlayLoader.Instance.PlayingMission.ModeGamePlay == ModeGameMission.SeekAndKill)
			SeekAndKillGamePlay.Instance?.OnEnemyDeath(skin); ;
	}

	public GlobalEnemyObservable( GameInputPanel gamePlayingPanel = null)
	{
		_disposables = new CompositeDisposable();
		_deadEnemy = new Subject<Skin>();
	}

	//public IObservable<Vector3> MoveStream => Observable.EveryUpdate()
	//				.Where(_ => TranformTarget != null && GamePlayLoader.Instance.currentGamePlay.CurrentState.Value == GamePlayState.Running)
	//				.Select(_ => TranformTarget.position)
	//				.TakeWhile(_ => AbstractGameManager.Instance.CurrentState.Value == GameState.Playing);

	public IObservable<Vector3> MoveStream => Observable.FromEvent<Vector3>(
								handler => MoveAction += handler,
								handler => MoveAction -= handler);

	public IObservable<float> GetHitStream => Observable.FromEvent<float>(
									handler => GetHit += handler,
									handler => GetHit -= handler);

	public IObservable<Collider> AttackStream => Observable.FromEvent<Collider>(
						handler => Attack += handler,
						handler => Attack -= handler);

	public IObservable<float> HealthPointStream { get; }

	public void AddDisposable(IDisposable disposable)
	{
		disposable.AddTo(_disposables);
	}

	public void ClearObservers()
	{
		_disposables.Clear();
	}
}
