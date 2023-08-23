using Assets._SDK.Input;
using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Move
{
    [Serializable]
    public class JumpRunnerSkillSlot : AbstractSkillSlot
    {

        public JumpRunnerSkillBehavior moveSkillBehavior;
        public JumpRunnerSkillLevel moveSkillLevel;
        public JumpRunnerSkill moveSkill;

        public override MonoBehaviour SkillBehavior => moveSkillBehavior;

        public JumpRunnerSkillSlot(ISkill moveSkill ,int levelIndex = DEFAULT_LEVEL_INDEX) : base(moveSkill ,levelIndex)
        {
        }

        protected override void LevelUpBehavior()
        {
            moveSkillLevel = (JumpRunnerSkillLevel)SkillLevel;
            moveSkillBehavior.LevelUp(moveSkillLevel);
        }

        protected override void AddBehaviorIfNull(GameObject toGameObject)
        {
            moveSkillBehavior = toGameObject.GetComponent<JumpRunnerSkillBehavior>();
			if (moveSkillBehavior != null) return;

            moveSkillBehavior = toGameObject.AddComponent<JumpRunnerSkillBehavior>();

        }
    }
}