using _SDK.Inventory;
using Assets._GAME.Scripts.Skills;
using UnityEngine;

namespace _GAME.Scripts.Inventory
{
    public class MaxStaminaSlot : AbstractGameItemSlot
    {
		public MaxstaminaBehavior supportItemBehavior;
		public GameObject Model;

		public MaxStaminaSlot(SupportItem item) : base(item)
        {

		}
        protected override void AddBehaviorIfNull(GameObject toGameObject)
        {
			supportItemBehavior = Model?.GetComponent<MaxstaminaBehavior>();
			if (supportItemBehavior != null) return;

			Model = Object.Instantiate(Item.Model, toGameObject.transform.position, Quaternion.identity, toGameObject.transform);

            Model.AddComponent<MaxstaminaBehavior>();
		}

        protected override void Activate(GameObject toGameObject)
        {
			//_renderer.material.color = ((SupportItem)Item).CubeColor;
		}
    }
}