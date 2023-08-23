using _SDK.Entities;
using _SDK.Inventory;
using Assets._GAME.Scripts.Enemies;
using Assets._GAME.Scripts.Game;
using Assets._SDK.Entities;
using Assets._SDK.Inventory;
using Assets._SDK.Inventory.Interfaces;
using Assets._SDK.Missions;
using Assets._SDK.Skills;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace _GAME.Scripts.Inventory
{
    [CreateAssetMenu(menuName = "Sound/SoundSettings", fileName = "SoundSettings")]
    public class SoundSettings : AbstractEntitySettings<Sound>
    {
        [HideLabel]
        public Sound Sound;

        public override IEntity Entity => Sound;
    }
}