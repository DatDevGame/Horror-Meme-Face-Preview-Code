using Assets._SDK.Input;
using Assets._SDK.Skills.Attributes;
using System.Collections;
using UniRx;
using UnityEngine;
using Assets._SDK.GamePlay;
using static SoundManager;
using Unity.VisualScripting;

namespace Assets._GAME.Scripts.Skills.Live
{
    public class LiveRunnerSkillBehavior : MonoBehaviour , IObserver
	{
        const float MAX_HEALTH = 200f;
		const float PERCENT_HP_LOW = 65f;
		private float _healthPoint;
		private bool _isRunnerMaxHP;
		//public float HealthPoint => _healthPoint;

		public float HealthPoint   // property
		{
			get { return _healthPoint; }
			private set {
				if (value >= MAX_HEALTH)
                {
					value = MAX_HEALTH;
					_maxHealthPoint = new PercentAttribute(value, _maxHealthPoint.percent);
				}
				if ((int)_healthPoint != (int)value)
					    _runnerObservable?.HeathPointAction.Invoke(new UpdateHPStaminaSetting { CurrentValue = value, MaxValue = MaxHealthPoint });

				_healthPoint = value;

				CheckHPLowToMakeSound(_healthPoint);
			}
		}

        public float MaxHealthPoint => _maxHealthPoint.Point;


        private PercentAttribute _healthRegeneration;

        [SerializeField]
        private PercentAttribute _maxHealthPoint;

        private CompositeDisposable _disposables;
		private RunnerObservable _runnerObservable;
		private bool isHealthing;
		private string _enemyHitName;

		private AudioSource _audioSource;

		[SerializeField]
        private RunnerSkillSystem _runnerSkillSystem;

		IEnumerator autoHealth;
		void Awake()
        {
            _healthRegeneration = new PercentAttribute(0, 0);
            _maxHealthPoint = new PercentAttribute(0, 0);
            _disposables = new CompositeDisposable();
            isHealthing = false;
        }

        private void Start()
        {
            _runnerSkillSystem = GetComponent<RunnerSkillSystem>();

			_runnerObservable?.HeathPointAction.Invoke(new UpdateHPStaminaSetting { CurrentValue = HealthPoint, MaxValue = MaxHealthPoint });
			AddSound();
		}

		private void AddSound()
		{
			var Camera = GetComponentInChildren<Camera>();
			_audioSource = Camera?.GetComponent<AudioSource>();
			if (_audioSource == null)
				_audioSource = Camera?.AddComponent<AudioSource>();

			if (_audioSource == null) return;

			_audioSource.clip = GameManager.Instance.SoundManager.GetSoundRunner(RunnerSound.LowHP);
			_audioSource.loop = true;
			_audioSource.volume = 0;
			_audioSource.Play();
			CheckHPLowToMakeSound(_healthPoint);
		}

        public void Observe(IObservable runnerObservable)
		{
			_runnerObservable = (RunnerObservable)runnerObservable;
			_runnerObservable.IsDeath = false;
			_runnerObservable.GetHit.Subscribe((hit) =>
			{
				GetTriggerBloodAnimation();

				if (_isRunnerMaxHP) return;

				OnDecreaseHealth(hit.Damage);
                _enemyHitName = hit.KillName;

            }).AddTo(_disposables);

			_runnerObservable.IsRunnerMaxHP.Subscribe((isRunnerMaxHP) =>
			{
				_isRunnerMaxHP = isRunnerMaxHP;

				if (_isRunnerMaxHP)
				{
					HealthPoint = _maxHealthPoint.Point;
				}

			}).AddTo(_disposables);
		}

		public void UnSubscribe()
		{
			_disposables?.Clear();
		}

		private void GetTriggerBloodAnimation()
        {
            var Blood = GameObject.Find("GamePlayingPanel")?.GetComponent<GamePlayingMainPanel>();

            if (Blood != null)
                Blood.TriggerBloodAnimation();
        }


        private IEnumerator AutoHealth()
        {
            while (HealthPoint < _maxHealthPoint.Point)
            {
				if (HealthPoint <= 0)
                {
					isHealthing = false;
					break;
				}

				HealthPoint += _healthRegeneration.Point;
                isHealthing = true;

                if (HealthPoint >= _maxHealthPoint.Point)
					HealthPoint = _maxHealthPoint.Point;

				yield return new WaitForSeconds(1);
            }

            isHealthing = false;
        }
        public void OnDecreaseHealth(float decreaseHealPoint)
        {
			HealthPoint = HealthPoint - decreaseHealPoint;
            if (HealthPoint <= 0)
            {
				HealthPoint = 0;
                Death();
            } else if (!isHealthing)
            {
				if (autoHealth != null)
					StopCoroutine(autoHealth);

				autoHealth = AutoHealth();
				StartCoroutine(autoHealth);
			}
            //_runnerSkillSystem.GamePlayingMainPanel.UpdateHealthPoint((int)HealthPoint);
            //_runnerSkillSystem.GamePlayingMainPanel.HealthPointUI((int)HealthPoint);
        }

		private void CheckHPLowToMakeSound(float hPCurrent)
		{
			if (_audioSource == null) return;

			float hpTired = MaxHealthPoint * PERCENT_HP_LOW / 100f;
			if (hPCurrent <= hpTired)
			{
				_audioSource.volume = 1 - (float)(hPCurrent / hpTired);
			}
		}

		private void Death()
        {
            _runnerObservable.EnemyNameAction.Invoke(_enemyHitName);
			if (GamePlay.GamePlayLoader.Instance.CurrentGamePlay.CanFire(GamePlayTrigger.Lose))
            {
                GamePlay.GamePlayLoader.Instance.CurrentGamePlay.Fire(GamePlayTrigger.Lose);
            }
        }

        public void LevelUp(LiveRunnerSkillLevel liveSkillLevel)
        {
            _maxHealthPoint = _maxHealthPoint.GetModifiedAttribute(liveSkillLevel.modifierOperator, liveSkillLevel.HealthPoint);

			HealthPoint = _maxHealthPoint.Point;

            _healthRegeneration = _healthRegeneration.GetModifiedAttribute(liveSkillLevel.modifierOperator, liveSkillLevel.HealthRegeneration);
        }

		private void OnDestroy()
		{
			UnSubscribe();
			_runnerObservable?.MaxHP?.Invoke(false);
			_audioSource.Stop();
		}
	}
}