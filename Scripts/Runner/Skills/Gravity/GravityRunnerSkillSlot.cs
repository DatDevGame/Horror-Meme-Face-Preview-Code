using Assets._SDK.Input;
using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Move
{
    [Serializable]
    public class GravityRunnerSkillSlot : AbstractSkillSlot
    {

        public GravityRunnerSkillBehavior gravitySkillBehavior;
        public GravityRunnerSkillLevel gravitySkillLevel;
        public GravityRunnerSkill gravitySkill;

        public override MonoBehaviour SkillBehavior => gravitySkillBehavior;

        public GravityRunnerSkillSlot(ISkill moveSkill, int levelIndex = DEFAULT_LEVEL_INDEX) : base(moveSkill, levelIndex)
        {
        }

        protected override void LevelUpBehavior()
        {
            gravitySkillLevel = (GravityRunnerSkillLevel)SkillLevel;
            gravitySkillBehavior.LevelUp(gravitySkillLevel);
        }

        protected override void AddBehaviorIfNull(GameObject toGameObject)
        {
            gravitySkillBehavior = toGameObject.GetComponent<GravityRunnerSkillBehavior>();
            if (gravitySkillBehavior != null) return;

            gravitySkillBehavior = toGameObject.AddComponent<GravityRunnerSkillBehavior>();

        }
    }
}