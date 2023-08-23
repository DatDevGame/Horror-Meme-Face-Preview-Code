using Assets._SDK.Skills.Attributes;
using Assets._SDK.Input;
using System;
using UniRx;
using UnityEngine;
using Unity.VisualScripting;
using UniRx.Triggers;
using static UnityEngine.GraphicsBuffer;
using static SoundManager;
using DG.Tweening;

namespace Assets._GAME.Scripts.Skills.Move
{
    public class RunRunnerSkillBehavior : MonoBehaviour, IObserver
    {
        [SerializeField]
        private PercentAttribute _speed;
		private CharacterController _characterCollider;
		[SerializeField]
		private PercentAttribute _stamina;  

        private CompositeDisposable _disposables;
        private RunnerObservable _runnerObservable;
		private Animator _animatorRunner;

        private AudioSource _audioSource;
        private AudioClip _runSound;
        private const float RUN_SOUND_LENGTH = 0.6f;
        private float _timeDelaySoundWalk = RUN_SOUND_LENGTH;

        private Transform _camera;
        void Awake()
        {
            _speed = new PercentAttribute(0, 0);
			_characterCollider = gameObject.GetComponent<CharacterController>();
			_disposables = new CompositeDisposable();
		}

        void Start()
        {
            _camera = transform.Find("Camera");
            AddSound();
        }
        private void AddSound()
        {
            if (GetComponent<AudioSource>() == null)
                this.gameObject.AddComponent<AudioSource>();

            _audioSource = GetComponent<AudioSource>();
            _runSound = GameManager.Instance.SoundManager.GetSoundRunner(RunnerSound.Run);
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
			StopRunAnimation();
			_disposables?.Clear();
        }
		public void UnSubscribe()
		{
			StopRunAnimation();
			_disposables?.Clear();
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
			_characterCollider.Move(_speed.Point *Time.deltaTime * target);
			MakeSoundWhenRun();
			StartRunning();
		}
        void StartRunning()
        {
			StartRunAnimation();
		}

		void StopRunning()
		{
			StopRunAnimation();
			if (_characterCollider.velocity.magnitude <= 0.1f) return;

			_characterCollider.Move(Vector3.zero);
		}

        private void MakeSoundWhenRun()
        {
            _timeDelaySoundWalk -= Time.deltaTime;
            if (_timeDelaySoundWalk <= 0)
            {
                _audioSource.PlayOneShot(_runSound);
                _timeDelaySoundWalk = RUN_SOUND_LENGTH;
            }
        }
        public void StopRunAnimation()
		{
			if (_animatorRunner?.GetBool("IsRun") == false) return;
			_animatorRunner?.SetBool("IsRun", false);
		}

		private void StartRunAnimation()
		{
            if (_animatorRunner?.GetBool("IsRun") == true) return;

			_animatorRunner?.SetBool("IsRun", true);
			_animatorRunner?.SetTrigger("RunTrigger");
		}

		public void Observe(IObservable input)
        {
            if (input == null) return;
            _runnerObservable = (RunnerObservable)input;
            Subscribe();
        }

        private void Subscribe()
        {
            if (_runnerObservable == null) return;

			_runnerObservable.RunStream.Subscribe((moveVector) =>
			{
                if (moveVector != Vector3.zero)
                    OnMove(moveVector);
                else
                    StopRunning();
			}).AddTo(_disposables);

            
            _runnerObservable.OnExitDoorStream
                .Subscribe((positionTarget) =>
            {
				StopRunAnimation();
				OnMoveTarget(positionTarget);

			}).AddTo(_disposables);
		}
        private void OnDestroy()
        {
            _disposables?.Clear();
        }

        public void LevelUp(RunRunnerSkillLevel moveSkillLevel)
        {
            _speed = _speed.GetModifiedAttribute(moveSkillLevel.modifierOperator, moveSkillLevel.Speed);
        }
    }
}