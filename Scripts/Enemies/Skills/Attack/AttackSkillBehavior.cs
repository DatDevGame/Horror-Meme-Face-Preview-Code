using Assets._SDK.Input;
using Assets._SDK.Skills.Attributes;
using Assets._GAME.Scripts.Skills.Live;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Assets._GAME.Scripts.Skills;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;
using _GAME.Scripts.Inventory;
using static SoundManager;
using UnityEngine.Windows;

namespace Assets._GAME.Scripts.Enemies.Skills
{
    public class AttackSkillBehavior : MonoBehaviour, IObserver
	{
        [SerializeField]
        private PercentAttribute _damage;
        private PercentAttribute _timeCountDown;
        private CompositeDisposable _disposables;
		private Animator _animatorEnemy;
		private RegionalEnemyObservable _regionalEnemyObservable;
		private GlobalEnemyObservable _globalEnemyObservable;

		private AudioSource _audioSource;
        private AudioClip _attackSound;

        private bool canAttack = false;
		IEnumerator countDownAttack;

		void Awake()
        {
            _damage = new PercentAttribute(0, 0);
            _timeCountDown = new PercentAttribute(0, 0);
            _disposables = new CompositeDisposable();
		}

		public void Observe(IObservable regionalEnemyObservable)
		{
			if (regionalEnemyObservable == null) return;

			if (regionalEnemyObservable is RegionalEnemyObservable)
				_regionalEnemyObservable = (RegionalEnemyObservable)regionalEnemyObservable;

			if (regionalEnemyObservable is GlobalEnemyObservable)
				_globalEnemyObservable = (GlobalEnemyObservable)regionalEnemyObservable;


			_globalEnemyObservable?.AttackStream.Subscribe((target) =>
			{
				Attack(target);
			}).AddTo(_disposables);

			_regionalEnemyObservable?.AttackStream.Subscribe((target) =>
			{
				Attack(target);
			}).AddTo(_disposables);

		}

		private void Attack(Collider target)
		{
			if (!canAttack) return;

			canAttack = false;
			transform.LookAt(target.transform);
			if (_animatorEnemy != null)
				_animatorEnemy?.SetTrigger("AttackTrigger");
			RunnerSkillSystem runnerSkillSystem = target.GetComponent<RunnerSkillSystem>();

			if (countDownAttack != null)
				StopCoroutine(countDownAttack);

			countDownAttack = CountDownAttack();
			StartCoroutine(countDownAttack);

			StartCoroutine(AttackDelay(runnerSkillSystem));
		}

		public void UnSubscribe()
		{
			if (countDownAttack != null)
				StopCoroutine(countDownAttack);
			canAttack = false;
			_disposables?.Clear();
		}

		private void Start()
        {
            canAttack = true;
			_animatorEnemy = GetComponentInChildren<Animator>();
            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
                _audioSource = gameObject.AddComponent<AudioSource>();
            _attackSound = GameManager.Instance.SoundManager.GetSoundEnemy(EnemySound.Attack);
        }

        private void OnDestroy()
        {
            _disposables?.Clear();
        }
        IEnumerator CountDownAttack()
        {
			yield return new WaitForSeconds(_timeCountDown.Point);
			canAttack = true;
		}

        public void LevelUp(AttackSkillLevel liveSkillLevel)
        {
            _damage = _damage.GetModifiedAttribute(liveSkillLevel.modifierOperator, liveSkillLevel.DamagePoint);
            _timeCountDown = _timeCountDown.GetModifiedAttribute(liveSkillLevel.modifierOperator, liveSkillLevel.TimeCountDown);

        }
		IEnumerator AttackDelay(RunnerSkillSystem runnerSkillSystem)
		{
			_regionalEnemyObservable?.IsPatrol?.Invoke(false);
			yield return new WaitForSeconds(0.5f);
			if (runnerSkillSystem != null)
			{
                _audioSource.PlayOneShot(_attackSound);
                var killerName = EnemyHelper.ExtractName(this.gameObject);
                runnerSkillSystem.RunnerObservable?.HitAction?.Invoke(new HitAction { Damage = _damage.Point, KillName = killerName });
			}
			_regionalEnemyObservable?.IsPatrol?.Invoke(true);
		}
	}
}