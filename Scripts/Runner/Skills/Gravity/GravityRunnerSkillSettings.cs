using _SDK.Entities;
using Assets._SDK.Skills;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Move
{
    [CreateAssetMenu(menuName = "Skill/GravityRunnerSkillSettings", fileName = "GravityRunner")]
    public class GravityRunnerSkillSettings : AbstractSkillSettings
    {
        public GravityRunnerSkill GravitySkill;

        public override AbstractSkill Skill => GravitySkill;

        public override IEntity Entity => GravitySkill;
    }
}