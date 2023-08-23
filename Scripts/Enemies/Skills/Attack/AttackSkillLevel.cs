using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using Assets._SDK.Skills.Enums;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets._GAME.Scripts.Enemies.Skills
{
    [Serializable]
    public class AttackSkillLevel : AbstractSkillLevel
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
        [LabelText("Count Down")]
        [LabelWidth(50)]
        private PercentAttribute _time;
        public PercentAttribute DamagePoint { get => _damage; set => _damage = value; }
        public PercentAttribute TimeCountDown { get => _time; set => _time = value; }

        public AttackSkillLevel(PercentAttribute damage, PercentAttribute countDown, int index)
        {
            DamagePoint = damage;
            TimeCountDown = countDown;
            _index = index;
        }

    }
}