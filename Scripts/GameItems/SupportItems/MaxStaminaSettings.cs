using _SDK.Entities;
using Assets._SDK.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME.Scripts.Inventory
{
    [CreateAssetMenu(menuName = "GameItems/SupportItem", fileName = "MaxStamina")]
    public class MaxStaminaSettings : AbstractEntitySettings<SupportItem>
    {
        [HideLabel]
        public SupportItem SupportItem;
        public override IEntity Entity => SupportItem;
    }
}