using Assets._GAME.Scripts.GamePlay;
using Assets._GAME.Scripts.Skills;
using Assets._SDK.Input;
using Assets._SDK.Skills.Attributes;
using System.Collections;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets._GAME.Scripts.Enemies.Skills
{
    public class ThinkToRegionalSeekSkillBehavior : MonoBehaviour, IObserver
	{
		private const float ATTACK_DISTANCE = 5F;

        [SerializeField]
        private PercentAttribute _distanceDetectRunner;
        private CompositeDisposable _disposables;
        private CompositeDisposable _disposablesChasing;
        private RegionalEnemyObservable _regionalEnemyObservable;

		private Vector3 _targetPosition;

		IEnumerator countDownThink;
		private float _distanceTarget;
		bool isPantrol;

		private IEnumerator _chasingTargetCoroutine;
		private bool _isChasingByGameplay = false;
		void Awake()
        {
			_distanceDetectRunner = new PercentAttribute(0, 0);
			_disposables = new CompositeDisposable();
			_disposablesChasing = new CompositeDisposable();
            _targetPosition = GamePlay.GamePlayLoader.Instance.CurrentGamePlay.Runner.transform.position;
		}

        public void Observe(IObservable regionalEnemyObservable)
        {
            if (regionalEnemyObservable == null) return;

            _regionalEnemyObservable = (RegionalEnemyObservable)regionalEnemyObservable;

			RunnerObservable runnerObservable = GamePlay.GamePlayLoader.Instance.CurrentGamePlay.Runner.GetComponent<RunnerSkillSystem>().RunnerObservable;

			runnerObservable.IsInvisible.Subscribe((isInvisible) =>
			{
				_isChasingByGameplay = false;
                _disposablesChasing.Clear();
                if (_chasingTargetCoroutine != null)
                    StopCoroutine(_chasingTargetCoroutine);

                if (countDownThink != null)
					StopCoroutine(countDownThink);
				isPantrol = isInvisible;
                _regionalEnemyObservable?.IsPatrol.Invoke(true);
			}).AddTo(_disposables);

            _regionalEnemyObservable?.OnChasing.Subscribe((target) => 
			{
                _chasingTargetCoroutine = ChasingTarget(target);
				StartCoroutine(_chasingTargetCoroutine);
			}).AddTo(_disposablesChasing);

        }
        private IEnumerator ChasingTarget(Transform target)
        {
            _regionalEnemyObservable?.IsPatrol.Invoke(false);
            if (countDownThink != null)
                StopCoroutine(countDownThink);
            while (true)
            {
				_isChasingByGameplay = true;
                _regionalEnemyObservable?.MoveAction.Invoke(target.position);
                yield return new WaitForSeconds(UnityEngine.Random.Range(0, 7f));
            }
        }
        private void OnDestroy()
        {
            _disposables?.Clear();
			_disposablesChasing?.Clear();

			if (_chasingTargetCoroutine != null)
				StopCoroutine(_chasingTargetCoroutine);
        }
		IEnumerator CountDownThink(Collider other)
		{
			WaitForSeconds delay = new WaitForSeconds(0.5f);

			_regionalEnemyObservable?.IsPatrol.Invoke(false);

			_targetPosition = other.gameObject.transform.position;
			_distanceTarget = Vector3.Distance(_targetPosition, this.transform.position);
			while (_distanceTarget <= _distanceDetectRunner.Point || _distanceTarget <= ATTACK_DISTANCE)
			{
				_distanceTarget = Vector3.Distance(_targetPosition, this.transform.position);
				if (_distanceTarget <= ATTACK_DISTANCE)
					_regionalEnemyObservable.Attack?.Invoke(other);

				_targetPosition = other.gameObject.transform.position;
				_regionalEnemyObservable?.MoveAction.Invoke(_targetPosition);
				yield return delay;
			}

			_regionalEnemyObservable?.IsPatrol.Invoke(true);
			StopCoroutine(countDownThink);
		}
		private void OnTriggerEnter(Collider other)
		{
            if (_isChasingByGameplay) return;

            if (!isPantrol && other.tag == "Runner")
			{
				if (countDownThink != null)
					StopCoroutine(countDownThink);

				countDownThink = CountDownThink(other);
				StartCoroutine(countDownThink);
			}
		}
		private void OnTriggerExit(Collider other)
        {
			if (_isChasingByGameplay) return;

			if (countDownThink != null)
				StopCoroutine(countDownThink);

			_regionalEnemyObservable?.IsPatrol.Invoke(true);
		}

        private void Start()
        {
			Vector3 directTarget = _targetPosition - transform.position;
			directTarget.y = 0;
			_regionalEnemyObservable?.IsPatrol.Invoke(true);

			if (directTarget.magnitude <= _distanceDetectRunner.Point)
			    _regionalEnemyObservable?.MoveAction.Invoke(_targetPosition);
        }

		public void LevelUp(ThinkToRegionalSeekSkillLevel ThinkToRegionalSkillLevel)
		{
			_distanceDetectRunner = _distanceDetectRunner.GetModifiedAttribute(ThinkToRegionalSkillLevel.modifierOperator, ThinkToRegionalSkillLevel.DistanceDetectRunnerPoint);
		}
    }
}