using Assets._SDK.Skills;
using System;
using UnityEngine;

namespace Assets._GAME.Scripts.Enemies.Skills
{
    [Serializable]
    public class SleepSkillSlot : AbstractSkillSlot
    {
        public SleepSkillBehavior SleepSkillBehavior;
        public SleepSkillLevel SleepSkillLevel;
        public SleepSkill SleepSkill;

        private const int SLEEP_LEVEL_PRO_INDEX = 3;
        public override MonoBehaviour SkillBehavior => SleepSkillBehavior;

        public SleepSkillSlot(ISkill moveSkill, int levelIndex = DEFAULT_LEVEL_INDEX) : base(moveSkill, levelIndex)
        {
        }

        protected override void LevelUpBehavior()
        {
            SleepSkillLevel = (SleepSkillLevel)SkillLevel;
            if (_levelIndex < SLEEP_LEVEL_PRO_INDEX)
            {
                var sleepKillNextLevel = (SleepSkillLevel)Skill.GetSkillLevelBy(_levelIndex + 1);
                int maxWakeUpPoint = (int)sleepKillNextLevel.RateWakeUp.Point;
                SleepSkillBehavior.LevelUpRateWakeUpMax(SleepSkillLevel, maxWakeUpPoint);
            }
            else
                SleepSkillBehavior.LevelUp(SleepSkillLevel);
        }

        protected override void AddBehaviorIfNull(GameObject toGameObject)
        {
            SleepSkillBehavior = toGameObject.GetComponent<SleepSkillBehavior>();
            if (SleepSkillBehavior != null) return;

            SleepSkill = (SleepSkill)Skill;

            var position = toGameObject.transform.position;
            GameObject effectSleep = UnityEngine.Object.Instantiate(SleepSkill.EffectSleepModel, new Vector3(position.x, position.y + 1, position.z), Quaternion.identity, toGameObject.transform);
            
            SleepSkillBehavior = toGameObject.AddComponent<SleepSkillBehavior>();
            SleepSkillBehavior.SleepEffect = effectSleep;
        }
    }
}