using _SDK.Entities;
using _SDK.Inventory;
using Assets._SDK.Entities;
using Assets._SDK.Inventory;
using Assets._SDK.Inventory.Interfaces;
using Assets._SDK.Missions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME.Scripts.Inventory
{
    [CreateAssetMenu(menuName = "GameItems/Map", fileName = "Map")]
    public class MapSettings : AbstractEntitySettings<Map>
    {
        [HideLabel]
        public Map map;

        public override IEntity Entity => map;
    }
}