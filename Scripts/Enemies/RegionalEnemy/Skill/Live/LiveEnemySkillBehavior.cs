using Assets._SDK.Input;
using Assets._SDK.Skills.Attributes;
using System.Collections;
using UniRx;
using UnityEngine;
using Assets._GAME.Scripts.GamePlay;
using Assets._GAME.Scripts.Enemies.Skills;
using UnityEngine.AI;
using static SoundManager;
using _GAME.Scripts.Inventory;
using Dissolve;
using Unity.VisualScripting;
using System.Collections.Generic;

namespace Assets._GAME.Scripts.Skills.Live
{
    public class LiveEnemySkillBehavior : MonoBehaviour, IObserver
    {
        [SerializeField] private GameObject BloodEffect;
        [SerializeField] private GameObject BloodMonsterEffect;
        private GameObject modelCollider;
		private float _healthPoint;

        [SerializeField]
        private PercentAttribute _maxHealthPoint;
        private CompositeDisposable _disposables;
        private RegionalEnemyObservable _regionalEnemyObservable;
		private GlobalEnemyObservable _globalEnemyObservable;

		private AudioSource _audioSource;
        private AudioClip _receiveDameSound;

        private DissolveHandler _dissolveMonster;

        Rigidbody[] rigidbodys;
        Collider[] colliders;
		Animator[] anims;
		RegionalEnemySkillSystem systemSkillRE;
        GlobalEnemySkillSystem systemSkillGE;

		void Awake()
        {
            _maxHealthPoint = new PercentAttribute(0, 0);
            _disposables = new CompositeDisposable();
        }
        private void Start()
        {
            AddSound();
			rigidbodys = GetComponentsInChildren<Rigidbody>();
			colliders = GetComponentsInChildren<Collider>();
            systemSkillRE = GetComponent<RegionalEnemySkillSystem>();
			systemSkillGE = GetComponent<GlobalEnemySkillSystem>();
            _dissolveMonster = GetComponentInChildren<DissolveHandler>();
            anims = GetComponentsInChildren<Animator>();
		}
        private void AddSound()
        {
            if (GetComponent<AudioSource>() == null)
                this.gameObject.AddComponent<AudioSource>();

            _audioSource = GetComponent<AudioSource>();

            _receiveDameSound = GameManager.Instance.SoundManager.GetSoundEnemy(EnemySound.ReceiveDame);
        }

        public void Observe(IObservable EnemyObservable)
        {
			if (EnemyObservable == null) return;
			if (EnemyObservable is GlobalEnemyObservable)
				_globalEnemyObservable = (GlobalEnemyObservable)EnemyObservable;
			if (EnemyObservable is RegionalEnemyObservable)
				_regionalEnemyObservable = (RegionalEnemyObservable)EnemyObservable;

            _regionalEnemyObservable?.GetHitStream.Subscribe((hit) =>
            {
				OnREDecreaseHealth(hit);
            }).AddTo(_disposables);

            _globalEnemyObservable?.GetHitStream.Subscribe((hit) =>
			{
				OnDecreaseHealth(hit);
			}).AddTo(_disposables);


		}

		public void UnSubscribe()
		{
			_disposables?.Clear();
		}

		public void OnDecreaseHealth(float decreaseHealPoint)
        {
            HitEffect();

            if(_receiveDameSound != null)
                _audioSource?.PlayOneShot(_receiveDameSound);

            _healthPoint -= decreaseHealPoint;
            if (_healthPoint <= 0)
            {
                Death();
            }
        }

		public void OnREDecreaseHealth(GetHitInfo hitInfor)
		{
			HitEffect();

			if (_receiveDameSound != null)
				_audioSource?.PlayOneShot(_receiveDameSound);

			_healthPoint -= hitInfor.Damage;
			if (_healthPoint <= 0)
            {
				Death();
				if(_regionalEnemyObservable.HasKey)
                {
					var RunnerObservable = (RunnerObservable)hitInfor.Hiter;
                    RunnerObservable.SetTotalKeysFound(1);
				}
                CallWhenDead();
            }
        }
        private void CallWhenDead()
        {
            CallWhenModeSeekAndKill();
            CallWhenModeSeekAndKillCollect();
        }
		private void HitEffect()
        {
            var formRE = _regionalEnemyObservable?.MonsterForm;
            var formGE = _globalEnemyObservable?.MonsterForm;

            if (BloodEffect != null && formRE == MonsterForm.NextBot || formGE == MonsterForm.NextBot)
                InstantiateBloodEffect(BloodEffect);
            else if (BloodMonsterEffect != null && formRE == MonsterForm.Monster || formGE == MonsterForm.Monster)
                InstantiateBloodEffect(BloodMonsterEffect);
        }
        private void InstantiateBloodEffect(GameObject effect)
        {
            Instantiate(effect, this.transform.position, Quaternion.identity, transform.parent);
        }

