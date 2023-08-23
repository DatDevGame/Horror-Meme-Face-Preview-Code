using _SDK.Entities;
using Assets._SDK.Skills;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Live
{
    [CreateAssetMenu(menuName = "Skill/LiveRunnerSkillSettings", fileName = "LiveSkill")]
    public class LiveRunnerSkillSettings : AbstractSkillSettings
    {
        public LiveRunnerSkill LiveSkill;
        public override AbstractSkill Skill => LiveSkill;
        public override IEntity Entity => LiveSkill;
    }
}