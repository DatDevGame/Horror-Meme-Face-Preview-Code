using Assets._GAME.Scripts.Skills.Live;
using Assets._SDK.Input;
using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._GAME.Scripts.Enemies.Skills
{
    [Serializable]
    public class PatrolSkillSlot : AbstractSkillSlot
    {

        public PatrolSkillBehavior PatrolSkillBehavior;
        public PatrolSkillLevel PatrolSkillLevel;
        public PatrolSkill PatrolSkill;

        public override MonoBehaviour SkillBehavior => PatrolSkillBehavior;

        public PatrolSkillSlot(ISkill moveSkill, int levelIndex = DEFAULT_LEVEL_INDEX) : base(moveSkill, levelIndex)
        {
        }

        protected override void LevelUpBehavior()
        {
            PatrolSkillLevel = (PatrolSkillLevel)SkillLevel;
			PatrolSkillBehavior.LevelUp(PatrolSkillLevel);
		}

        protected override void AddBehaviorIfNull(GameObject toGameObject)
        {
            PatrolSkillBehavior = toGameObject.GetComponent<PatrolSkillBehavior>();
            if (PatrolSkillBehavior != null) return;

            PatrolSkillBehavior = toGameObject.AddComponent<PatrolSkillBehavior>();
        }
    }
}