using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets._SDK.Skills;

namespace Assets._GAME.Scripts.Skills.DetectNearestTarget
{
    [Serializable]
    public class DetectNearestTargetRunnerSkill : AbstractSkill
    {
        public override int Id => (nameof(DetectNearestTargetRunnerSkill) + Name).GetHashCode();

        [field: SerializeField, ShowInInspector, LabelWidth(50), PropertyOrder(-1)]
        private List<DetectNearestTargetRunnerSkillLevel> _skillLevels;

        [SerializeField]
        [LabelText("ScreenIndicator")]
        [LabelWidth(100)]
        public GameObject ScreenIndicator;

        [SerializeField]
        [LabelText("ArrowIndicator")]
        [LabelWidth(100)]
        public GameObject ArrowIndicator;

        [SerializeField]
        [LabelText("BoxIndicator")]
        [LabelWidth(100)]
        public GameObject BoxIndicator;

        [SerializeField]
        [LabelText("SeekAndKill")]
        [LabelWidth(100)]
        public Color SeekAndKillColor;

        [SerializeField]
        [LabelText("SeekAndKillCollect")]
        [LabelWidth(100)]
        public Color SeekAndKillCollectColor;

        [SerializeField]
        [LabelText("Treasure")]
        [LabelWidth(100)]
        public Color TreasureColor;

        public Sprite KeySprite;
        public Sprite TreasureSprite;
        public Sprite ExitDoorSprite;

        public override List<ISkillLevel> SkillLevels => _skillLevels?.Select(skillLevel => (ISkillLevel)skillLevel).ToList();

    }
}