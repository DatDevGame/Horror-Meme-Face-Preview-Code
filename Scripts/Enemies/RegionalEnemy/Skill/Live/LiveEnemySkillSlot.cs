using Assets._SDK.Input;
using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using System;
using System.Collections;
using UnityEngine;
using Assets._GAME.Scripts.Enemies.Skills;

namespace Assets._GAME.Scripts.Skills.Live
{
    [Serializable]
    public class LiveEnemySkillSlot : AbstractSkillSlot
    {

        public LiveEnemySkillBehavior LiveEnemySkillBehavior;
        public LiveEnemySkillLevel LiveEnemySkillLevel;
        public LiveEnemySkill LiveEnemySkill;

        private GameObject bloodEffect;
        private GameObject bloodMonsterEffect;
		private GameObject modelCollider;

		public override MonoBehaviour SkillBehavior => LiveEnemySkillBehavior;

        public LiveEnemySkillSlot(ISkill moveSkill, int levelIndex = DEFAULT_LEVEL_INDEX) : base(moveSkill, levelIndex)
        {
        }

        protected override void LevelUpBehavior()
        {
            LiveEnemySkillLevel = (LiveEnemySkillLevel)SkillLevel;
            LiveEnemySkillBehavior.LevelUp(LiveEnemySkillLevel, modelCollider);
            LiveEnemySkillBehavior.SetBloodEffect(bloodEffect, bloodMonsterEffect);
        }

        protected override void AddBehaviorIfNull(GameObject toGameObject)
        {
            LiveEnemySkillBehavior = toGameObject.GetComponent<LiveEnemySkillBehavior>();
            if (LiveEnemySkillBehavior != null) return;

			LiveEnemySkill = (LiveEnemySkill)Skill;
			modelCollider = UnityEngine.Object.Instantiate(LiveEnemySkill.Model, toGameObject.transform.position, Quaternion.identity, toGameObject.transform);

            bloodEffect = LiveEnemySkill.HitEffect;
            bloodMonsterEffect = LiveEnemySkill.HitMonsterEffect;

            LiveEnemySkillBehavior = toGameObject.AddComponent<LiveEnemySkillBehavior>();
		}
    }
} 