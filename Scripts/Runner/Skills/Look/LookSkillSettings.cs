using _SDK.Entities;
using Assets._SDK.Skills;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Move
{
    [CreateAssetMenu(menuName = "Skill/LookSkillSettings", fileName = "Look")]
    public class LookSkillSettings : AbstractSkillSettings
    {
        public LookSkill RotationSkill;
        public override AbstractSkill Skill => RotationSkill;

        public override IEntity Entity => Skill;
    }
}