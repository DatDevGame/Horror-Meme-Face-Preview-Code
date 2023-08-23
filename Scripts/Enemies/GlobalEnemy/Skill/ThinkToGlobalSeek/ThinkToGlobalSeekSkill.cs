using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets._SDK.Skills;

namespace Assets._GAME.Scripts.Enemies.Skills
{
    [Serializable]
    public class ThinkToGlobalSeekSkill : AbstractSkill
    {
        public override int Id => (nameof(ThinkToGlobalSeekSkill) + Name).GetHashCode();

        [field: SerializeField, ShowInInspector, LabelWidth(50), PropertyOrder(-1)]
        private List<ThinkToGlobalSeekSkillLevel> _skillLevels;

        public override List<ISkillLevel> SkillLevels => _skillLevels?.Select(skillLevel => (ISkillLevel)skillLevel).ToList();

    }
}