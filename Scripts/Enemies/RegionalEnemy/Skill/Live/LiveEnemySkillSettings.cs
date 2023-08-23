using _SDK.Entities;
using Assets._SDK.Skills;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Live
{
    [CreateAssetMenu(menuName = "Skill/Enemies/Skills/LiveEnemySkillSettings", fileName = "LiveSkill")]
    public class LiveEnemySkillSettings : AbstractSkillSettings
    {
        public LiveEnemySkill LiveEnemySkill;
        public override AbstractSkill Skill => LiveEnemySkill;

        public override IEntity Entity => LiveEnemySkill;
    }
}