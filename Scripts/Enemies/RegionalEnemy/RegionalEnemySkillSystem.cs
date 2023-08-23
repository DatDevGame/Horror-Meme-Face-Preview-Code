using _GAME.Scripts.Inventory;
using Assets._GAME.Scripts.Enemies;
using Assets._GAME.Scripts.Enemies.Skills;
using Assets._GAME.Scripts.Skills;
using Assets._GAME.Scripts.Skills.Live;
using Assets._GAME.Scripts.Skills.Move;
using Assets._SDK.Input;
using Assets._SDK.Skills;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

public class RegionalEnemySkillSystem : AbstractSkillSystem
{
	//private CompositeDisposable _disposables;
	private List<AbstractSkillSlot> skillSlots;
	public RegionalEnemyObservable RegionalEnemyObservable;
	private RegionalEnemySettings _enemySettings;
	private RegionalEnemy _enemy;
	private EnemyLevel _enemyLevel;
	protected override ISkillSlotFactory SkillSlotFactory { get; set; }

	public void Init(EnemyLevel enemyLevel, RegionalEnemyType REType, GameInputPanel gameInputPanel = null)
	{
		SkillSlotFactory = new RegionalEnemySkillSlotFactory();

		RegionalEnemyObservable = GetComponent<RegionalEnemyObservable>();
		if (RegionalEnemyObservable == null)
			RegionalEnemyObservable = gameObject.AddComponent<RegionalEnemyObservable>();

        _enemySettings = GetSettingEnemy(REType);
		_enemy = _enemySettings.RegionalEnemy;
		//_disposables = new CompositeDisposable();
		skillSlots = new List<AbstractSkillSlot>();
		_enemyLevel = enemyLevel;
		LoadSkillSettings();
		LoadSkillWithLevelEnemy(enemyLevel);

	}

	private RegionalEnemySettings GetSettingEnemy(RegionalEnemyType REType)
	{
		switch (REType)
		{
			case RegionalEnemyType.RE0:
				return GameManager.Instance.Resources.RegionalEnemySleepSettings;

			case RegionalEnemyType.RE1:
				return GameManager.Instance.Resources.RegionalEnemyPatrolSettings;

			case RegionalEnemyType.RE2:
				return GameManager.Instance.Resources.RegionalEnemySettings;

			case RegionalEnemyType.NONE:
				return GameManager.Instance.Resources.RegionalEnemySettings;

			default:	return GameManager.Instance.Resources.RegionalEnemySettings;
		}
	}

	public void LoadSkillSleep()
	{
		foreach (var child in skillSlots)
		{
			Destroy(child.SkillBehavior);
		}
		AttachSkill(_enemy.SleepSkillSettings, _enemy.GetSleepSkillLevel(_enemyLevel));
		AttachSkill(_enemy.LiveEnemySkillSetting, _enemy.LiveSkillLevel);
	}

	public void RemoveSkill()
	{
		foreach (var skill in skillSlots)
		{
			if (skill != null)
			{
				if (skill.SkillBehavior.gameObject != this.gameObject)
					Destroy(skill.SkillBehavior.gameObject);
				Destroy(skill.SkillBehavior);
			}
		}
		skillSlots.Clear();
	}

	public void UnSubscribeSkillWhenDeath()
	{
		AbstractSkillSlot liveSlot = GetSlotSkillExist(_enemy.LiveEnemySkillSetting.Skill);
		if (liveSlot != null)
		{
			var liveBehaviour = (LiveEnemySkillBehavior)liveSlot.SkillBehavior;
			liveBehaviour.UnSubscribe();
		}

		AbstractSkillSlot atkSlot = GetSlotSkillExist(_enemy.AttackSkillSettings.Skill);
		if (atkSlot != null)
		{
			var atkeBehaviour = (AttackSkillBehavior)atkSlot.SkillBehavior;
			atkeBehaviour.UnSubscribe();
		}
		
	}


