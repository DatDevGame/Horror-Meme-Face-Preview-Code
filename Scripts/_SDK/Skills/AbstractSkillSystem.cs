using Assets._SDK.Input;
using UnityEngine;

namespace Assets._SDK.Skills
{
    public abstract class AbstractSkillSystem : MonoBehaviour
    {
        protected abstract ISkillSlotFactory SkillSlotFactory { get; set; }

        protected AbstractSkillSlot AttachSkillSlot(ISkill skill, IObservable input = null)
        {

            AbstractSkillSlot skillSlot = SkillSlotFactory.CreateSkillSlotFor(skill);

            skillSlot.LevelUp(gameObject);

            if (input != null && skillSlot.SkillBehavior is IObserver)
            {
                IObserver hasInputBehavior = (IObserver)skillSlot.SkillBehavior;
                hasInputBehavior.Observe(input);
            }

            return skillSlot;
        }

		protected AbstractSkillSlot AttachSkillSlotForIndex(ISkill skill,int index = 1, IObservable input = null, AbstractSkillSlot skillSlot = null)
		{
            if (skillSlot == null)
				skillSlot = SkillSlotFactory.CreateSkillSlotForIndex(skill, index);

			skillSlot.LevelUp(gameObject);

			if (input != null && skillSlot.SkillBehavior is IObserver)
			{
				IObserver hasInputBehavior = (IObserver)skillSlot.SkillBehavior;
				hasInputBehavior.Observe(input);
			}

			return skillSlot;
		}


	}
}