        private void CallWhenModeSeekAndKill()
        {
            if (GamePlayLoader.Instance.CurrentGamePlay is SeekAndKillGamePlay)
                _regionalEnemyObservable.SetWhenEnemyDeadModeSeekAndKill(gameObject);
        }
        private void CallWhenModeSeekAndKillCollect()
        {
            if (GamePlayLoader.Instance.CurrentGamePlay is SeekAndKillCollectGamePlay && _regionalEnemyObservable.HasKey)
                _regionalEnemyObservable.SetWhenEnemyDeadModeSeekAndKillCollect(gameObject);
        }
        private void Death()
        {
            if (_regionalEnemyObservable != null)
                _regionalEnemyObservable.SetDeadEnemy(_regionalEnemyObservable.Skin);
            else if (_globalEnemyObservable != null)
                _globalEnemyObservable.SetDeadEnemy(_globalEnemyObservable.Skin);

            if (modelCollider != null)
                Destroy(modelCollider);

			DeadEffect();
        }

        private void DeadEffect()
        {
            Dissolve();

			var SleepEffect = GetComponent<SleepSkillBehavior>();
            if (SleepEffect != null)
                SleepEffect.WakeupEffect();


			Destroy(GetComponent<NavMeshAgent>());
			//RE
			//Eenemy Monster
			if (_regionalEnemyObservable != null)
            {
				if (_regionalEnemyObservable?.MonsterForm == MonsterForm.Monster)
					RagdollMonsterEffect();
				//Eenemy Nextbot
				else
					RagdollNextBotEffect();
			} else if(_globalEnemyObservable != null)
            {
				//GE
				//Eenemy Monster
				if (_globalEnemyObservable?.MonsterForm == MonsterForm.Monster)
					RagdollMonsterEffect();
				//Eenemy Nextbot
				else
					RagdollNextBotEffect();
			}
			foreach (Rigidbody rb in rigidbodys)
				rb.isKinematic = false;

			
			StartCoroutine(StopRagdollEffect(rigidbodys, colliders));
		}

        private void RagdollNextBotEffect()
        {
            float timeDestroyAnim = 0.1f;
            if(anims.Length > 0)
			    anims[0].SetTrigger("DeadTrigger");
            //Destroy(anim, timeDestroyAnim);

            foreach (Animator animator in anims)
				Destroy(animator, timeDestroyAnim);

		}

        private void RagdollMonsterEffect()
        {
			foreach (Animator animator in anims)
				Destroy(animator);

            _dissolveMonster.EnableColliderRagdol();

            StartCoroutine(ForceRagdollHandle());
		}
        IEnumerator ForceRagdollHandle()
        {
            Vector3 directionForce = RandomDirectionForce();
            float Force = 35f;
            float time = 0.5f;

            while (time > 0)
            {
                time -= Time.deltaTime;
                transform.Translate(directionForce * time * Force * Time.deltaTime, Space.World);
                yield return null;
            }
        }
        private Vector3 RandomDirectionForce()
        {
            int randNumber = UnityEngine.Random.Range(1, 4);
            if (randNumber == 1)
                return GamePlayLoader.Instance.Runner.transform.forward;
            //else if (randNumber == 2)
            //    return -GamePlayLoader.Instance.Runner.transform.forward;
            else
                return Vector3.up * 0.5f + GamePlayLoader.Instance.Runner.transform.forward;

        }
        IEnumerator StopRagdollEffect(Rigidbody[] rigidbodies, Collider[] colliders)
        {
            systemSkillRE?.UnSubscribeSkillWhenDeath();
			systemSkillGE?.UnSubscribeSkillWhenDeath();
			yield return new WaitForSeconds(3);

			systemSkillRE?.RemoveSkill();
			systemSkillGE?.RemoveSkill();
			foreach (Rigidbody rb in rigidbodies)
                if(rb != null)
                    rb.isKinematic = true;

            foreach (Collider cd in colliders)
                if(cd != null)
                    Destroy(cd);

            Destroy(gameObject, 3f);
		}
        private void Dissolve()
        {
            if (_dissolveMonster != null)
                _dissolveMonster.StartDissolve();
        }

        public void LevelUp(LiveEnemySkillLevel liveSkillLevel, GameObject modelColliderLive)
        {
            _maxHealthPoint = _maxHealthPoint.GetModifiedAttribute(liveSkillLevel.modifierOperator, liveSkillLevel.HealthPoint);
            _healthPoint = _maxHealthPoint.Point;

            if(modelColliderLive != null)
                modelCollider = modelColliderLive;
		}
        public void SetBloodEffect(GameObject effecNextBottHit, GameObject effectMonsterHit)
        {
            if (effecNextBottHit != null)
                BloodEffect = effecNextBottHit;

            if (effectMonsterHit != null)
                BloodMonsterEffect = effectMonsterHit;
        }
    }
}