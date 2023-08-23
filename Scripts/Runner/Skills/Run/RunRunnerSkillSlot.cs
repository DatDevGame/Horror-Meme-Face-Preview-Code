using _GAME.Scripts.Inventory;
using Assets._SDK.Input;
using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Move
{
    [Serializable]
    public class RunRunnerSkillSlot : AbstractSkillSlot
    {
        public RunRunnerSkillBehavior moveSkillBehavior;
        public RunRunnerSkillLevel moveSkillLevel;
        public RunRunnerSkill moveSkill;

        public override MonoBehaviour SkillBehavior => moveSkillBehavior;

        public RunRunnerSkillSlot(ISkill moveSkill ,int levelIndex = DEFAULT_LEVEL_INDEX) : base(moveSkill ,levelIndex)
        {
        }
        protected override void LevelUpBehavior()
        {
            moveSkillLevel = (RunRunnerSkillLevel)SkillLevel;
            moveSkillBehavior.LevelUp(moveSkillLevel);
        }

        protected override void AddBehaviorIfNull(GameObject toGameObject)
        {
            moveSkillBehavior = toGameObject.GetComponent<RunRunnerSkillBehavior>();
			if (moveSkillBehavior != null) return;

            moveSkillBehavior = toGameObject.AddComponent<RunRunnerSkillBehavior>();

        }
    }
}