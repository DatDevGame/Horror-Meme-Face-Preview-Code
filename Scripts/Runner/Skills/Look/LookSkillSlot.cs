using Assets._SDK.Input;
using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Move
{
    [Serializable]
    public class LookSkillSlot : AbstractSkillSlot
    {

        public LookSkillBehavior LookSkillBehavior;
        public LookSkillLevel LookSkillLevel;
        public LookSkill LookSkill;

        public override MonoBehaviour SkillBehavior => LookSkillBehavior;

        public LookSkillSlot(ISkill moveSkill ,int levelIndex = 1) : base(moveSkill ,levelIndex)
        {
        }

        protected override void LevelUpBehavior()
        {
            LookSkillLevel = (LookSkillLevel)SkillLevel;
            LookSkillBehavior.LevelUp(LookSkillLevel);
        }

        protected override void AddBehaviorIfNull(GameObject toGameObject)
        {
			LookSkillBehavior = toGameObject.GetComponent<LookSkillBehavior>();

			if (LookSkillBehavior != null) return;

            LookSkillBehavior = toGameObject.AddComponent<LookSkillBehavior>();

        }
    }
}