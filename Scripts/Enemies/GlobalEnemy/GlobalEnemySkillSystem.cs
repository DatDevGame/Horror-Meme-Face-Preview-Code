using _GAME.Scripts.Inventory;
using _SDK.Inventory;
using Assets._GAME.Scripts.Enemies;
using Assets._GAME.Scripts.Enemies.Skills;
using Assets._GAME.Scripts.Game;
using Assets._GAME.Scripts.Skills;
using Assets._GAME.Scripts.Skills.Live;
using Assets._SDK.Input;
using Assets._SDK.Skills;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public enum SkillLevel
{
	Noob = 1,
	Middle = 2,
	Pro = 3
}
public class GlobalEnemySkillSystem : AbstractSkillSystem
{
	//private CompositeDisposable _disposables;
	private List<AbstractSkillSlot> skillSlots;
	public GlobalEnemyObservable GEObservable;
	private GlobalEnemySettings _enemySettings;
	private GlobalEnemy _enemy;

	public GlobalEnemy Enemy => _enemy;

	protected override ISkillSlotFactory SkillSlotFactory { get; set; }

	public void Init(EnemyLevel enemyLevel, GameInputPanel gameInputPanel = null)
	{
		SkillSlotFactory = new GlobalEnemySkillSlotFactory();
		GEObservable = new GlobalEnemyObservable(gameInputPanel);
		_enemySettings = GameManager.Instance.Resources.GlobalEnemySettings;
		_enemy = _enemySettings.GlobalEnemy;
		//_disposables = new CompositeDisposable();
		skillSlots = new List<AbstractSkillSlot>();

		LoadDefaultEnemySkillSettings();
		LoadSkillWithLevelEnemy(enemyLevel);
	}

	private void LoadDefaultEnemySkillSettings()
	{
		if (_enemy.MoveEnemySkillLevel > 0)
		{
			AttachSkill(_enemy.MoveEnemySkillSettings, _enemy.MoveEnemySkillLevel);
		}

		if (_enemy.ThinkToGlobalSeekSkillLevel > 0)
		{
			AttachSkill(_enemy.ThinkToGlobalSeekSkillSettings, _enemy.ThinkToGlobalSeekSkillLevel);
		}

		if (_enemy.AttackSkillLevel > 0)
		{
			AttachSkill(_enemy.AttackSkillSettings, _enemy.AttackSkillLevel);
		}

		if (_enemy.LiveEnemySkillLevel > 0)
		{
			AttachSkill(_enemy.LiveEnemySkillSettings, _enemy.LiveEnemySkillLevel);
		}
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
		AbstractSkillSlot liveSlot = GetSlotSkillExist(_enemy.LiveEnemySkillSettings.Skill);
		var liveBehaviour = (LiveEnemySkillBehavior)liveSlot.SkillBehavior;
		liveBehaviour.UnSubscribe();

		AbstractSkillSlot atkSlot = GetSlotSkillExist(_enemy.AttackSkillSettings.Skill);
		var atkeBehaviour = (AttackSkillBehavior)atkSlot.SkillBehavior;
		atkeBehaviour.UnSubscribe();
	}

	private void LoadSkillWithLevelEnemy(EnemyLevel enemyLevel)
	{
		foreach (var enemyLevelSetting in _enemy.EnemyLevelSettings)
		{
			if(enemyLevelSetting.levelEnemy == enemyLevel)
			{
				LevelUpSkill(_enemy.MoveEnemySkillSettings, (int)enemyLevelSetting.MoveEnemySkillLevel);
				LevelUpSkill(_enemy.AttackSkillSettings, (int)enemyLevelSetting.AttackSkillLevel);
			}
			
		}
	}

	public void LevelUpSkill(AbstractSkillSettings skillSettings, int index = 1)
	{
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
		slot = AttachSkillSlotForIndex(skillSettings.Skill, index, GEObservable, slot);
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
