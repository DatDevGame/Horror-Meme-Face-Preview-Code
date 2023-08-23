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
	public class RegionalEnemySkillSlotFactory : ISkillSlotFactory
	{
		public AbstractSkillSlot CreateSkillSlotFor(ISkill skill)
		{
			if (skill is MoveEnemySkill) return new MoveEnemySkillSlot(skill);
			if (skill is ThinkToRegionalSeekSkill) return new ThinkToRegionalSeekSkillSlot(skill);
			if (skill is AttackSkill) return new AttackSkillSlot(skill);
			if (skill is PatrolSkill) return new PatrolSkillSlot(skill);
			if (skill is SleepSkill) return new SleepSkillSlot(skill);
			if (skill is LiveEnemySkill) return new LiveEnemySkillSlot(skill);

			return null;
		}

		public AbstractSkillSlot CreateSkillSlotForIndex(ISkill skill, int Index = 1)
		{
			if (skill is MoveEnemySkill) return new MoveEnemySkillSlot(skill, Index);
			if (skill is ThinkToRegionalSeekSkill) return new ThinkToRegionalSeekSkillSlot(skill, Index);
			if (skill is AttackSkill) return new AttackSkillSlot(skill, Index);
			if (skill is PatrolSkill) return new PatrolSkillSlot(skill, Index);
			if (skill is SleepSkill) return new SleepSkillSlot(skill, Index);
			if (skill is SleepSkill) return new SleepSkillSlot(skill, Index);
			if (skill is LiveEnemySkill) return new LiveEnemySkillSlot(skill, Index);
			return null;
		}
	}
}