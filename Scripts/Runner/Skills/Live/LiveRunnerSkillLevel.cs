using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using Assets._SDK.Skills.Enums;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Live
{
    [Serializable]
    public class LiveRunnerSkillLevel : AbstractSkillLevel
    {
        [SerializeField]
        [LabelText("Modifier")]
        [LabelWidth(50)]
        public ModifierOperator modifierOperator = ModifierOperator.Override;

        [SerializeField]
        [LabelText("HealthPoint")]
        [LabelWidth(50)]
        private PercentAttribute _healthPointDefault;

        [SerializeField]
        [LabelText("HealthRegeneration  HP/s")]
        [LabelWidth(50)]
        private PercentAttribute _healthRegeneration;

        public PercentAttribute HealthPoint { get => _healthPointDefault; set => _healthPointDefault = value; }
        public PercentAttribute HealthRegeneration { get => _healthRegeneration; set => _healthRegeneration = value; }

        public LiveRunnerSkillLevel(PercentAttribute healthPoint, PercentAttribute healthRegeneration, int index)
        {
            _healthPointDefault = healthPoint;
            _healthRegeneration = healthRegeneration;
            _index = index;
        }

    }
}