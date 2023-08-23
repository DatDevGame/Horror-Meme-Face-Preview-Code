using Assets._GAME.Scripts.Skills.Move;
using Assets._GAME.Scripts.Skills.Live;
using Assets._GAME.Scripts.Skills.TurnLightOnOff;
using Assets._SDK.Skills;
using System;
using System.Collections;
using UnityEngine;
using Assets._GAME.Scripts.Skills.DetectNearestTarget;

namespace Assets._GAME.Scripts.Skills
{
    public class RunnerSkillSlotFactory : ISkillSlotFactory
    {
        public AbstractSkillSlot CreateSkillSlotFor(ISkill skill)
        {
            if (skill is WalkRunnerSkill) return new WalkRunnerSkillSlot(skill);
			if (skill is RunRunnerSkill) return new RunRunnerSkillSlot(skill);
			if (skill is LookSkill) return new LookSkillSlot(skill);
			if (skill is JumpRunnerSkill) return new JumpRunnerSkillSlot(skill);
			if (skill is GravityRunnerSkill) return new GravityRunnerSkillSlot(skill);
			if (skill is LiveRunnerSkill) return new LiveRunnerSkillSlot(skill);
			if (skill is MakeMovingSoundSkill) return new MakeMovingSoundSkillSlot(skill);
			if (skill is SetLightSkill) return new SetLightSkillSlot(skill);
			if (skill is StaminaSkill) return new StaminaSkillSlot(skill);
			if (skill is AttackMeleeSkill) return new AttackMeleeSkillSlot(skill);
			if (skill is DetectNearestTargetRunnerSkill) return new DetectNearestTargetRunnerSkillSlot(skill);
			return null;
        }

		public AbstractSkillSlot CreateSkillSlotForIndex(ISkill skill ,int Index = 1 )
		{
			if (skill is WalkRunnerSkill) return new WalkRunnerSkillSlot(skill, Index);
			if (skill is RunRunnerSkill) return new RunRunnerSkillSlot(skill, Index);
			if (skill is LookSkill) return new LookSkillSlot(skill, Index);
			if (skill is JumpRunnerSkill) return new JumpRunnerSkillSlot(skill, Index);
			if (skill is GravityRunnerSkill) return new GravityRunnerSkillSlot(skill, Index);
			if (skill is LiveRunnerSkill) return new LiveRunnerSkillSlot(skill, Index);
			if (skill is MakeMovingSoundSkill) return new MakeMovingSoundSkillSlot(skill, Index);
			if (skill is SetLightSkill) return new SetLightSkillSlot(skill, Index);
			if (skill is StaminaSkill) return new StaminaSkillSlot(skill, Index);
			if (skill is StaminaSkill) return new StaminaSkillSlot(skill, Index);
			if (skill is AttackMeleeSkill) return new AttackMeleeSkillSlot(skill, Index);
			if (skill is DetectNearestTargetRunnerSkill) return new DetectNearestTargetRunnerSkillSlot(skill, Index);
			return null;
		}
	}
}