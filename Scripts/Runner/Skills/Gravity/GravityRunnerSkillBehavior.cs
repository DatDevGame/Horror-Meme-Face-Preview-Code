using Assets._SDK.Input;
using Assets._SDK.Skills.Attributes;
using System.Collections;
using UniRx;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Move
{
    public class GravityRunnerSkillBehavior : MonoBehaviour, IObserver
	{
        private CharacterController _characterCollider;

        private const float GROUND_OFFSET = 1F;
        private const float GROUND_RADIUS = 0.3F;
        [SerializeField]
        private PercentAttribute _gravityForce;
        public float GravityForce => _gravityForce.Point;
		private RunnerObservable _runnerObservable;
		private CompositeDisposable _disposables;

        private bool _isGrounded;
        public bool IsGrounded => _isGrounded;

        private LayerMask _layerGround;

        void Awake()
        {
            _gravityForce = new PercentAttribute(0, 0);

            _characterCollider = gameObject.GetComponent<CharacterController>();
            _disposables = new CompositeDisposable();
            _layerGround = LayerMask.GetMask("Ground");
        }
        private void OnGravity(Vector3 moveVector)
        {
            GroundedCheck();
            if (!_isGrounded)
            {
                _characterCollider.Move(moveVector * _gravityForce.Point * Time.deltaTime);
            }
        }

		public void Observe(IObservable runnerObservable)
		{
			_runnerObservable = (RunnerObservable)runnerObservable;
			_runnerObservable.IsGround = false;
		}

		public void FixedUpdate()
        {
            OnGravity(Vector3.down);
        }

        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GROUND_OFFSET, transform.position.z);
			_isGrounded = Physics.CheckSphere(spherePosition, GROUND_RADIUS, _layerGround, QueryTriggerInteraction.Ignore);

            _runnerObservable.IsGround = _isGrounded;

		}

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (_isGrounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GROUND_OFFSET, transform.position.z), GROUND_RADIUS);
        }

        private void OnDestroy()
        {
            _disposables?.Clear();
        }

        public void LevelUp(GravityRunnerSkillLevel moveSkillLevel)
        {
            _gravityForce = _gravityForce.GetModifiedAttribute(moveSkillLevel.modifierOperator, moveSkillLevel.GravityForce);
        }
    }
}