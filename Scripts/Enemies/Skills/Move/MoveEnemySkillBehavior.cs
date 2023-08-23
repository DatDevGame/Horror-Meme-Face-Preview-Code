using Assets._SDK.Input;
using Assets._SDK.Skills.Attributes;
using Assets._GAME.Scripts.Skills.Live;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;
using System.Collections;
using Unity.VisualScripting;

namespace Assets._GAME.Scripts.Enemies.Skills
{
    public class MoveEnemySkillBehavior : MonoBehaviour, IObserver
    {
        [SerializeField]
        private PercentAttribute _speed;
        private CompositeDisposable _disposables;

        private GlobalEnemyObservable _inputGlobal;
		private RegionalEnemyObservable _inputRegional;
        private NavMeshAgent _navMeshAgent;
        private Animator _animatorEnemy;

        private AudioSource _audioSource;

        GameObject soundModel;

        bool isCheckOutOfRange = false;
        Vector3 _target;
		void Awake()
        {
            _speed = new PercentAttribute(0, 0);
            _disposables = new CompositeDisposable();
        }
        private void Start()
        {
            _navMeshAgent = transform.GetComponent<NavMeshAgent>();

			if (_navMeshAgent == null)
                _navMeshAgent = transform.gameObject.AddComponent<NavMeshAgent>();

            _navMeshAgent.baseOffset = _navMeshAgent.height / 2.0f;
            _navMeshAgent.speed = _speed.Point;

			_animatorEnemy = GetComponentInChildren<Animator>();

			_navMeshAgent.acceleration = 50f;
			_navMeshAgent.angularSpeed = 250f;


			_target = transform.position;

			AddSound();
        }

		private void Update()
		{
			if (isCheckOutOfRange == false) return;

			Vector3 directTarget = _target - transform.position;
			directTarget.y = 0;

			if (directTarget.magnitude <0.5f && _navMeshAgent != null)
			{
				_navMeshAgent?.CompleteOffMeshLink();
				StopRunAnimation();
				isCheckOutOfRange = false;
			}
		}

		public void Observe(IObservable input)
        {
            if (input == null) return;
            if(input is GlobalEnemyObservable)
			    _inputGlobal = (GlobalEnemyObservable)input;
			if (input is RegionalEnemyObservable)
				_inputRegional = (RegionalEnemyObservable)input;
			Subscribe();
        }
        private void Subscribe()
        {
			_inputGlobal?.MoveStream.Subscribe((target) =>
			{
				OnMove(target);
                _target = target;

			}).AddTo(_disposables);

			_inputRegional?.MoveStream.Subscribe((target) =>
			{
				OnMove(target);
				_target = target;
			}).AddTo(_disposables);

        }

        private void OnMove(Vector3 target)
        {
            if (_navMeshAgent == null) return;

            isCheckOutOfRange = true;

			_navMeshAgent?.SetDestination(target);

            Vector3 directTarget = transform.position - target;
            directTarget.y = 0;
			if (directTarget.magnitude < 2.5f)
            {
				_navMeshAgent.speed = 0f;
				StopRunAnimation();
            }
            else {
				_navMeshAgent.speed = _speed.Point;
				StartRunAnimation();
			}
		}

        private void AddSound()
        {
            _audioSource = GetComponentInChildren<AudioSource>();
            if (_audioSource == null) return;

            SetUpSound();
        }

        private void SetUpSound()
        {
            //var skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            //if (skinnedMeshRenderer == null) return;

            //string name = skinnedMeshRenderer.gameObject.name;
            string Object = EnemyHelper.ExtractName(this.gameObject);
            if (string.IsNullOrEmpty(Object)) return;

            if (GetComponent<GlobalEnemySkillSystem>() != null)
            {
                _audioSource.clip = GameManager.Instance.SoundManager.GetSounds("Chasing", Object);
                _audioSource.loop = true;
                _audioSource.Play();
            }
        }
        private void StopRunAnimation()
		{
			_animatorEnemy?.SetBool("IsMove", false);
		}

		private void StartRunAnimation()
		{
			_animatorEnemy?.SetBool("IsMove", true);
			_animatorEnemy?.SetTrigger("MoveTrigger");
		}

		private void OnDestroy()
        {
			_audioSource.Stop();
			_disposables?.Clear();
        }
        public void LevelUp(MoveEnemySkillLevel moveSkillLevel, GameObject SoundModel = null)
        {
            _speed = _speed.GetModifiedAttribute(moveSkillLevel.modifierOperator, moveSkillLevel.SpeedPoint);

            if (SoundModel != null)
                soundModel = SoundModel;

		}

        
    }
}