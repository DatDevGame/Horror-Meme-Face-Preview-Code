
using Assets._SDK.Input;
using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.TurnLightOnOff
{
    [Serializable]
    public class SetLightSkillSlot : AbstractSkillSlot
    {
        public SetLightSkillBehavior setLightSkillBehavior;
        public SetLightSkillLevel setLightSkillLevel;
        public SetLightSkill setLightSkill;

        public override MonoBehaviour SkillBehavior => setLightSkillBehavior;

        public SetLightSkillSlot(ISkill moveSkill, int levelIndex = DEFAULT_LEVEL_INDEX) : base(moveSkill, levelIndex)
        {
        }

        protected override void LevelUpBehavior()
        {
            setLightSkillLevel = (SetLightSkillLevel)SkillLevel;
            setLightSkillBehavior.LevelUp(setLightSkillLevel);
        }

        protected override void AddBehaviorIfNull(GameObject toGameObject)
        {
            setLightSkillBehavior = toGameObject.GetComponentInChildren<SetLightSkillBehavior>();
            if (setLightSkillBehavior != null) return;

            setLightSkill = (SetLightSkill)Skill;
            GameObject skillPrefab = UnityEngine.Object.Instantiate(setLightSkill.Model, toGameObject.transform.position, Quaternion.identity, toGameObject.transform);
            setLightSkillBehavior = skillPrefab.AddComponent<SetLightSkillBehavior>();
        }
    }
}