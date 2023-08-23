using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using Assets._SDK.Skills.Enums;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets._GAME.Scripts.Enemies.Skills
{
    [Serializable]
    public class PatrolSkillLevel : AbstractSkillLevel
    {
        [SerializeField]
        [LabelText("Modifier")]
        [LabelWidth(50)]
        public ModifierOperator modifierOperator = ModifierOperator.Override;

        [SerializeField]
        [LabelText("AreaPatrol")]
        [LabelWidth(50)]
        private PercentAttribute _radius;
        public PercentAttribute RadiusPoint { get => _radius; set => _radius = value; }

        public PatrolSkillLevel(PercentAttribute radius, int index)
        {
			_radius = radius;
            _index = index;
        }

    }
}