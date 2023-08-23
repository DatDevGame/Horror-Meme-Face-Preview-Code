using Assets._SDK.Input;
using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Move
{
    [Serializable]
    public class StaminaSkillSlot : AbstractSkillSlot
    {

        public StaminaSkillBehavior staminaSkillBehavior;
        public StaminaSkillLevel staminaSkillLevel;
        public StaminaSkill staminaSkill;

        public override MonoBehaviour SkillBehavior => staminaSkillBehavior;

        public StaminaSkillSlot(ISkill moveSkill, int levelIndex = DEFAULT_LEVEL_INDEX) : base(moveSkill, levelIndex)
        {
        }

        protected override void LevelUpBehavior()
        {
            staminaSkillLevel = (StaminaSkillLevel)SkillLevel;
            staminaSkillBehavior.LevelUp(staminaSkillLevel);
        }

        protected override void AddBehaviorIfNull(GameObject toGameObject)
        {
            staminaSkillBehavior = toGameObject.GetComponent<StaminaSkillBehavior>();
            if (staminaSkillBehavior != null) return;

            staminaSkillBehavior = toGameObject.AddComponent<StaminaSkillBehavior>();

        }
    }
}