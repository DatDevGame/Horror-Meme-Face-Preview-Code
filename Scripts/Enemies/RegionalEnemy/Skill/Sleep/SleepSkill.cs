using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets._SDK.Skills;

namespace Assets._GAME.Scripts.Enemies.Skills
{
    [Serializable]
    public class SleepSkill : AbstractSkill
    {
        public override int Id => (nameof(SleepSkill) + Name).GetHashCode();

        [field: SerializeField, ShowInInspector, LabelWidth(50), PropertyOrder(-1)]
        private List<SleepSkillLevel> _skillLevels;

        [SerializeField]
        [LabelText("Effect Sleep")]
        [LabelWidth(100)]
        public GameObject EffectSleepModel;

        public override List<ISkillLevel> SkillLevels => _skillLevels?.Select(skillLevel => (ISkillLevel)skillLevel).ToList();

    }
}