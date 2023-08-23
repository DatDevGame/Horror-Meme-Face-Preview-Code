using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using Assets._SDK.Skills.Enums;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets._GAME.Scripts.Enemies.Skills
{
    [Serializable]
    public class ThinkToRegionalSeekSkillLevel : AbstractSkillLevel
    {
        [SerializeField]
        [LabelText("Modifier")]
        [LabelWidth(50)]
        public ModifierOperator modifierOperator = ModifierOperator.Override;

        [SerializeField]
        [LabelText("DistanceDetectRunner")]
        [LabelWidth(50)]
        private PercentAttribute _distanceDetectRunner;
        public PercentAttribute DistanceDetectRunnerPoint { get => _distanceDetectRunner; set => _distanceDetectRunner = value; }

        public ThinkToRegionalSeekSkillLevel(PercentAttribute speed, int index)
        {
			_distanceDetectRunner = speed;
            _index = index;
        }

    }
}