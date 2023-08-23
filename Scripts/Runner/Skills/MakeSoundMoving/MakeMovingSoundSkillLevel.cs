using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using Assets._SDK.Skills.Enums;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Move
{
    [Serializable]
    public class MakeMovingSoundSkillLevel : AbstractSkillLevel
    {
        [SerializeField]
        [LabelText("Modifier")]
        [LabelWidth(50)]
        public ModifierOperator modifierOperator = ModifierOperator.Override;

        [SerializeField]
        [LabelText("MaxRadiusAreaWalk")]
        [LabelWidth(50)]
        private PercentAttribute _maxRadiusWalk;

		[SerializeField]
		[LabelText("MaxRadiusAreaRun")]
		[LabelWidth(50)]
		private PercentAttribute _maxRadiusRun;

		public PercentAttribute MaxRadiusWalk { get => _maxRadiusWalk; set => _maxRadiusWalk = value; }
		public PercentAttribute MaxRadiusRun { get => _maxRadiusRun; set => _maxRadiusRun = value; }

		public MakeMovingSoundSkillLevel(PercentAttribute maxRadiusWalk, PercentAttribute maxRadiusRun, int index)
        {
			_maxRadiusWalk = maxRadiusWalk;
			_maxRadiusRun = maxRadiusRun;
			_index = index;
        }

    }
}