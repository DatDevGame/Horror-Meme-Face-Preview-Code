using Assets._SDK.Input;
using Assets._SDK.Skills.Attributes;
using Assets._GAME.Scripts.Skills.Live;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows;
using Assets._GAME.Scripts.GamePlay;
using static UnityEngine.GraphicsBuffer;
using System.Collections;
using System;
using Assets._GAME.Scripts.Skills;

namespace Assets._GAME.Scripts.Enemies.Skills
{
	public class ThinkToGlobalSeekSkillBehavior : MonoBehaviour, IObserver
	{
		const float RADIUS_PATROL = 20f;
		private const float ATTACK_DISTANCE = 5F;
		private GlobalEnemyObservable _globalEnemyObservable;
		IEnumerator countDownThink;
		IEnumerator AttackCoroutine;
		bool canThink;
		float distanceTarget;
		private CompositeDisposable _disposables;
		bool isRunnerVisible;

		Vector3 positionTarget   // property
		{
			set {}
			get
			{
				if (isRunnerVisible)
				{
					Vector2 RadiusRundom = UnityEngine.Random.insideUnitCircle * RADIUS_PATROL;
					return this.transform.position + new Vector3(RadiusRundom.x, 0, RadiusRundom.y);
				}
					return GamePlay.GamePlayLoader.Instance.CurrentGamePlay.Runner.transform.position;
			}
		}

		public void Observe(IObservable globalEnemyObservable)
		{
			if (globalEnemyObservable == null) return;
			_globalEnemyObservable = (GlobalEnemyObservable)globalEnemyObservable;
			positionTarget = GamePlay.GamePlayLoader.Instance.CurrentGamePlay.Runner.transform.position;
			canThink = true;
		}

		private void Start()
		{
			RunnerObservable runnerObservable = GamePlay.GamePlayLoader.Instance.CurrentGamePlay.Runner.GetComponent<RunnerSkillSystem>().RunnerObservable;
			_disposables = new CompositeDisposable();
			runnerObservable.IsInvisible.Subscribe((isInvisible) =>
			{
				if (AttackCoroutine != null)
					StopCoroutine(AttackCoroutine);
				canThink = true;
				isRunnerVisible = isInvisible;

			}).AddTo(_disposables);
		}

		private void FixedUpdate()
		{
			if (canThink)
			{
				if (countDownThink != null)
					StopCoroutine(countDownThink);

				countDownThink = CountDownThink();
				StartCoroutine(countDownThink);
			}

		}
        private void OnTriggerEnter(Collider target)
        {
			if (!isRunnerVisible && target.tag == "Runner")
			{
				if (AttackCoroutine != null)
					StopCoroutine(AttackCoroutine);

				AttackCoroutine = AttackThink(target);
 				StartCoroutine(AttackCoroutine);
			}
		}
        IEnumerator AttackThink(Collider target)
		{
			distanceTarget = Vector3.Distance(target.transform.position, this.transform.position);
			while (distanceTarget < ATTACK_DISTANCE)
			{
				distanceTarget = Vector3.Distance(target.transform.position, this.transform.position);
				_globalEnemyObservable.Attack?.Invoke(target);
				yield return null;
			}
		}
		IEnumerator CountDownThink()
		{
			_globalEnemyObservable?.MoveAction.Invoke(positionTarget);
			canThink = false;
			yield return new WaitForSeconds(UnityEngine.Random.Range(0, 7f));
			canThink = true;
		}

		private void OnDestroy()
		{
			_disposables.Clear();
		}

	}
}