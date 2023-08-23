using _SDK.Entities;
using Assets._SDK.Skills;
using UnityEngine;

namespace Assets._GAME.Scripts.Enemies.Skills
{
    [CreateAssetMenu(menuName = "Skill/Enemies/Skills/AttackSkill", fileName = "AttackSkill")]
    public class AttackSkillSettings : AbstractSkillSettings
    {
        public AttackSkill AttackSkill;
        public override AbstractSkill Skill => AttackSkill;

        public override IEntity Entity => AttackSkill;
    }
}