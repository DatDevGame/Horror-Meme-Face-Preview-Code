using Assets._SDK.Inventory.Interfaces;
using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using Assets._SDK.Skills.Enums;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.TurnLightOnOff
{
    [Serializable]
    public class SetLightSkillLevel : AbstractSkillLevel
    {
        [SerializeField]
        [LabelText("Modifier")]
        [LabelWidth(50)]
        public ModifierOperator modifierOperator = ModifierOperator.Override;

        [SerializeField]
        [LabelText("Size X")]
        [LabelWidth(50)]
        private PercentAttribute _sizeX;

        [SerializeField]
        [LabelText("Size Y")]
        [LabelWidth(50)]
        private PercentAttribute _sizeY;

        [SerializeField]
        [LabelText("Size Z")]
        [LabelWidth(50)]
        private PercentAttribute _sizeZ;

        public PercentAttribute SizeX { get => _sizeX; set => _sizeX = value; }
        public PercentAttribute SizeY { get => _sizeY; set => _sizeY = value; }
        public PercentAttribute SizeZ { get => _sizeZ; set => _sizeZ = value; }

        public SetLightSkillLevel(PercentAttribute sizeX, PercentAttribute sizeY, PercentAttribute sizeZ, int index)
        {
            _sizeX = sizeX;
            _sizeY = sizeY;
            _sizeZ = sizeZ;
            _index = index;
        }
    }
}