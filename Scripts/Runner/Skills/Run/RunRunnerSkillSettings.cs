using _SDK.Entities;
using Assets._SDK.Skills;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Move
{
    [CreateAssetMenu(menuName = "Skill/RunRunnerSkillSettings", fileName = "RunRunner")]
    public class RunRunnerSkillSettings : AbstractSkillSettings
    {
        public RunRunnerSkill moveSkill;
        public override AbstractSkill Skill => moveSkill;

        public override IEntity Entity => Skill;
    }
}