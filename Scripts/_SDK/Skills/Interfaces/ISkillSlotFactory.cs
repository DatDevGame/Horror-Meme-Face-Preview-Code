using System.Collections;
using UnityEngine;

namespace Assets._SDK.Skills
{
    public interface ISkillSlotFactory 
    {
        public AbstractSkillSlot CreateSkillSlotFor(ISkill skill);
		public AbstractSkillSlot CreateSkillSlotForIndex(ISkill skill, int index);
	}
}