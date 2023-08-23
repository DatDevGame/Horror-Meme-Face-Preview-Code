using Assets._SDK.Input;
using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Live
{
    [Serializable]
    public class LiveRunnerSkillSlot : AbstractSkillSlot
    {

        public LiveRunnerSkillBehavior liveSkillBehavior;
        public LiveRunnerSkillLevel liveSkillLevel;
        public LiveRunnerSkill liveSkill;

        public override MonoBehaviour SkillBehavior => liveSkillBehavior;

        public LiveRunnerSkillSlot(ISkill moveSkill, int levelIndex = DEFAULT_LEVEL_INDEX) : base(moveSkill, levelIndex)
        {
        }

        protected override void LevelUpBehavior()
        {
            liveSkillLevel = (LiveRunnerSkillLevel)SkillLevel;
            liveSkillBehavior.LevelUp(liveSkillLevel);
        }

        protected override void AddBehaviorIfNull(GameObject toGameObject)
        {
            liveSkillBehavior = toGameObject.GetComponent<LiveRunnerSkillBehavior>();
            if (liveSkillBehavior != null) return;

            liveSkillBehavior = toGameObject.AddComponent<LiveRunnerSkillBehavior>();

        }
    }
}