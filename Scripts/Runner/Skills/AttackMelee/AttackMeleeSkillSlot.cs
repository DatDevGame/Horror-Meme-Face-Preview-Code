using _GAME.Scripts.Inventory;
using Assets._SDK.Skills;
using System;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills
{
    [Serializable]
    public class AttackMeleeSkillSlot : AbstractSkillSlot
    {
        public AttackMeleeSkillBehavior attackMeleeSkillBehavior;
        public AttackMeleeSkillLevel attackMeleeSkillLevel;
        public AttackMeleeSkill attackMeleeSkill;

        public GameObject ModelAttackMelee;

        public AudioClip AudioWeapon;

        //WeaponSlot weaponsSlot;
        public override MonoBehaviour SkillBehavior => attackMeleeSkillBehavior;

        public AttackMeleeSkillSlot(ISkill moveSkill, int levelIndex = DEFAULT_LEVEL_INDEX) : base(moveSkill, levelIndex)
        {
        }
        protected override void LevelUpBehavior()
        {
            attackMeleeSkillLevel = (AttackMeleeSkillLevel)SkillLevel;
            attackMeleeSkillBehavior.LevelUp(attackMeleeSkillLevel, ModelAttackMelee?.GetComponentInChildren<Animator>(), AudioWeapon);
		}

        protected override void AddBehaviorIfNull(GameObject toGameObject)
        {
            attackMeleeSkillBehavior = ModelAttackMelee?.GetComponent<AttackMeleeSkillBehavior>();
            if (attackMeleeSkillBehavior != null) return;

			attackMeleeSkill = (AttackMeleeSkill)Skill;
			ModelAttackMelee = UnityEngine.Object.Instantiate(attackMeleeSkill.Model, toGameObject.transform.position, Quaternion.Euler(0, 0, 0), toGameObject.transform);
			ModelAttackMelee.transform.localRotation = Quaternion.Euler(0, 0, 0);

            AudioWeapon = attackMeleeSkill.AudioWeapon;

            attackMeleeSkillBehavior = ModelAttackMelee.AddComponent<AttackMeleeSkillBehavior>();
		}

		public void DestroySkill()
		{
            UnityEngine.Object.Destroy(attackMeleeSkillBehavior);
            UnityEngine.Object.Destroy(ModelAttackMelee);
		}
	}
}