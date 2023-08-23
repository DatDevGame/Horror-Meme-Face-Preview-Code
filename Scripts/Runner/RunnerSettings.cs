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
    [CreateAssetMenu(menuName = "GameItems/Runner", fileName = "Runner")]
    public class RunnerSettings : AbstractEntitySettings<Runner>
    {
        [HideLabel]
        public Runner Runner;

        public override IEntity Entity => Runner;
    }
}