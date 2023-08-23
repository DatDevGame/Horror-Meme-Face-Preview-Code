using Assets._GAME.Scripts.Skills.Move;
using Assets._GAME.Scripts.Skills.Live;
using Assets._GAME.Scripts.Skills.TurnLightOnOff;
using Assets._SDK.Skills;
using System;
using System.Collections;
using UnityEngine;
using Assets._GAME.Scripts.Enemies.Skills;

namespace Assets._GAME.Scripts.Skills
{
	public class GlobalEnemySkillSlotFactory : ISkillSlotFactory
	{
		public AbstractSkillSlot CreateSkillSlotFor(ISkill skill)
		{
			if (skill is MoveEnemySkill) return new MoveEnemySkillSlot(skill);
			if (skill is AttackSkill) return new AttackSkillSlot(skill);
			if (skill is ThinkToGlobalSeekSkill) return new ThinkToGlobalSeekSkillSlot(skill);
			if (skill is AttackSkill) return new AttackSkillSlot(skill);
			if (skill is LiveEnemySkill) return new LiveEnemySkillSlot(skill);
			return null;
		}

		public AbstractSkillSlot CreateSkillSlotForIndex(ISkill skill, int Index = 1)
		{
			if (skill is MoveEnemySkill) return new MoveEnemySkillSlot(skill, Index, true);
			if (skill is AttackSkill) return new AttackSkillSlot(skill, Index);
			if (skill is ThinkToGlobalSeekSkill) return new ThinkToGlobalSeekSkillSlot(skill, Index);
			if (skill is AttackSkill) return new AttackSkillSlot(skill, Index);
			if (skill is LiveEnemySkill) return new LiveEnemySkillSlot(skill, Index);
			return null;
		}
	}
}