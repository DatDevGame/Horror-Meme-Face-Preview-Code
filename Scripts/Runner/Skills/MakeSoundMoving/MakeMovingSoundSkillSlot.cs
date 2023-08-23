using Assets._SDK.Input;
using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Move
{
    [Serializable]
    public class MakeMovingSoundSkillSlot : AbstractSkillSlot
    {

        public MakeMovingSoundSkillBehavior makeMovingSoundSkillBehavior;
        public MakeMovingSoundSkillLevel makeMovingSoundSkillLevel;
        public MakeMovingSoundSkill makeMovingSoundSkill;

        public override MonoBehaviour SkillBehavior => makeMovingSoundSkillBehavior;

        public MakeMovingSoundSkillSlot(ISkill moveSkill, int levelIndex = DEFAULT_LEVEL_INDEX) : base(moveSkill, levelIndex)
        {
        }

        protected override void LevelUpBehavior()
        {
            makeMovingSoundSkillLevel = (MakeMovingSoundSkillLevel)SkillLevel;
            makeMovingSoundSkillBehavior.LevelUp(makeMovingSoundSkillLevel);
        }

        protected override void AddBehaviorIfNull(GameObject toGameObject)
        {
            makeMovingSoundSkillBehavior = toGameObject.GetComponentInChildren<MakeMovingSoundSkillBehavior>();
            if (makeMovingSoundSkillBehavior != null) return;
            makeMovingSoundSkill = (MakeMovingSoundSkill)Skill;
            GameObject skillPrefab = UnityEngine.Object.Instantiate(makeMovingSoundSkill.Model, toGameObject.transform.position, Quaternion.identity, toGameObject.transform);
            makeMovingSoundSkillBehavior = skillPrefab.AddComponent<MakeMovingSoundSkillBehavior>();
        }
    }
}