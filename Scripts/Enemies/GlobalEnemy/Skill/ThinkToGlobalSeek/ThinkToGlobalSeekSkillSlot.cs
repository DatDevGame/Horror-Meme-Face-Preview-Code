using Assets._SDK.Input;
using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._GAME.Scripts.Enemies.Skills
{
    [Serializable]
    public class ThinkToGlobalSeekSkillSlot : AbstractSkillSlot
    {

        public ThinkToGlobalSeekSkillBehavior ThinkToGlobalSeekSkillBehavior;
        public ThinkToGlobalSeekSkillLevel ThinkToGlobalSeekSkillLevel;
        public ThinkToGlobalSeekSkill ThinkToGlobalSeekSkill;

        public override MonoBehaviour SkillBehavior => ThinkToGlobalSeekSkillBehavior;

        public ThinkToGlobalSeekSkillSlot(ISkill moveSkill, int levelIndex = DEFAULT_LEVEL_INDEX) : base(moveSkill, levelIndex)
        {
        }

        protected override void LevelUpBehavior()
        {
            ThinkToGlobalSeekSkillLevel = (ThinkToGlobalSeekSkillLevel)SkillLevel;
        }

        protected override void AddBehaviorIfNull(GameObject toGameObject)
        {
            ThinkToGlobalSeekSkillBehavior = toGameObject.GetComponent<ThinkToGlobalSeekSkillBehavior>();
            if (ThinkToGlobalSeekSkillBehavior != null) return;

            ThinkToGlobalSeekSkillBehavior = toGameObject.AddComponent<ThinkToGlobalSeekSkillBehavior>();
        }
    }
}