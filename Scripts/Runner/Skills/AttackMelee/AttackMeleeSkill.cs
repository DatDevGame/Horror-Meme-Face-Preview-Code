using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets._SDK.Skills;

namespace Assets._GAME.Scripts.Skills
{
    [Serializable]
    public class AttackMeleeSkill : AbstractSkill
    {
        public override int Id => (nameof(AttackMeleeSkill) + Name).GetHashCode();

        [field: SerializeField, ShowInInspector, LabelWidth(50), PropertyOrder(-1)]
        private List<AttackMeleeSkillLevel> _skillLevels;

		[SerializeField]
		[LabelText("Prefabs Skill")]
		[LabelWidth(100)]
		public GameObject Model;

		[SerializeField]
		[LabelText("Audio Weapon")]
		[LabelWidth(100)]
		public AudioClip AudioWeapon;

		public override List<ISkillLevel> SkillLevels => _skillLevels?.Select(skillLevel => (ISkillLevel)skillLevel).ToList();

    }
}