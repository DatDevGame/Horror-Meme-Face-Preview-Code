using UnityEngine;

namespace Assets._SDK.Skills
{
    public interface ISkillSlot
    {
        ISkillLevel SkillLevel { get; }
        void IncreaseLevel(GameObject toGameObject);

        public void LevelUp(GameObject toGameObject);
    }
}