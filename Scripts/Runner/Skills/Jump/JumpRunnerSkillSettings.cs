using _SDK.Entities;
using Assets._SDK.Skills;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Move
{
    [CreateAssetMenu(menuName = "Skill/JumpRunnerSkillSettings", fileName = "JumpRunner")]
    public class JumpRunnerSkillSettings : AbstractSkillSettings
    {
        public JumpRunnerSkill moveSkill;
        public override AbstractSkill Skill => moveSkill;

        public override IEntity Entity => Skill;
    }
}