using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using Assets._SDK.Skills.Enums;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Move
{
    [Serializable]
    public class WalkRunnerSkillLevel : AbstractSkillLevel
    {
        [SerializeField]
        [LabelText("Modifier")]
        [LabelWidth(50)]
        public ModifierOperator modifierOperator = ModifierOperator.Override;

        [SerializeField]
        [LabelText("Speed")]
        [LabelWidth(50)]
        private PercentAttribute _speed;

        public PercentAttribute Speed { get => _speed; set => _speed = value; }

        public WalkRunnerSkillLevel(PercentAttribute speed, int index)
        {
            _speed = speed;
            _index = index;
        }

    }
}