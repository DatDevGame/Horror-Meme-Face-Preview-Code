using _SDK.Entities;
using Assets._SDK.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME.Scripts.Inventory
{
    [CreateAssetMenu(menuName = "GameItems/Weapon", fileName = "Weapon")]
    public class WeaponSettings : AbstractEntitySettings<Weapon>
    {
        [HideLabel]
        public Weapon weapon;
        public override IEntity Entity => weapon;
    }
}