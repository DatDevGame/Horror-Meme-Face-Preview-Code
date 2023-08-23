using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using Assets._SDK.Skills.Enums;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Move
{
    [Serializable]
    public class JumpRunnerSkillLevel : AbstractSkillLevel
    {
        [SerializeField]
        [LabelText("Modifier")]
        [LabelWidth(50)]
        public ModifierOperator modifierOperator = ModifierOperator.Override;

        [SerializeField]
        [LabelText("Speed")]
        [LabelWidth(50)]
        private PercentAttribute _speed;

		[SerializeField]
		[LabelText("CoolDownTime")]
		[LabelWidth(50)]
		private PercentAttribute _coolDownTime;

		public PercentAttribute Speed { get => _speed; set => _speed = value; }
		public PercentAttribute CoolDownTime { get => _coolDownTime; set => _coolDownTime = value; }

		public JumpRunnerSkillLevel(PercentAttribute speed, int index)
        {
            _speed = speed;
            _index = index;
        }

    }
}