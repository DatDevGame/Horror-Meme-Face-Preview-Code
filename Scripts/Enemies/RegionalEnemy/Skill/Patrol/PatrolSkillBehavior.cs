using Assets._SDK.Input;
using Assets._SDK.Skills.Attributes;
using Assets._GAME.Scripts.Skills.Live;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows;
using System.Collections.Generic;
using Assets._GAME.Scripts.Skills;
using System.Collections;

namespace Assets._GAME.Scripts.Enemies.Skills
{
    public class PatrolSkillBehavior : MonoBehaviour, IObserver
    {
		private PercentAttribute _radius;
		private CompositeDisposable _disposables;
        private RegionalEnemyObservable _regionalEnemyObservable;

		private Vector3 _targetDefault;
		IEnumerator countPatrolAgain;
		void Awake()
        {
			_radius = new PercentAttribute(0, 0);
			_disposables = new CompositeDisposable();
			_targetDefault = transform.position;
		}

		private void Start()
		{
			_regionalEnemyObservable?.IsPatrol.Invoke(true);
		}

		public void Observe(IObservable regionalEnemyObservable)
        {
            if (regionalEnemyObservable == null) return;
            _regionalEnemyObservable = (RegionalEnemyObservable)regionalEnemyObservable;

			_regionalEnemyObservable?.ActionPatrolStream.Subscribe((isPatrol) =>
			{
				if (countPatrolAgain != null)
					StopCoroutine(countPatrolAgain);

				if (isPatrol == false)
				{
					return; 
				} 
				
				OnPatrol(_targetDefault);

			}).AddTo(_disposables);
		}

        public void OnPatrol(Vector3 target)
        {
			if (target != null)
            {
				Vector2 RadiusRundom = Random.insideUnitCircle * _radius.Point;

				Vector3 targetRandom = target + new Vector3(RadiusRundom.x, 0, RadiusRundom.y);

				_regionalEnemyObservable?.MoveAction.Invoke(targetRandom);

				if (countPatrolAgain != null)
					StopCoroutine(countPatrolAgain);

				countPatrolAgain = DelayToPatrolAgain();
				StartCoroutine(countPatrolAgain);

			}
                
        }
		private void OnDestroy()
        {
            _disposables?.Clear();
        }

		public void LevelUp(PatrolSkillLevel PatrolSkillLevel)
		{
			_radius = _radius.GetModifiedAttribute(PatrolSkillLevel.modifierOperator, PatrolSkillLevel.RadiusPoint);
		}

		IEnumerator DelayToPatrolAgain()
		{
			yield return new WaitForSeconds(Random.Range(4, 12));

			_regionalEnemyObservable?.IsPatrol.Invoke(true);
		}
	}
}