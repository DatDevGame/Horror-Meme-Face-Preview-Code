using Assets._SDK.Input;
using Assets._SDK.Skills.Attributes;
using UniRx;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.TurnLightOnOff
{
    public class SetLightSkillBehavior : MonoBehaviour
    {
        private const string LIGHT_TAG = "Light";

        private BoxCollider _boxCollider;

        [SerializeField]
        private PercentAttribute _sizeX;
        [SerializeField]
        private PercentAttribute _sizeY;
        [SerializeField]
        private PercentAttribute _sizeZ;

        private CompositeDisposable _disposables;

        void Awake()
        {
            _sizeX = new PercentAttribute(0, 0);
            _sizeY = new PercentAttribute(0, 0);
            _sizeZ = new PercentAttribute(0, 0);
            _disposables = new CompositeDisposable();
        }

        private void Start()
        {
            _boxCollider = GetComponent<BoxCollider>();
            if (_boxCollider != null)
            {
                _boxCollider.size = new Vector3(_sizeX.Point, _sizeY.Point, _sizeZ.Point);
            }
        }
        private void OnDestroy()
        {
            _disposables?.Clear();
        }
        public void LevelUp(SetLightSkillLevel liveSkillLevel)
        {
            _sizeX = _sizeX.GetModifiedAttribute(liveSkillLevel.modifierOperator, liveSkillLevel.SizeX);
            _sizeY = _sizeY.GetModifiedAttribute(liveSkillLevel.modifierOperator, liveSkillLevel.SizeY);
            _sizeZ = _sizeZ.GetModifiedAttribute(liveSkillLevel.modifierOperator, liveSkillLevel.SizeZ);
        }

        private void OnTriggerEnter(Collider other)
        {
            //if (other.gameObject.CompareTag(LIGHT_TAG))
            //{
            //    other.GetComponent<Light>().enabled = true;
            //}
			other.GetComponent<Light>().enabled = true;
		}
        private void OnTriggerExit(Collider other)
        {
   //         if (other.gameObject.CompareTag(LIGHT_TAG))
   //         {
			//	other.GetComponent<Light>().enabled = false;
			//}
			other.GetComponent<Light>().enabled = false;
		}
    }
}