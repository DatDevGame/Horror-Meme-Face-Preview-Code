using Assets._SDK.Input;
using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Move
{
    [Serializable]
    public class WalkRunnerSkillSlot : AbstractSkillSlot
    {

        private WalkRunnerSkillBehavior _moveSkillBehavior;
        public WalkRunnerSkillLevel moveSkillLevel;
        public WalkRunnerSkill moveSkill;

        public override MonoBehaviour SkillBehavior => _moveSkillBehavior;

        public WalkRunnerSkillSlot(ISkill moveSkill ,int levelIndex = DEFAULT_LEVEL_INDEX) : base(moveSkill ,levelIndex)
        {
        }

        protected override void LevelUpBehavior()
        {
            moveSkillLevel = (WalkRunnerSkillLevel)SkillLevel;
            _moveSkillBehavior.LevelUp(moveSkillLevel);
        }

        protected override void AddBehaviorIfNull(GameObject toGameObject)
        {
            _moveSkillBehavior = toGameObject.GetComponent<WalkRunnerSkillBehavior>();
			if (_moveSkillBehavior != null) return;

            _moveSkillBehavior = toGameObject.AddComponent<WalkRunnerSkillBehavior>();

        }

	}
}