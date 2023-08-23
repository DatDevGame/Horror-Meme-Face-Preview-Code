using _SDK.Entities;
using Assets._SDK.Skills;
using UnityEngine;

namespace Assets._GAME.Scripts.Enemies.Skills
{
    [CreateAssetMenu(menuName = "Skill/Enemies/Skills/Patrol", fileName = "PatrolSkill")]
    public class PatrolSkillSettings : AbstractSkillSettings
    {
        public PatrolSkill MoveSkill;
        public override AbstractSkill Skill => MoveSkill;

        public override IEntity Entity => MoveSkill;
    }
}