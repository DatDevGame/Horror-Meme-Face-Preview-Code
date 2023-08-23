using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using Assets._SDK.Skills.Enums;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets._GAME.Scripts.Skills.Move
{
    [Serializable]
    public class StaminaSkillLevel : AbstractSkillLevel
    {
        [SerializeField]
        [LabelText("Modifier")]
        [LabelWidth(50)]
        public ModifierOperator modifierOperator = ModifierOperator.Override;

        [SerializeField]
        [LabelText("Stamina Point")]
        [LabelWidth(50)]
        private PercentAttribute _stanimaPoint;

        [SerializeField]
        [LabelText("Decrease Stamina = Stamina/s")]
        [LabelWidth(50)]
        private PercentAttribute _decreaseStamina;

        public PercentAttribute StaminaPoint { get => _stanimaPoint; set => _stanimaPoint = value; }
        public PercentAttribute DecreaseStamina { get => _decreaseStamina; set => _decreaseStamina = value; }

        public StaminaSkillLevel(PercentAttribute stanimaPoint, PercentAttribute decreaseStamina, int index)
        {
            _stanimaPoint = stanimaPoint;
            _decreaseStamina = decreaseStamina;
            _index = index;
        }
    }
}