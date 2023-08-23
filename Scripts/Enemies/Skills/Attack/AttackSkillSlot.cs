using Assets._SDK.Input;
using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._GAME.Scripts.Enemies.Skills
{
    [Serializable]
    public class AttackSkillSlot : AbstractSkillSlot
    {

        public AttackSkillBehavior AttackSkillBehavior;
        public AttackSkillLevel AttackSkillLevel;
        public AttackSkill AttackSkill;

        public override MonoBehaviour SkillBehavior => AttackSkillBehavior;

        public AttackSkillSlot(ISkill moveSkill, int levelIndex = DEFAULT_LEVEL_INDEX) : base(moveSkill, levelIndex)
        {
        }

        protected override void LevelUpBehavior()
        {
            AttackSkillLevel = (AttackSkillLevel)SkillLevel;
			AttackSkillBehavior.LevelUp(AttackSkillLevel);
		}

        protected override void AddBehaviorIfNull(GameObject toGameObject)
        {
            AttackSkillBehavior = toGameObject.GetComponent<AttackSkillBehavior>();
            if (AttackSkillBehavior != null) return;

            AttackSkillBehavior = toGameObject.AddComponent<AttackSkillBehavior>();
        }
    }
}