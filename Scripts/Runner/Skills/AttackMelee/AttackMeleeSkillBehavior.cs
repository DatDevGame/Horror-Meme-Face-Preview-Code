using Assets._SDK.Input;
using Assets._SDK.Skills.Attributes;
using Assets._GAME.Scripts.Skills.Live;
using UniRx;
using UnityEngine;
using System;
using Unity.VisualScripting;
using Assets._GAME.Scripts.GamePlay;
using Assets._GAME.Scripts.Enemies.Skills;
using System.Collections;
using static SoundManager;
using static UnityEngine.ParticleSystem;

namespace Assets._GAME.Scripts.Skills
{
    public class AttackMeleeSkillBehavior : MonoBehaviour, IObserver
	{
        [SerializeField]
        private PercentAttribute _damage;

        [SerializeField]
        private PercentAttribute _timeCoutDown; 

        private CompositeDisposable _disposables;

        public Animator AnimatorControl;

		private float timeCountDown;

        private const string ENEMY_SLEEP_ZONE_TAG = "SleepZone";

		private AudioSource _audioSource;
		private AudioClip _attackSound;
		private SphereCollider colliderAtk;
		private RunnerObservable _runnerObservable;

		void Awake()
        {
            _damage = new PercentAttribute(0, 0);
            _timeCoutDown = new PercentAttribute(0, 0);
			_disposables = new CompositeDisposable();
		}
        private void Start()
        {
			//_attackSound = GameManager.Instance.SoundManager.GetSoundRunner(RunnerSound.Attack);
			_audioSource = GetComponent<AudioSource>();
			colliderAtk = GetComponent<SphereCollider>();
			AnimatorControl?.SetTrigger("StartViewWeapon");

		}
		public void Observe(IObservable runnerObservable)
		{
			_runnerObservable = (RunnerObservable)runnerObservable;
			if (_runnerObservable == null) return;

			_runnerObservable.RunStream.Subscribe((moveVector) =>
			{
				if (moveVector != Vector3.zero)
					StartRunAnimation();
				else
					StopRunAnimation();
			}).AddTo(_disposables);

			_runnerObservable.WalkStream.Subscribe((moveVector) =>
			{
				if (moveVector != Vector3.zero)
					StartWalkAnimation();
				else
					StopWalkAnimation();
			}).AddTo(_disposables);
		}

		public void UnSubscribe()
		{
			StopRunAnimation();
			StopWalkAnimation();
			_disposables?.Clear();
		}

		private void StopRunAnimation()
		{
			if (AnimatorControl?.GetBool("IsRun") == false) return;
				AnimatorControl?.SetBool("IsRun", false);
		}

		private void StartRunAnimation()
		{
			if (AnimatorControl?.GetBool("IsRun") == true) return;

			AnimatorControl?.SetBool("IsRun", true);
			AnimatorControl?.SetTrigger("RunTrigger");
		}

		private void StopWalkAnimation()
		{
			AnimatorControl?.SetBool("IsWalk", false);
		}

		private void StartWalkAnimation()
		{
			StopRunAnimation();
			if (AnimatorControl.GetBool("IsWalk") == true) return;

			AnimatorControl?.SetBool("IsWalk", true);
			AnimatorControl?.SetTrigger("WalkTrigger");
		}

		public void LevelUp(AttackMeleeSkillLevel attackMeleeSkillLevel, Animator animatorControl, AudioClip audioWeapon)
        {
            _damage = _damage.GetModifiedAttribute(attackMeleeSkillLevel.modifierOperator, attackMeleeSkillLevel.DamegePoint);
            _timeCoutDown = _timeCoutDown.GetModifiedAttribute(attackMeleeSkillLevel.modifierOperator, attackMeleeSkillLevel.TimeCountDownPoint);
			_attackSound = audioWeapon;
			AnimatorControl = animatorControl;
		}

		IEnumerator SoundAttack()
		{
			yield return new WaitForSeconds(0.1f);
			_audioSource.PlayOneShot(_attackSound);
		}

		IEnumerator CountDownTimeAttack()
		{
			timeCountDown = _timeCoutDown.Point;
			while (timeCountDown > 0)
			{
				timeCountDown -= Time.deltaTime;
				yield return null;
			}

			colliderAtk.enabled = true;
		}

		private void AttackEffect()
		{
			ParticleSystem[] Effect = GetComponentsInChildren<ParticleSystem>();
			if (Effect.Length <= 0) return;

			foreach (ParticleSystem child in Effect)
				child.Play();
		}

        private void OnTriggerEnter(Collider other)
        {
			if (timeCountDown <= 0)
			{
				AttackEffect();
				colliderAtk.enabled = false;
				AnimatorControl.SetTrigger("AttackTrigger");

				var regionalEnemySkillSystem = other.transform.parent?.GetComponent<RegionalEnemySkillSystem>();
				if(regionalEnemySkillSystem != null)
					StartCoroutine(AttackDelay(regionalEnemySkillSystem));

				var GESkillSystem = other.transform.parent?.GetComponent<GlobalEnemySkillSystem>();
				if (GESkillSystem != null)
					StartCoroutine(AttackDelayGE(GESkillSystem));

				StartCoroutine(CountDownTimeAttack());
				//Destroy(other.gameObject.GetComponent<SphereCollider>());
			}
		}

		IEnumerator AttackDelay(RegionalEnemySkillSystem regionalEnemySkillSystem)
		{
			StartCoroutine(SoundAttack());
			yield return new WaitForSeconds(0.5f);
			GetHitInfo hitInfor = new GetHitInfo();
			hitInfor.Hiter = _runnerObservable;
			hitInfor.Damage = _damage.Point;
			if (regionalEnemySkillSystem != null)
				regionalEnemySkillSystem.RegionalEnemyObservable?.GetHit.Invoke(hitInfor);
		}

		IEnumerator AttackDelayGE(GlobalEnemySkillSystem GESkillSystem)
		{
			StartCoroutine(SoundAttack());
			yield return new WaitForSeconds(0.5f);
			GESkillSystem.GEObservable?.GetHit.Invoke(_damage.Point);
		}

		private void OnDestroy()
		{
			_disposables.Clear();
		}
	}
}