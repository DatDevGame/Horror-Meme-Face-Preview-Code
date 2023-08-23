using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets._SDK.Skills;

namespace Assets._GAME.Scripts.Enemies.Skills
{
    [Serializable]
    public class MoveEnemySkill : AbstractSkill
    {
        public override int Id => (nameof(MoveEnemySkill) + Name).GetHashCode();

        [field: SerializeField, ShowInInspector, LabelWidth(50), PropertyOrder(-1)]
        private List<MoveEnemySkillLevel> _skillLevels;

        [SerializeField]
        [LabelText("Prefabs Sound Chasing")]
        [LabelWidth(100)]
        public GameObject AudioSourceModel;
        public override List<ISkillLevel> SkillLevels => _skillLevels?.Select(skillLevel => (ISkillLevel)skillLevel).ToList();

    }
}