using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using Assets._SDK.Skills.Enums;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets._GAME.Scripts.Enemies.Skills
{
    [Serializable]
    public class MoveEnemySkillLevel : AbstractSkillLevel
    {
        [SerializeField]
        [LabelText("Modifier")]
        [LabelWidth(50)]
        public ModifierOperator modifierOperator = ModifierOperator.Override;

        [SerializeField]
        [LabelText("_speed")]
        [LabelWidth(50)]
        private PercentAttribute _speed;
        public PercentAttribute SpeedPoint { get => _speed; set => _speed = value; }

        public MoveEnemySkillLevel(PercentAttribute speed, int index)
        {
            SpeedPoint = speed;
            _index = index;
        }

    }
}