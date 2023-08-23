using _SDK.Entities;
using Assets._SDK.Skills;
using UnityEngine;

namespace Assets._GAME.Scripts.Enemies.Skills
{
    [CreateAssetMenu(menuName = "Skill/Enemies/Skills/ThinkToRegionalSeek", fileName = "ThinkToRegionalSeekSkill")]
    public class ThinkToRegionalSeekSkillSettings : AbstractSkillSettings
    {
        public ThinkToRegionalSeekSkill MoveSkill;
        public override AbstractSkill Skill => MoveSkill;

        public override IEntity Entity => MoveSkill;
    }
}