	public void LoadSkillWhenWakeUp()
	{
		_enemySettings = GetSettingEnemy(RegionalEnemyType.RE2);
		LoadSkillSettings();
		LoadSkillWithLevelEnemy(_enemyLevel);
	}
	private void LoadSkillSettings()
	{
		if (_enemy.MoveEnemySkillLevel > 0)
		{
			AttachSkill(_enemy.MoveEnemySkillSettings, _enemy.MoveEnemySkillLevel);
		}

		if (_enemy.ThinkToRegionalSeekSkillLevel > 0)
		{
			AttachSkill(_enemy.ThinkToRegionalSeekSkillSettings, _enemy.ThinkToRegionalSeekSkillLevel);
		}

		if (_enemy.AttackSkillLevel > 0)
		{
			AttachSkill(_enemy.AttackSkillSettings, _enemy.AttackSkillLevel);
		}

		if (_enemy.PatrolSkillLevel > 0)
		{
			AttachSkill(_enemy.PatrolSkillSettings, _enemy.PatrolSkillLevel);
		}

		if (_enemy.SleepSkillLevel > 0)
		{
			AttachSkill(_enemy.SleepSkillSettings, _enemy.SleepSkillLevel);
		}
		if (_enemy.LiveSkillLevel > 0)
		{
			AttachSkill(_enemy.LiveEnemySkillSetting, _enemy.LiveSkillLevel);
		}
	}
	private void LoadSkillWithLevelEnemy(EnemyLevel enemyLevel = EnemyLevel.Noob)
	{
		foreach (var enemyLevelSetting in _enemy.EnemyLevelSettings)
		{
			if (enemyLevelSetting.levelEnemy == enemyLevel)
			{
				LevelUpSkill(_enemy.MoveEnemySkillSettings, (int)enemyLevelSetting.MoveEnemySkillLevel);
				LevelUpSkill(_enemy.AttackSkillSettings, (int)enemyLevelSetting.AttackSkillLevel);
				LevelUpSkill(_enemy.SleepSkillSettings, (int)enemyLevelSetting.SleepSkillLevel);
				LevelUpSkill(_enemy.LiveEnemySkillSetting, (int)enemyLevelSetting.LiveSkillLevel);
			}
		}
	}

	public void LevelUpSkill(AbstractSkillSettings skillSettings, int index = 1)
	{
		if (skillSettings == null) return;
		AbstractSkillSlot slot = GetSlotSkillExist(skillSettings.Skill);
		if (slot != null)
		{
			skillSlots.Remove(slot);
			slot = SkillSlotFactory.CreateSkillSlotForIndex(skillSettings.Skill, index);
			slot.LevelUp(gameObject);
			if (slot != null)
				skillSlots.Add(slot);
		}
	}
	private AbstractSkillSlot GetSlotSkillExist(ISkill skill)
	{
		foreach (var Slot in skillSlots)
		{
			if (Slot.Skill == skill)
			{
				return Slot;
			}
		}
		return null;
	}

	private bool IsSkillExist(ISkill skill)
	{
		foreach (var Slot in skillSlots)
		{
			if (Slot.Skill == skill)
			{
				return true;
			}
		}
		return false;
	}

	public void AttachSkill(AbstractSkillSettings skillSettings, int index = 1)
	{
		AbstractSkillSlot slot = GetSlotSkillExistWithIndex(skillSettings.Skill, index);
		slot = AttachSkillSlotForIndex(skillSettings.Skill, index, RegionalEnemyObservable, slot);
		if (slot != null)
			skillSlots.Add(slot);
	}
	private AbstractSkillSlot GetSlotSkillExistWithIndex(ISkill skill, int index = 1)
	{
		foreach (var Slot in skillSlots)
		{
			if (Slot.Skill == skill && Slot.SkillLevel.Index == index)
			{
				return Slot;
			}
		}
		return null;
	}


	private void OnDestroy()
	{
		//_disposables?.Clear();
	}
}
