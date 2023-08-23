using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using Assets._SDK.Skills.Enums;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets._GAME.Scripts.Enemies.Skills
{
    [Serializable]
    public class SleepSkillLevel : AbstractSkillLevel
    {
        [SerializeField]
        [LabelText("Modifier")]
        [LabelWidth(50)]
        public ModifierOperator modifierOperator = ModifierOperator.Override;

        [SerializeField]
        [LabelText("Rate Wake Up %")]
        [LabelWidth(50)]
        private PercentAttribute _rateWakeUp;
        public PercentAttribute RateWakeUp { get => _rateWakeUp; set => _rateWakeUp = value; }

        public SleepSkillLevel(PercentAttribute percent, int index)
        {
            _rateWakeUp = percent;
            _index = index;
        }

    }
}