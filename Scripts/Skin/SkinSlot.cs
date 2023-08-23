using _SDK.Inventory;
using UnityEngine;

namespace _GAME.Scripts.Inventory
{
    public class SkinSlot : AbstractGameItemSlot
    {
        private MeshRenderer _renderer;
        public GameObject Model;

		public SkinSlot(Skin item) : base(item)
        {

        }
        protected override void AddBehaviorIfNull(GameObject toGameObject)
        {
			Model = Object.Instantiate(Item.Model, toGameObject.transform.position, Quaternion.identity, toGameObject.transform);
			if (!Model.TryGetComponent(out _renderer))
			{
				_renderer = Model.AddComponent<MeshRenderer>();
			}
		}

        protected override void Activate(GameObject toGameObject)
        {
            _renderer.material.color = ((Skin)Item).CubeColor;
        }
    }
}