using Assets._SDK.Skills;
using System;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.DetectNearestTarget
{
    [Serializable]
    public class DetectNearestTargetRunnerSkillSlot : AbstractSkillSlot
    {
        public DetectNearestTargetRunnerSkillBehavior DetectNearestTargetRunnerSkillBehavior;
        public DetectNearestTargetRunnerSkillLevel DetectNearestTargetRunnerSkillLevel;
        public DetectNearestTargetRunnerSkill DetectNearestTargetRunnerSkill;

        private DetectNearestTargetRunnerSkill _detectNearestTargetRunnerSkill;
        public override MonoBehaviour SkillBehavior => DetectNearestTargetRunnerSkillBehavior;

        private GameObject _screenIndicator;
        public DetectNearestTargetRunnerSkillSlot(ISkill moveSkill ,int levelIndex = DEFAULT_LEVEL_INDEX) : base(moveSkill ,levelIndex)
        {
        }
        protected override void LevelUpBehavior()
        {
            DetectNearestTargetRunnerSkillLevel = (DetectNearestTargetRunnerSkillLevel)SkillLevel;
            DetectNearestTargetRunnerSkillBehavior.LevelUp(_screenIndicator, 
                _detectNearestTargetRunnerSkill.SeekAndKillColor,
                _detectNearestTargetRunnerSkill.SeekAndKillCollectColor,
                _detectNearestTargetRunnerSkill.TreasureColor,
                _detectNearestTargetRunnerSkill.ExitDoorSprite,
                _detectNearestTargetRunnerSkill.TreasureSprite,
                _detectNearestTargetRunnerSkill.KeySprite);
        }

        protected override void AddBehaviorIfNull(GameObject toGameObject)
        {
            DetectNearestTargetRunnerSkillBehavior = toGameObject.GetComponent<DetectNearestTargetRunnerSkillBehavior>();
			if (DetectNearestTargetRunnerSkillBehavior != null) return;
            DetectNearestTargetRunnerSkillBehavior = toGameObject.AddComponent<DetectNearestTargetRunnerSkillBehavior>();

            _detectNearestTargetRunnerSkill = (DetectNearestTargetRunnerSkill)Skill;

            _screenIndicator = UnityEngine.Object.Instantiate(_detectNearestTargetRunnerSkill.ScreenIndicator,
                toGameObject.transform.position,
                Quaternion.Euler(0, 0, 0), toGameObject.transform.parent);

            _screenIndicator.transform.GetChild(0)
                .GetComponent<OffScreenIndicator>()
                .SetCamera(toGameObject.transform.Find("Camera").GetComponent<Camera>());
        }
    }
}