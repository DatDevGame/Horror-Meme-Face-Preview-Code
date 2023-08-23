using _SDK.Entities;
using Assets._SDK.Skills;
using UnityEngine;

namespace Assets._GAME.Scripts.Enemies.Skills
{
    [CreateAssetMenu(menuName = "Skill/Enemies/MoveSkill", fileName = "MoveSkill")]
    public class MoveEnemySkillSettings : AbstractSkillSettings
    {
        public MoveEnemySkill MoveSkill;
        public override AbstractSkill Skill => MoveSkill;

        public override IEntity Entity => MoveSkill;
    }
}