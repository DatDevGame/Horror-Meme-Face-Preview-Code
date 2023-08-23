using Assets._SDK.Skills.Enums;
using Assets._SDK.Utils;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets._SDK.Skills.Attributes
{
    [Serializable]
    public class PercentAttribute : IAttribute
    {
        [SerializeField]
        [LabelText("Point")]
        [LabelWidth(70)]
        public float basePoint;

        [SerializeField]
        [LabelText("Percent")]
        [LabelWidth(70)]
        public int percent;

        public PercentAttribute(float basePoint, int percent)
        {
            this.basePoint = basePoint;
            this.percent = percent;
        }
        public float Point
        {
            get
            {
                return basePoint + percent.OfValue(basePoint);
            }
        }

        public IAttribute Clone()
        {
            return new PercentAttribute(basePoint, percent);
        }

        public PercentAttribute GetModifiedAttribute(ModifierOperator byModifier, PercentAttribute withAttribute)
        {
            float basePoint = this.basePoint;
            int percent = this.percent;

            switch (byModifier)
            {
                case ModifierOperator.Add:
                    basePoint += withAttribute.basePoint;
                    percent += withAttribute.percent;
                    break;
                case ModifierOperator.Override:
                    basePoint = withAttribute.basePoint;
                    percent = withAttribute.percent;
                    break;
                case ModifierOperator.Multiply:
                    break;
                default:
                    break;
            }

            return new PercentAttribute(basePoint, percent);
        }


    }
}