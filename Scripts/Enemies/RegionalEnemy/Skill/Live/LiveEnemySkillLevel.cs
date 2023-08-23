using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using Assets._SDK.Skills.Enums;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Live
{
    [Serializable]
    public class LiveEnemySkillLevel : AbstractSkillLevel
    {
        [SerializeField]
        [LabelText("Modifier")]
        [LabelWidth(50)]
        public ModifierOperator modifierOperator = ModifierOperator.Override;

        [SerializeField]
        [LabelText("HealthPoint")]
        [LabelWidth(50)]
        private PercentAttribute _healthPointDefault;

        public PercentAttribute HealthPoint { get => _healthPointDefault; set => _healthPointDefault = value; }

        public LiveEnemySkillLevel(PercentAttribute healthPoint, int index)
        {
            _healthPointDefault = healthPoint;
            _index = index;
        }
    }
}