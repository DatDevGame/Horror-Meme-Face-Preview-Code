using _SDK.Entities;
using Assets._SDK.Skills;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Move
{
    [CreateAssetMenu(menuName = "Skill/MakeSoundMovingSkillSettings", fileName = "MakeSoundMovingSkill")]
    public class MakeMovingSoundSkillSettings : AbstractSkillSettings
    {
        public MakeMovingSoundSkill MakeSoundMovingSkillSkill;
        public override AbstractSkill Skill => MakeSoundMovingSkillSkill;

        public override IEntity Entity => Skill;
    }
}