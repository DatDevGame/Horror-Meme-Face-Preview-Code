using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using Assets._SDK.Skills.Enums;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Move
{
    [Serializable]
    public class GravityRunnerSkillLevel : AbstractSkillLevel
    {
        [SerializeField]
        [LabelText("Modifier")]
        [LabelWidth(50)]
        public ModifierOperator modifierOperator = ModifierOperator.Override;

        [SerializeField]
        [LabelText("GravityForce")]
        [LabelWidth(50)]
        private PercentAttribute _gravity_Force;
        public PercentAttribute GravityForce { get => _gravity_Force; set => _gravity_Force = value; }

        public GravityRunnerSkillLevel(PercentAttribute gravityForce, int index)
        {
            _gravity_Force = gravityForce;
            _index = index;
        }

    }
}