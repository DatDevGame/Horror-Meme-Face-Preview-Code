using Assets._SDK.Input;
using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._GAME.Scripts.Enemies.Skills
{
    [Serializable]
    public class MoveEnemySkillSlot : AbstractSkillSlot
    {

        public MoveEnemySkillBehavior MoveSkillBehavior;
        public MoveEnemySkillLevel MoveSkillLevel;
        public MoveEnemySkill MoveSkill;

        private GameObject SoundModel;

		public bool IsGlobalEnemy { get; private set; }

        public override MonoBehaviour SkillBehavior => MoveSkillBehavior;

        public MoveEnemySkillSlot(ISkill moveSkill, int levelIndex = DEFAULT_LEVEL_INDEX, bool isGlobalEnemy = false) : base(moveSkill, levelIndex)
        {
            IsGlobalEnemy = isGlobalEnemy;
        }

        protected override void LevelUpBehavior()
        {
            MoveSkillLevel = (MoveEnemySkillLevel)SkillLevel;
            MoveSkillBehavior.LevelUp(MoveSkillLevel, SoundModel);
        }

        protected override void AddBehaviorIfNull(GameObject toGameObject)
        {
            MoveSkillBehavior = toGameObject.GetComponent<MoveEnemySkillBehavior>();
            if (MoveSkillBehavior != null) return;

            MoveSkill = (MoveEnemySkill)Skill;

           if(IsGlobalEnemy)
				SoundModel = UnityEngine.Object.Instantiate(MoveSkill.AudioSourceModel, toGameObject.transform.position, Quaternion.identity, toGameObject.transform);

            MoveSkillBehavior = toGameObject.AddComponent<MoveEnemySkillBehavior>();
        }
    }
}