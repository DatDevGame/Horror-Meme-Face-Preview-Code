using Assets._SDK.Skills.Attributes;
using Assets._SDK.Input;
using System;
using UniRx;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Windows;
using static SoundManager;
using DG.Tweening;

namespace Assets._GAME.Scripts.Skills.Move
{
    public class WalkRunnerSkillBehavior : MonoBehaviour, IObserver
    {
        [SerializeField]
        private PercentAttribute _speed;
		private CharacterController _characterCollider;
		private CompositeDisposable _disposables;

        private RunnerObservable _runnerObservable;
        private Animator _animatorRunner;

        private AudioSource _audioSource;
        private AudioClip _walkSound;
        private const float TIME_DEFAULT_DELAY = 1.541f;
        private float _timeDelaySoundWalk = TIME_DEFAULT_DELAY;

        private Transform _camera;
        void Awake()
        {
            _speed = new PercentAttribute(0, 0);
			_characterCollider = gameObject.GetComponent<CharacterController>();
			_disposables = new CompositeDisposable();
            _walkSound = GameManager.Instance.Resources.SoundSettings.Sound?.RunnerWalk;
            _audioSource = GetComponent<AudioSource>();
        }
        private void Start()
        {
            _camera = transform.Find("Camera");
            AddSound();
        }

        private void AddSound()
        {
            if (GetComponent<AudioSource>() == null)
                this.gameObject.AddComponent<AudioSource>();

            _audioSource = GetComponent<AudioSource>();
            _walkSound = GameManager.Instance.SoundManager.GetSoundRunner(RunnerSound.Walk);
        }

        public void SetAnimator(Animator Aniamtor)
        {
            _animatorRunner = Aniamtor;
		}

		private void OnEnable()
        {
            Subscribe();
        }

		private void OnDisable()
		{
            _disposables?.Clear();
        }

		public void UnSubscribe()
		{
			StopWalkAnimation();
			_disposables?.Clear();
		}

		private void MakeSoundWhenWalk()
        {
            _timeDelaySoundWalk -= Time.deltaTime;
            if (_timeDelaySoundWalk <= 0)
            {
                _audioSource.PlayOneShot(_walkSound);
                _timeDelaySoundWalk = TIME_DEFAULT_DELAY;
            }
        }
        private void OnMoveTarget(Transform exitDoorTarget)
        {
			Vector3 posittionTargetHandle = new Vector3(exitDoorTarget.position.x, transform.position.y, exitDoorTarget.position.z);
			Vector3 rotationTargetHandle = new Vector3(exitDoorTarget.eulerAngles.x, exitDoorTarget.eulerAngles.y, exitDoorTarget.eulerAngles.z);
			transform.DOMove(posittionTargetHandle, 1f);
            _camera.DORotate(rotationTargetHandle, 1.5f)
								.OnComplete(() => _runnerObservable.SetCompeleteMissionStream(true));
		}
        private void OnMove(Vector3 moveVector)
        {
            Vector3 target = transform.TransformDirection(moveVector);
            target.y = 0;
            _characterCollider.Move(_speed.Point * Time.deltaTime * target);
			StartWalkAnimation();
			MakeSoundWhenWalk();
		}

		void StopRunning()
		{
            if (_characterCollider.velocity.magnitude <= 0.1f) return;

			StopWalkAnimation();
			_characterCollider.Move(Vector3.zero);
		}

		public void StopWalkAnimation()
		{
			_animatorRunner?.SetBool("IsWalk", false);
		}

		private void StartWalkAnimation()
		{
			if (_animatorRunner?.GetBool("IsWalk") == true) return;

			_animatorRunner?.SetBool("IsWalk", true);
			_animatorRunner?.SetTrigger("WalkTrigger");
		}

		public void Observe(IObservable runnerObservable)
        {
            if (runnerObservable == null) return;
            _runnerObservable = (RunnerObservable)runnerObservable;
            Subscribe();
        }

        private void Subscribe()
        {
            if (_runnerObservable == null) return;

			_runnerObservable.WalkStream.Subscribe((moveVector) =>
			{
				if (moveVector != Vector3.zero)
					OnMove(moveVector);
				else
					StopRunning();
			}).AddTo(_disposables);

            _runnerObservable.OnExitDoorStream
                .Subscribe((positionTarget) =>
            {
				StopWalkAnimation();
				OnMoveTarget(positionTarget);
			}).AddTo(_disposables);

        }

        private void OnDestroy()
        {
            _disposables?.Clear();
        }

        public void LevelUp(WalkRunnerSkillLevel skillLevel)
        {
            _speed = _speed.GetModifiedAttribute(skillLevel.modifierOperator, skillLevel.Speed);
        }
    }
}