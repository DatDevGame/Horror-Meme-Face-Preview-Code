using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets._SDK.Skills;

namespace Assets._GAME.Scripts.Skills.Move
{
    [Serializable]
    public class LookSkill : AbstractSkill
    {
        public override int Id => (nameof(LookSkill) + Name).GetHashCode();

        [field: SerializeField, ShowInInspector, LabelWidth(50), PropertyOrder(-1)]
        private List<LookSkillLevel> _skillLevels;

        public override List<ISkillLevel> SkillLevels => _skillLevels?.Select(skillLevel => (ISkillLevel)skillLevel).ToList();

    }
}