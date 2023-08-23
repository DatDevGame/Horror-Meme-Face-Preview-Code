using Assets._SDK.Input;
using Assets._SDK.Skills.Attributes;
using System;
using UniRx;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Move
{
    public class MakeMovingSoundSkillBehavior : MonoBehaviour, IObserver
    {

        [SerializeField]
        private PercentAttribute _maxRadiusWalk;

		[SerializeField]
		private PercentAttribute _maxRadiusRun;

		private SphereCollider _areaMakeSound;

        private CompositeDisposable _disposables;

        private const int RADIUS_SIZE_DEFAULT = 3;

        bool isDisable = false;

        void Awake()
        {
            _maxRadiusWalk = new PercentAttribute(RADIUS_SIZE_DEFAULT, 0);
			_maxRadiusRun = new PercentAttribute(RADIUS_SIZE_DEFAULT, 0);
			_disposables = new CompositeDisposable();
            _areaMakeSound = GetComponent<SphereCollider>();
        }
        private void OnEnable()
        {
            isDisable = false;
        }

        private void OnDisable()
        {
            isDisable = true;
        }

        public void Observe(IObservable input)
        {
            if (input == null) return;

			((RunnerObservable)input).RunStream.Subscribe((moveVector) =>
			{
				if (isDisable) return;

				if (moveVector != Vector3.zero)
					StartMakingSound(_maxRadiusRun);
				else
					StopMakingSound();
			}).AddTo(_disposables);

			((RunnerObservable)input).WalkStream.Subscribe((moveVector) =>
			{
				if (moveVector != Vector3.zero)
					StartMakingSound(_maxRadiusWalk);
				else
					StopMakingSound();
			}).AddTo(_disposables);
		}

        private void StartMakingSound(PercentAttribute maxRadius)
        {
            _areaMakeSound.radius = maxRadius.Point;
        }

		private void StopMakingSound()
        {
            _areaMakeSound.radius = 0;
        }

        private void OnDestroy()
        {
            _disposables?.Clear();
        }

        public void LevelUp(MakeMovingSoundSkillLevel skillLevel)
        {
            _maxRadiusWalk = _maxRadiusWalk.GetModifiedAttribute(skillLevel.modifierOperator, skillLevel.MaxRadiusWalk);
			_maxRadiusRun = _maxRadiusRun.GetModifiedAttribute(skillLevel.modifierOperator, skillLevel.MaxRadiusRun);

		}
    }
}