using Assets._SDK.Input;
using Assets._SDK.Skills.Attributes;
using System.Collections;
using UniRx;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Move
{
    public class JumpRunnerSkillBehavior : MonoBehaviour, IObserver
    {
        [SerializeField]
        private PercentAttribute _speed;

		private CharacterController _characterCollider;

		[SerializeField]
		private PercentAttribute _coolDownTime;

        private CompositeDisposable _disposables;

        private GravityRunnerSkillBehavior _gravityRunnerSkillBehavior;

        private RunnerObservable _runnerObservable;

        void Awake()
        {
            _speed = new PercentAttribute(0, 0);
            _characterCollider = gameObject.GetComponent<CharacterController>();
            _disposables = new CompositeDisposable();
        }
        void Start()
        {
            _gravityRunnerSkillBehavior = gameObject.GetComponent<GravityRunnerSkillBehavior>();
        }
        private void OnEnable()
        {
            Subscribe();
		}

		private void OnDisable()
		{
            _disposables?.Clear();
        }

        private void OnMove(Vector3 moveVector)
        {
            if (_gravityRunnerSkillBehavior == null || _gravityRunnerSkillBehavior.IsGrounded)
                StartCoroutine(Jump(moveVector));
        }

        public void Observe(IObservable runnerObservable)
        {
            if (runnerObservable == null) return;
            _runnerObservable = (RunnerObservable)runnerObservable;
            Subscribe();
        }

        void Subscribe()
        {
            _runnerObservable?.JumpStream.Subscribe((moveVector) =>
			{
                OnMove(moveVector);
            }).AddTo(_disposables);
		}

        IEnumerator Jump(Vector3 moveVector)
        {
            float gravityForce = _gravityRunnerSkillBehavior.GravityForce;
            float speedCurrent = _speed.Point;
            while (speedCurrent >= 0)
            {
                _characterCollider.Move(moveVector * speedCurrent * Time.deltaTime);
                speedCurrent -= gravityForce * Time.deltaTime;

                yield return new WaitForFixedUpdate();
                if(_runnerObservable.IsGround == true)
					yield break;

			}
        }
        private void OnDestroy()
        {
            _disposables?.Clear();
        }

        public void LevelUp(JumpRunnerSkillLevel moveSkillLevel)
        {
            _speed = _speed.GetModifiedAttribute(moveSkillLevel.modifierOperator, moveSkillLevel.Speed);
        }
    }
}