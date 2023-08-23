using _GAME.Scripts.Inventory;
using Assets._GAME.Scripts.GamePlay;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
public struct GetHitInfo
{
	public float Damage;
	public IObservable Hiter;
}
public class RegionalEnemyObservable : MonoBehaviour, IObservable
{
	private CompositeDisposable _disposables;
	public Action<Vector3> MoveAction;
	public Action<bool> IsPatrol;
	public Action<bool> IsWakeUp;
	public Action<GetHitInfo> GetHit;
	public Action<Collider> Attack;
	public MonsterForm MonsterForm => Skin.MonsterForm;
	public Skin Skin { get ; set ; }


	private Subject<Transform> _chasingTarget;
    public IObservable<Transform> OnChasing => _chasingTarget;
	public void SetChasing(Transform target) => _chasingTarget.OnNext(target);

	private void Awake()
	{
		_callWhenEnemyDeadModeSeekAndKill = new Subject<GameObject>();
		_callWhenEnemyDeadModeSeekAndKillCollect = new Subject<GameObject>();
		_chasingTarget = new Subject<Transform>();
		_deadEnemy = new Subject<Skin>();

	}

    private bool _hasKey;
	public bool HasKey => _hasKey;
	public void SetKey(bool hasKey)
	{
		_hasKey = hasKey;
	}

	private Subject<Skin> _deadEnemy;
	public IObservable<Skin> OnDeadEnemy => _deadEnemy;
	public void SetDeadEnemy(Skin skin)
	{
		_deadEnemy.OnNext(skin);
		if(GamePlayLoader.Instance.PlayingMission.ModeGamePlay == ModeGameMission.SeekAndKill)
			SeekAndKillGamePlay.Instance?.OnEnemyDeath(skin);
	}

    private Subject<GameObject> _callWhenEnemyDeadModeSeekAndKillCollect;
    public IObservable<GameObject> OnCallWhenEnemyDeadModeSeekAndKillCollect => _callWhenEnemyDeadModeSeekAndKillCollect;
    public void SetWhenEnemyDeadModeSeekAndKillCollect(GameObject monster)
    {
        _callWhenEnemyDeadModeSeekAndKillCollect.OnNext(monster);
        var seekAndSkillCollectObservable = SeekAndKillCollectGamePlay.Instance.SeekAndSkillCollectObservable;
        var indicatorTarget = monster.GetComponent<OffScreenTargetIndicator>();
        if (indicatorTarget != null)
        {
            UnityEngine.Object.Destroy(indicatorTarget);
        }

		seekAndSkillCollectObservable.SetHaveMonsterDead(monster);
		seekAndSkillCollectObservable.SetNameMonsterNeedRemoveSlot(monster.name);
    }


    private Subject<GameObject> _callWhenEnemyDeadModeSeekAndKill;
    public IObservable<GameObject> OnCallWhenEnemyDeadModeSeekAndKill => _callWhenEnemyDeadModeSeekAndKill;
    public void SetWhenEnemyDeadModeSeekAndKill(GameObject monster)
    {
        _callWhenEnemyDeadModeSeekAndKillCollect.OnNext(monster);
        var seekAndSkillObservable = SeekAndKillGamePlay.Instance.SeekAndKillObservable;
        var indicatorTarget = monster.GetComponent<OffScreenTargetIndicator>();
        if (indicatorTarget != null)
        {
            UnityEngine.Object.Destroy(indicatorTarget);
            seekAndSkillObservable.SetHaveMonsterDead(monster.name);
        }
        seekAndSkillObservable.SetNameMonsterNeedRemoveSlot(monster.name);
    }
    public IObservable<Vector3> MoveStream => Observable.FromEvent<Vector3>(
							handler => MoveAction += handler,
							handler => MoveAction -= handler);

	public IObservable<bool> ActionPatrolStream => Observable.FromEvent<bool>(
								handler => IsPatrol += handler,
								handler => IsPatrol -= handler);

	public IObservable<bool> WakeUpStream => Observable.FromEvent<bool>(
								handler => IsWakeUp += handler,
								handler => IsWakeUp -= handler);

	public IObservable<GetHitInfo> GetHitStream => Observable.FromEvent<GetHitInfo>(
								handler => GetHit += handler,
								handler => GetHit -= handler);

	public IObservable<Collider> AttackStream => Observable.FromEvent<Collider>(
							handler => Attack += handler,
							handler => Attack -= handler);

	public IObservable<float> HealthPointStream { get; }

	public void AddDisposable(IDisposable disposable)
	{
		disposable.AddTo(_disposables);
		Debug.Log("Hello");
	}

	public void ClearObservers()
	{
		_disposables.Clear();
	}
}
