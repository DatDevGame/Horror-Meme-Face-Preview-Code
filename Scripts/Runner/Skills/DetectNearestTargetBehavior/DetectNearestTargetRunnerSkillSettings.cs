using _SDK.Entities;
using Assets._SDK.Skills;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.DetectNearestTarget
{
    [CreateAssetMenu(menuName = "Skill/DetectNearestTargetRunnerSkillSettings", fileName = "DetectNearestTarget")]
    public class DetectNearestTargetRunnerSkillSettings : AbstractSkillSettings
    {
        public DetectNearestTargetRunnerSkill detectNearestTargetSkill;
        public override AbstractSkill Skill => detectNearestTargetSkill;

        public override IEntity Entity => Skill;
    }
}