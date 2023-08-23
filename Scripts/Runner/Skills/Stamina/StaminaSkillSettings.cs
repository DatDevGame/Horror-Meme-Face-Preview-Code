using _SDK.Entities;
using Assets._SDK.Skills;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Move
{
    [CreateAssetMenu(menuName = "Skill/StaminaSkillSettings", fileName = "StaminaSkill")]
    public class StaminaSkillSettings : AbstractSkillSettings
    {
        public StaminaSkill StaminaSkill;
        public override AbstractSkill Skill => StaminaSkill;

        public override IEntity Entity => Skill;
    }
}