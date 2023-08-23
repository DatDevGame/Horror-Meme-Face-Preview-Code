using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets._SDK.Skills;
using Assets._SDK.Inventory.Interfaces;

namespace Assets._GAME.Scripts.Skills.Move
{
    [Serializable]
    public class MakeMovingSoundSkill : AbstractSkill
    {
        public override int Id => (nameof(MakeMovingSoundSkill) + Name).GetHashCode();

        [field: SerializeField, ShowInInspector, LabelWidth(50), PropertyOrder(-1)]
        private List<MakeMovingSoundSkillLevel> _skillLevels;

        [SerializeField]
        [LabelText("Prefabs Skill")]
        [LabelWidth(100)]
        public GameObject Model;

		public override List<ISkillLevel> SkillLevels => _skillLevels?.Select(skillLevel => (ISkillLevel)skillLevel).ToList();
    }
}