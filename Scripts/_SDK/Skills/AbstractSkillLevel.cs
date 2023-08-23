
using Sirenix.OdinInspector;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets._SDK.Skills
{
    public class AbstractSkillLevel : ISkillLevel
    {
        [SerializeField]
        [LabelText("Index")]
        [LabelWidth(50)]
        [ValidateInput("MustBePositive", "Level index must be positive")]
        protected int _index;

        public int Index => _index;

        private bool MustBePositive(int value)
        {
            return value > 0;
        }
    }
}