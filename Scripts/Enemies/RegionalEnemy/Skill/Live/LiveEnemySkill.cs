using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets._SDK.Skills;

namespace Assets._GAME.Scripts.Skills.Live
{
    [Serializable]
    public class LiveEnemySkill : AbstractSkill
    {
        public override int Id => (nameof(LiveEnemySkill) + Name).GetHashCode();

        [field: SerializeField, ShowInInspector, LabelWidth(50), PropertyOrder(-1)]
        private List<LiveEnemySkillLevel> _skillLevels;

		[SerializeField]
		[LabelText("Prefabs Model Hit")]
		[LabelWidth(100)]
		public GameObject Model;

        [SerializeField]
        [LabelText("Effect Hit")]
        [LabelWidth(100)]
        public GameObject HitEffect;

        [SerializeField]
        [LabelText("Effect Monster Hit")]
        [LabelWidth(100)]
        public GameObject HitMonsterEffect;

        public override List<ISkillLevel> SkillLevels => _skillLevels?.Select(skillLevel => (ISkillLevel)skillLevel).ToList();

    }
}