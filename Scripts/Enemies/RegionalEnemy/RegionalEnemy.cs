using System;
using System.Collections.Generic;
using Assets._GAME.Scripts.Enemies;
using Assets._GAME.Scripts.Enemies.Skills;
using Assets._SDK.Entities;
using Assets._SDK.Inventory.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using Assets._GAME.Scripts.Skills.Live;

namespace _GAME.Scripts.Inventory
{
	[Serializable]
	public class RegionalEnemy : AbstractEntity, IGameItem
    {
        public override int Id => (nameof(RegionalEnemy) + Name).GetHashCode();

		public GameObject Model { get; set; }


		[HideLabel, SuffixLabel("Description", true), MultiLineProperty(5), PropertyOrder(-1)]
        public string Description { get; set; }

		public MoveEnemySkillSettings MoveEnemySkillSettings;
		public ThinkToRegionalSeekSkillSettings ThinkToRegionalSeekSkillSettings;
		public AttackSkillSettings AttackSkillSettings;
		public PatrolSkillSettings PatrolSkillSettings;
		public SleepSkillSettings SleepSkillSettings;
		public LiveEnemySkillSettings LiveEnemySkillSetting;
		//public JumpRunnerSkillSettings JumpRunnerSkillSettings;
		//public GravityRunnerSkillSettings GravityRunnerSkillSettings;
		//public LiveRunnerSkillSettings LiveRunnerSkillSettings;
		//public MakeMovingSoundSkillSettings MakeMovingSoundSkillSettings;
		//public SetLightSkillSettings SetLightSkillSettings;
		//public	StaminaSkillSettings StaminaSkillSettings;


		public int MoveEnemySkillLevel;
		public int ThinkToRegionalSeekSkillLevel;
		public int AttackSkillLevel;
		public int PatrolSkillLevel;
		public int SleepSkillLevel;
		public int LiveSkillLevel;
		//public int JumpRunnerSkillLevel;
		//public int GravityRunnerSkillLevel;
		//public int LiveRunnerSkillLevel;
		//public int MakeMovingSoundSkillLevel;
		//public int SetLightSkillLevel;
		//public int StaminaSkillLevel;

		[TableList(ShowIndexLabels = true), SerializeField, PropertyOrder(99)]
		public List<RegionalSkillLevelSetting> EnemyLevelSettings;

		public int GetSleepSkillLevel(EnemyLevel _enemyLevel)
		{
			return (int)EnemyLevelSettings[(int)_enemyLevel - 1].SleepSkillLevel;
		}
	}

	[Serializable]
	public class RegionalSkillLevelSetting
	{
		[VerticalGroup("LevelEnemy"), HideLabel]
		public EnemyLevel levelEnemy = EnemyLevel.Noob;

		[VerticalGroup("SkillLevel"), LabelWidth(135)]
		public SkillLevel MoveEnemySkillLevel = SkillLevel.Noob;
		[VerticalGroup("SkillLevel"), LabelWidth(135)]
		public SkillLevel AttackSkillLevel = SkillLevel.Noob;
		[VerticalGroup("SkillLevel"), LabelWidth(135)]
		public SkillLevel SleepSkillLevel = SkillLevel.Noob;
		[VerticalGroup("SkillLevel"), LabelWidth(135)]
		public SkillLevel LiveSkillLevel = SkillLevel.Noob;
		[VerticalGroup("SkillLevel"), LabelWidth(135)]
		public SkillLevel PatrolSkillLevel = SkillLevel.Noob;
	}
}