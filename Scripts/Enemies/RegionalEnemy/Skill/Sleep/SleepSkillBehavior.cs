using Assets._GAME.Scripts.GamePlay;
using Assets._SDK.Input;
using Assets._SDK.Skills.Attributes;
using UniRx;
using UnityEngine;

namespace Assets._GAME.Scripts.Enemies.Skills
{
    public class SleepSkillBehavior : MonoBehaviour, IObserver
	{
        [SerializeField]
        private PercentAttribute _rateWakeUp;
        private CompositeDisposable _disposables;
        private RegionalEnemyObservable _regionalEnemyObservable;

        public GameObject SleepEffect;

        private bool _isSleeping = true;
        public bool IsSleeping
        {
            get => _isSleeping;
            set => _isSleeping = value;
        }

        private int _rateWakeUpMax;

        private Animator _animatorEnemy;


		void Awake()
        {
            _rateWakeUp = new PercentAttribute(0, 0);
			_disposables = new CompositeDisposable();
		}

        private void Start()
		{
			_animatorEnemy = GetComponentInChildren<Animator>();
			CheckStartWakeup(-1);
        }

        public void Observe(IObservable regionalEnemyObservable)
		{
			if (regionalEnemyObservable == null) return;
			_regionalEnemyObservable = (RegionalEnemyObservable)regionalEnemyObservable;

            //Subscribe();
        }
        public void WakeupEffect()
        {
            if (SleepEffect == null) return;
            Destroy(SleepEffect);

            _animatorEnemy?.SetBool("isSleep", false);
        }

        private void Subscribe()
        {
            _regionalEnemyObservable?.WakeUpStream.Subscribe((isWakeUp) =>
            {
                float rateWakeUp = _rateWakeUp.Point;

                if (_rateWakeUpMax > 0)
                    rateWakeUp = IncreaseWakePointToNextLevel();

                if(GameManager.Instance.DriverToolTest.ApplyBalanceToggle.isOn == false)
					rateWakeUp = _rateWakeUp.Point;

				Debug.Log(rateWakeUp);
                CheckStartWakeup();

			}).AddTo(_disposables);
        }

        void CheckStartWakeup(float rateWakeUp = 0)
        {

			int RateRandom = Random.Range(0, 100);
			if (RateRandom <= (int)rateWakeUp)
			{
				WakeupEffect();
				GetComponent<RegionalEnemySkillSystem>().LoadSkillWhenWakeUp();
				_isSleeping = false;
                Destroy(this);
                return;
			}

			_animatorEnemy?.SetBool("isSleep", true);
		}

        private int IncreaseWakePointToNextLevel()
        {
            var seekAndKillGamePlay = SeekAndKillGamePlay.Instance;
            var rateWakeUp = _rateWakeUp.Point + (_rateWakeUpMax - _rateWakeUp.Point) * (seekAndKillGamePlay.QualityKill / seekAndKillGamePlay.TargetToWin);
            return (int)rateWakeUp;
        }
        private void OnDestroy()
        {
            _disposables?.Clear();
        }
		public void LevelUp(SleepSkillLevel SleepSkillLevel)
		{
            _rateWakeUp = _rateWakeUp.GetModifiedAttribute(SleepSkillLevel.modifierOperator, SleepSkillLevel.RateWakeUp);
        }
        public void LevelUpRateWakeUpMax(SleepSkillLevel SleepSkillLevel, int RateWakeUpMax)
        {
            _rateWakeUp = _rateWakeUp.GetModifiedAttribute(SleepSkillLevel.modifierOperator, SleepSkillLevel.RateWakeUp);
            _rateWakeUpMax = RateWakeUpMax;
        }
    }
}