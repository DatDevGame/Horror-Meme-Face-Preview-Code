using System;
using System.Collections.Generic;
using System.Linq;
using _SDK.Inventory;
using _SDK.Money;
using _SDK.Shop;
using Assets._GAME.Scripts.Enemies;
using Assets._GAME.Scripts.Enemies.Skills;
using Assets._GAME.Scripts.Skills.Live;
using Assets._GAME.Scripts.Skills.Move;
using Assets._GAME.Scripts.Skills.TurnLightOnOff;
using Assets._SDK.Entities;
using Assets._SDK.Inventory.Interfaces;
using Assets._SDK.Skills;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME.Scripts.Inventory
{
	[Serializable]
	public class GlobalEnemy : AbstractEntity, IGameItem
    {
        public override int Id => (nameof(GlobalEnemy) + Name).GetHashCode();

		public GameObject Model { get; set; }

		[HideLabel, SuffixLabel("Description", true), MultiLineProperty(5), PropertyOrder(-1)]
        public string Description { get; set; }

		public MoveEnemySkillSettings MoveEnemySkillSettings;
		public ThinkToGlobalSeekSkillSettings ThinkToGlobalSeekSkillSettings;
		public AttackSkillSettings AttackSkillSettings;
		//public JumpRunnerSkillSettings JumpRunnerSkillSettings;
		//public GravityRunnerSkillSettings GravityRunnerSkillSettings;
		public LiveEnemySkillSettings LiveEnemySkillSettings;
		//public MakeMovingSoundSkillSettings MakeMovingSoundSkillSettings;
		//public SetLightSkillSettings SetLightSkillSettings;
		//public	StaminaSkillSettings StaminaSkillSettings;


		public int MoveEnemySkillLevel;
		public int ThinkToGlobalSeekSkillLevel;
		public int AttackSkillLevel;
		//public int JumpRunnerSkillLevel;
		//public int GravityRunnerSkillLevel;
		public int LiveEnemySkillLevel;
		//public int MakeMovingSoundSkillLevel;
		//public int SetLightSkillLevel;
		//public int StaminaSkillLevel;

		[TableList(ShowIndexLabels = true), SerializeField, PropertyOrder(99)]
		public List<GlobalEnemyLevelSetting> EnemyLevelSettings;
	}

	[Serializable]
	public class GlobalEnemyLevelSetting
	{

		[VerticalGroup("LevelEnemy"), HideLabel]
		public EnemyLevel levelEnemy = EnemyLevel.Noob;
		[VerticalGroup("SkillLevel"), LabelWidth(135)]
		public SkillLevel MoveEnemySkillLevel = SkillLevel.Noob;
		[VerticalGroup("SkillLevel"), LabelWidth(135)]
		public SkillLevel AttackSkillLevel = SkillLevel.Noob;
	}
}