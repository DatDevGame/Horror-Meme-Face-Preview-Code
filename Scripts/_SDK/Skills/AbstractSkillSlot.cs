using Assets._SDK.Game;
using System;
using UnityEngine;

namespace Assets._SDK.Skills
{
    [Serializable]
    public abstract class AbstractSkillSlot : ISkillSlot
    {
        public const int DEFAULT_LEVEL_INDEX = 1;

        [SerializeField]
        protected int _levelIndex;

        public AbstractSkillSlot(ISkill skill, int levelIndex)
        {
            Skill = skill;
            _levelIndex = levelIndex;
        }

        public abstract MonoBehaviour SkillBehavior { get; }

        public ISkillLevel SkillLevel => Skill.GetSkillLevelBy(_levelIndex);

        public ISkill Skill {get; private set;}

        public void IncreaseLevel(GameObject toGameObject)
        {
            _levelIndex++;
            LevelUp(toGameObject);
        }
        public void LevelUp(GameObject toGameObject)
        {
            AddBehaviorIfNull(toGameObject);
            LevelUpBehavior();
        }

        protected abstract void AddBehaviorIfNull(GameObject toGameObject);
        protected abstract void LevelUpBehavior();

    }
}