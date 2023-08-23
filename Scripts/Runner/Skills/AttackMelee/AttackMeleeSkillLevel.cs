using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using Assets._SDK.Skills.Enums;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills
{
    [Serializable]
    public class AttackMeleeSkillLevel : AbstractSkillLevel
    {
        [SerializeField]
        [LabelText("Modifier")]
        [LabelWidth(50)]
        public ModifierOperator modifierOperator = ModifierOperator.Override;

        [SerializeField]
        [LabelText("Damage")]
        [LabelWidth(50)]
        private PercentAttribute _damage;

        [SerializeField]
        [LabelText("Count Down Attack")]
        [LabelWidth(50)]
        private PercentAttribute _timeCountDown;

        public PercentAttribute DamegePoint { get => _damage; set => _damage = value; }
        public PercentAttribute TimeCountDownPoint { get => _timeCountDown; set => _timeCountDown = value; }

        public AttackMeleeSkillLevel(PercentAttribute damege, PercentAttribute timeCountDown, int index)
        {
            DamegePoint = damege;
            _timeCountDown = timeCountDown;
            _index = index;
        }

    }
}