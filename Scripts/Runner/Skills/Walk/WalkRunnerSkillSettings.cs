using _SDK.Entities;
using Assets._SDK.Skills;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Move
{
    [CreateAssetMenu(menuName = "Skill/WalkRunnerSkillSettings", fileName = "WalkRunner")]
    public class WalkRunnerSkillSettings : AbstractSkillSettings
    {
        public WalkRunnerSkill moveSkill;
        public override AbstractSkill Skill => moveSkill;

        public override IEntity Entity => Skill;
    }
}