using _SDK.Inventory;
using Assets._SDK.Inventory.Interfaces;
using UnityEngine;

namespace _GAME.Scripts.Inventory
{
    public class WeaponSlot : AbstractGameItemSlot
    {
        public GameObject Model;

        public WeaponSlot(Weapon item) : base(item)
        {

        }
        protected override void AddBehaviorIfNull(GameObject toGameObject)
        {
            
        }

        protected override void Activate(GameObject toGameObject)
        {

        }
    }
}