using Assets._SDK.Skills;
using System;
using UnityEngine;

namespace Assets._GAME.Scripts.Enemies.Skills
{
    [Serializable]
    public class ThinkToRegionalSeekSkillSlot : AbstractSkillSlot
    {

        public ThinkToRegionalSeekSkillBehavior ThinkToRegionalSeekSkillBehavior;
        public ThinkToRegionalSeekSkillLevel ThinkToRegionalSeekSkillLevel;
        public ThinkToRegionalSeekSkill ThinkToRegionalSeekSkill;

        public override MonoBehaviour SkillBehavior => ThinkToRegionalSeekSkillBehavior;

        public ThinkToRegionalSeekSkillSlot(ISkill moveSkill, int levelIndex = DEFAULT_LEVEL_INDEX) : base(moveSkill, levelIndex)
        {
        }

        protected override void LevelUpBehavior()
        {
            ThinkToRegionalSeekSkillLevel = (ThinkToRegionalSeekSkillLevel)SkillLevel;
			ThinkToRegionalSeekSkillBehavior.LevelUp(ThinkToRegionalSeekSkillLevel);
		}

        protected override void AddBehaviorIfNull(GameObject toGameObject)
        {
            ThinkToRegionalSeekSkillBehavior = toGameObject.GetComponent<ThinkToRegionalSeekSkillBehavior>();
            if (ThinkToRegionalSeekSkillBehavior != null) return;

            ThinkToRegionalSeekSkillBehavior = toGameObject.AddComponent<ThinkToRegionalSeekSkillBehavior>();
        }
    }
}