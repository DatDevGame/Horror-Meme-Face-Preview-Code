using System;
using Assets._GAME.Scripts.Skills;
using Assets._GAME.Scripts.Skills.DetectNearestTarget;
using Assets._GAME.Scripts.Skills.Live;
using Assets._GAME.Scripts.Skills.Move;
using Assets._GAME.Scripts.Skills.TurnLightOnOff;
using Assets._SDK.Entities;
using Assets._SDK.Inventory.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME.Scripts.Inventory
{
    [Serializable]
    public class Runner : AbstractEntity, IGameItem
    {
        public override int Id => (nameof(Runner) + Name).GetHashCode();

		public GameObject Model { get; set; }

		[HideLabel, SuffixLabel("Description", true), MultiLineProperty(5), PropertyOrder(-1)]
        public string Description { get; set; }

		public WalkRunnerSkillSettings WalkRunnerSkillSettings;
		public RunRunnerSkillSettings RunRunnerSkillSettings;
		public LookSkillSettings LookRunnerSkillSettings;
		public JumpRunnerSkillSettings JumpRunnerSkillSettings;
		public GravityRunnerSkillSettings GravityRunnerSkillSettings;
		public LiveRunnerSkillSettings LiveRunnerSkillSettings;
		public MakeMovingSoundSkillSettings MakeMovingSoundSkillSettings;
		public SetLightSkillSettings SetLightSkillSettings;
		public StaminaSkillSettings StaminaSkillSettings;
		public AttackMeleeSkillSettings AttackMeleeSkillSettings;
		public DetectNearestTargetRunnerSkillSettings DetectNearestTargetSkillSettings;


		public int WalkRunnerSkillLevel;
		public int RunRunnerSkillLevel;
		public int LookSkillLevel;
		public int JumpRunnerSkillLevel;
		public int GravityRunnerSkillLevel;
		public int LiveRunnerSkillLevel;
		public int MakeMovingSoundSkillLevel;
		public int SetLightSkillLevel;
		public int StaminaSkillLevel;
		public int AttackMeleeSkillLevel;
		public int DetectNearestTargetSkillLevel;
	}
}