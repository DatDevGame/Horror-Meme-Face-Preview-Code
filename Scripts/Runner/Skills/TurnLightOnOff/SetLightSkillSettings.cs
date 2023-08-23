using _SDK.Entities;
using Assets._SDK.Skills;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.TurnLightOnOff
{
    [CreateAssetMenu(menuName = "Skill/SetLightSkillSettings", fileName = "SetLightSkill")]
    public class SetLightSkillSettings : AbstractSkillSettings
    {
        public SetLightSkill SetLightSkill;
        public override AbstractSkill Skill => SetLightSkill;

        public override IEntity Entity => Skill;
    }
}