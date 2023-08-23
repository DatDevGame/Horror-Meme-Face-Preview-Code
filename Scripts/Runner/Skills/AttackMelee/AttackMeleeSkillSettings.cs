using _SDK.Entities;
using Assets._SDK.Skills;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills
{
    [CreateAssetMenu(menuName = "Skill/AttackMelee", fileName = "AttackMelee")]
    public class AttackMeleeSkillSettings : AbstractSkillSettings
    {
        public AttackMeleeSkill AttackMelee;
        public override AbstractSkill Skill => AttackMelee;
        public override IEntity Entity => AttackMelee;
    }
}