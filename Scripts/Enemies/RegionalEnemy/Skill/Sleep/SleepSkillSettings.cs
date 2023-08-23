using _SDK.Entities;
using Assets._SDK.Skills;
using UnityEngine;

namespace Assets._GAME.Scripts.Enemies.Skills
{
    [CreateAssetMenu(menuName = "Skill/Enemies/Skills/Sleep", fileName = "SleepSkill")]
    public class SleepSkillSettings : AbstractSkillSettings
    {
        public SleepSkill SleepSkill;
        public override AbstractSkill Skill => SleepSkill;

        public override IEntity Entity => SleepSkill;
    }
}