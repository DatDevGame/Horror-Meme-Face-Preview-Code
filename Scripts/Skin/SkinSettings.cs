using _SDK.Entities;
using Assets._SDK.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME.Scripts.Inventory
{
    [CreateAssetMenu(menuName = "GameItems/Skin", fileName = "Skin")]
    public class SkinSettings: AbstractEntitySettings<Skin>
    {
        [HideLabel]
        public Skin skin;
        public override IEntity Entity => skin;
    }
}