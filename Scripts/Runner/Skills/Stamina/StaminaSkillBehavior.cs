using Assets._GAME.Scripts.GamePlay;
using Assets._SDK.GamePlay;
using Assets._SDK.Input;
using Assets._SDK.Skills.Attributes;
using System.Collections;
using UniRx;
using UnityEngine;
using static SoundManager;

namespace Assets._GAME.Scripts.Skills.Move
{
    public class StaminaSkillBehavior : MonoBehaviour, IObserver
    {
        const float MAX_STAMINA = 200f;
        private float _staminaPoint;
        //public int StaminaPoint => (int)_staminaPoint;

		public float StaminaPoint   // property
		{
			get { return _staminaPoint; }
			private set
			{
                if(value >= MAX_STAMINA)
				{
					value = MAX_STAMINA;
					_maxStamina = new PercentAttribute(value, _maxStamina.percent);
				}
				if (_staminaPoint != value)
					_runnerObservable?.StaminaPointAction.Invoke(new UpdateHPStaminaSetting { CurrentValue = value, MaxValue = MaxStaminaPoint });

				_staminaPoint = value;  
			}
		}

		private PercentAttribute _decreaseStaminaPoint;

        [SerializeField]
        private PercentAttribute _maxStamina;

		public float MaxStaminaPoint => _maxStamina.Point;

		private CompositeDisposable _disposables;

        private RunnerObservable _runnerObservable;
        private bool _isRunning;
		private bool _isRunningMaxStamina;

		private RunnerSkillSystem _runnerSkillSystem;

		private AudioSource _audioSource;
		private AudioClip _tiredSound;
		private const float TIRED_SOUND_LENGTH = 3.216f;
		private float _timeDelaySoundTired = TIRED_SOUND_LENGTH;

		IEnumerator _decreaseStamina;
		IEnumerator _increaseStamina;
		void Awake()
        {
            _decreaseStaminaPoint = new PercentAttribute(0, 0);
            _maxStamina = new PercentAttribute(0, 0);
            _disposables = new CompositeDisposable();
            _isRunning = false;
        }

        private void Start()
        {
            _runnerSkillSystem = GetComponent<RunnerSkillSystem>();
			_runnerObservable?.StaminaPointAction.Invoke(new UpdateHPStaminaSetting { CurrentValue = StaminaPoint, MaxValue = MaxStaminaPoint });

            AddSound();
		}

		private void AddSound()
		{
			if (GetComponent<AudioSource>() == null)
				this.gameObject.AddComponent<AudioSource>();

			_audioSource = GetComponent<AudioSource>();
			_tiredSound = GameManager.Instance.SoundManager.GetSoundRunner(RunnerSound.Tired);
            CheckTiredToMakeSound();
		}

		private IEnumerator OnIncreaseStamina()
        {
            while (!_isRunning && _staminaPoint < _maxStamina.Point)
            {
                StaminaPoint += (_decreaseStaminaPoint.Point * 2) * Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            if (StaminaPoint >= _maxStamina.Point)
                StaminaPoint = _maxStamina.Point;
        }
        public IEnumerator OnDecreaseStamina()
        {
            while (_isRunning && StaminaPoint > 0)
            {
                if (_isRunningMaxStamina) yield break;

                StaminaPoint -= _decreaseStaminaPoint.Point * Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            if (StaminaPoint <= 0)
            {
                StaminaPoint = 0;
                _isRunning = false;
                _runnerObservable.GameInputPanel.BtnRun.onClick.Invoke();

                if (_increaseStamina != null)
                    StopCoroutine(_increaseStamina);

                _increaseStamina = OnIncreaseStamina();
                StartCoroutine(_increaseStamina);
            }

        }
        public void Observe(IObservable RunnerObservable)
        {
            _runnerObservable = (RunnerObservable)RunnerObservable;

			_runnerObservable.RunStream.Subscribe((moveVector) =>
			{
				if (_isRunningMaxStamina) return;
				_isRunning = moveVector != Vector3.zero;
				if (moveVector != Vector3.zero)
                {
                    if (_decreaseStamina != null)
                        StopCoroutine(_decreaseStamina);

                    _decreaseStamina = OnDecreaseStamina();
                    StartCoroutine(_decreaseStamina);
                }
				else
                {
                    if (_increaseStamina != null)
                        StopCoroutine(_increaseStamina);

                    _increaseStamina = OnIncreaseStamina();
                    StartCoroutine(_increaseStamina);

					CheckTiredToMakeSound();
				}
			}).AddTo(_disposables);

			_runnerObservable. IsRunMaxStamina.Subscribe((isRunning) =>
			{
				_isRunningMaxStamina = isRunning;

				if (_isRunningMaxStamina)
                {
					_staminaPoint = _maxStamina.Point;
					//_runnerSkillSystem.GamePlayingMainPanel.UpdateStamina(_staminaPoint);
				}

			}).AddTo(_disposables);

			_runnerObservable?.WalkStream.Subscribe((moveVector) =>
			{
                if (moveVector == Vector3.zero)
                {
					CheckTiredToMakeSound();
					return;
				}
                _isRunning = false;
                if (_decreaseStamina != null)
                {
                    StopCoroutine(_decreaseStamina);
                    _decreaseStamina = null;


                    if (_increaseStamina != null)
                        StopCoroutine(_increaseStamina);

                    _increaseStamina = OnIncreaseStamina();
                    StartCoroutine(_increaseStamina);
                }

			}).AddTo(_disposables);
		}

		private void CheckTiredToMakeSound()
		{
			_timeDelaySoundTired -= Time.deltaTime;

			if (_timeDelaySoundTired <= 0)
			{
				_audioSource.PlayOneShot(_tiredSound);
				_timeDelaySoundTired = _tiredSound.length;
			}
		}

		private void OnDestroy()
        {
            _runnerObservable.MaxStamina.Invoke(false);
			_disposables?.Clear();
        }

        public void LevelUp(StaminaSkillLevel staminaSkillLevel)
        {
            _maxStamina = _maxStamina.GetModifiedAttribute(staminaSkillLevel.modifierOperator, staminaSkillLevel.StaminaPoint);

			StaminaPoint = _maxStamina.Point;

            _decreaseStaminaPoint = _decreaseStaminaPoint.GetModifiedAttribute(staminaSkillLevel.modifierOperator, staminaSkillLevel.DecreaseStamina);
        }
    }
}