using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using Assets._SDK.Skills.Enums;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Move
{
    [Serializable]
    public class LookSkillLevel : AbstractSkillLevel
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
		[LabelText("TimeInertia")]
		[LabelWidth(50)]
		private PercentAttribute _timeInertia;

		public PercentAttribute Speed { get => _speed; set => _speed = value; }

		public PercentAttribute TimeInertia { get => _timeInertia; set => _timeInertia = value; }

		public LookSkillLevel(PercentAttribute speed, PercentAttribute timeInertia, int index)
        {
            _speed = speed;
            _timeInertia = timeInertia;
			_index = index;

		}

    }
}