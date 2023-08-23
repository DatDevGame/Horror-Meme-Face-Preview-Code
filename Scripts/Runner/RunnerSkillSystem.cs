using _GAME.Scripts.Inventory;
using Assets._SDK.Input;
using Assets._SDK.Skills;
using Assets._GAME.Scripts.Skills.Move;
using UnityEngine;
using System.Collections.Generic;
using UniRx;
using System;
using Assets._GAME.Scripts.Skills.Live;
using Unity.VisualScripting;
using System.Linq;
using Assets._GAME.Scripts.Skills.DetectNearestTarget;
using Assets._GAME.Scripts.GamePlay;

namespace Assets._GAME.Scripts.Skills
{
    public class RunnerSkillSystem : AbstractSkillSystem
    {
		private CompositeDisposable _disposables;
		private List<AbstractSkillSlot> skillSlots;
		AbstractSkillSlot walkSkillSlot;
		AbstractSkillSlot runSkillSlot;

		public RunnerObservable RunnerObservable;
		private Runner _runner;
		private RunnerSettings _runnerSettings;

		[SerializeField]
		private GameObject _model2Hands;

		public GamePlayingMainPanel GamePlayingMainPanel;

		protected WeaponInventory _weaponInventory;
		protected Weapon playingWeapon;
		protected override ISkillSlotFactory SkillSlotFactory { get; set; }
		int IndexDefault = 1;
		public void Init(GameInputPanel gameInputPanel)
		{
			RunnerObservable = new RunnerObservable(gameInputPanel);
			SkillSlotFactory = new RunnerSkillSlotFactory();
			_runnerSettings = GameManager.Instance.Resources.RunnerSettings;
			_runner = _runnerSettings.Runner;
			_disposables = new CompositeDisposable();
			skillSlots = new List<AbstractSkillSlot>();

			LoadRunnerSettings();

			runSkillSlot.SkillBehavior.enabled = false;
			walkSkillSlot.SkillBehavior.enabled = true;

			RunnerObservable?.IsRunActiveStream.Subscribe((isRunActive) =>
			{
				runSkillSlot.SkillBehavior.enabled = isRunActive;
				walkSkillSlot.SkillBehavior.enabled = !isRunActive;
			}).AddTo(_disposables);

			RunnerObservable?.AttackSkillFromItem.Subscribe((ItemUpgradeSkillSetting) =>
			{
				LevelUpSkill(ItemUpgradeSkillSetting.SkillSeting, ItemUpgradeSkillSetting.Index);
			}).AddTo(_disposables);

			GameManager.Instance.SoundManager.Listener = GetComponent<AudioListener>();
		}

		private void LoadRunnerSettings()
		{
			if (_runner.WalkRunnerSkillLevel > 0)
			{
				AttachSkill(_runner.WalkRunnerSkillSettings, _runner.WalkRunnerSkillLevel);
				walkSkillSlot = GetSlotSkillExistWithIndex(_runner.WalkRunnerSkillSettings.Skill, _runner.WalkRunnerSkillLevel);
			}

			if (_runner.RunRunnerSkillLevel > 0)
			{
				AttachSkill(_runner.RunRunnerSkillSettings, _runner.RunRunnerSkillLevel);
				runSkillSlot = GetSlotSkillExistWithIndex(_runner.RunRunnerSkillSettings.Skill, _runner.RunRunnerSkillLevel);
			}

			if (_runner.LookSkillLevel > 0)
				AttachSkill(_runner.LookRunnerSkillSettings, _runner.LookSkillLevel);
			if (_runner.JumpRunnerSkillLevel > 0)
				AttachSkill(_runner.JumpRunnerSkillSettings, _runner.JumpRunnerSkillLevel);
			if (_runner.GravityRunnerSkillLevel > 0)
				AttachSkill(_runner.GravityRunnerSkillSettings, _runner.GravityRunnerSkillLevel);
			if (_runner.LiveRunnerSkillLevel > 0)
				AttachSkill(_runner.LiveRunnerSkillSettings, _runner.LiveRunnerSkillLevel);
			if (_runner.MakeMovingSoundSkillLevel > 0)
				AttachSkill(_runner.MakeMovingSoundSkillSettings, _runner.MakeMovingSoundSkillLevel);
			//if (_runner.SetLightSkillLevel > 0)
			//	AttachSkill(_runner.SetLightSkillSettings, _runner.SetLightSkillLevel);
			if (_runner.StaminaSkillLevel > 0)
				AttachSkill(_runner.StaminaSkillSettings, _runner.StaminaSkillLevel);
			if (_runner.DetectNearestTargetSkillLevel > 0)
				AttachSkill(_runner.DetectNearestTargetSkillSettings, _runner.DetectNearestTargetSkillLevel);

			_model2Hands?.SetActive(true);
			Animator AnimatorRunner = _model2Hands.GetComponentInChildren<Animator>();
			((WalkRunnerSkillBehavior)walkSkillSlot.SkillBehavior).SetAnimator(AnimatorRunner);
			((RunRunnerSkillBehavior)runSkillSlot.SkillBehavior).SetAnimator(AnimatorRunner);
			AbstractSkillSlot skillSlotAttack = GetSlotSkillExistWithIndex(_runner.AttackMeleeSkillSettings.Skill, _runner.AttackMeleeSkillLevel);
		}

		public void AttachSkillAttack(Weapon weapon)
		{
			_model2Hands?.SetActive(false);
			LoadWeapon(weapon);
		}

		public void UnSubscribeSkillWhenWin()
		{
			var walkBehaviour = (WalkRunnerSkillBehavior)walkSkillSlot.SkillBehavior;
			walkBehaviour.StopWalkAnimation();
			walkBehaviour.UnSubscribe();

			var runBehaviour = (RunRunnerSkillBehavior)runSkillSlot.SkillBehavior;
			runBehaviour.StopRunAnimation();
			runBehaviour.UnSubscribe();

			AbstractSkillSlot liveSlot = GetSlotSkillExist(_runner.LiveRunnerSkillSettings.Skill);
			var liveBehaviour = (LiveRunnerSkillBehavior)liveSlot.SkillBehavior;
			liveBehaviour.UnSubscribe();

			AbstractSkillSlot attackSlot = GetSlotSkillExist(_runner.AttackMeleeSkillSettings.Skill);
			var attackBehaviour = (AttackMeleeSkillBehavior)attackSlot.SkillBehavior;
			attackBehaviour.UnSubscribe();

			AbstractSkillSlot detectNearestTargetSlot = GetSlotSkillExist(_runner.DetectNearestTargetSkillSettings.Skill);
			var detectNearestTargetBehaviour = (DetectNearestTargetRunnerSkillBehavior)detectNearestTargetSlot.SkillBehavior;
			detectNearestTargetBehaviour.UnSubscribe();
		}

		public void RemoveSkill ()
		{
			((WalkRunnerSkillBehavior)walkSkillSlot.SkillBehavior).StopWalkAnimation();
			((RunRunnerSkillBehavior)runSkillSlot.SkillBehavior).StopRunAnimation();
			if(runSkillSlot.SkillBehavior)
				runSkillSlot.SkillBehavior.enabled = false;
			if(walkSkillSlot.SkillBehavior)
				walkSkillSlot.SkillBehavior.enabled = false;

			foreach (var skill in skillSlots)
			{
				if (skill == null) return;
				if (skill.SkillBehavior.gameObject != this.gameObject) 
					Destroy(skill.SkillBehavior.gameObject);

				Destroy(skill.SkillBehavior);
			}

            var indicatorTarget = gameObject.GetComponent<OffScreenTargetIndicator>();
            if (indicatorTarget != null)
                Destroy(indicatorTarget);
            //foreach (var comp in gameObject.GetComponents<Component>())
            //{
            //	if ((comp is MonoBehaviour) && !(comp is RunnerSkillSystem))
            //	{
            //		Destroy(comp);
            //	}
            //}

            skillSlots.Clear();
		}

		public void AttachSkill(AbstractSkillSettings skillSettings,int index = 1)
		{
			AbstractSkillSlot slot = GetSlotSkillExistWithIndex(skillSettings.Skill, index);
			slot = AttachSkillSlotForIndex(skillSettings.Skill, index, RunnerObservable, slot);
			if(slot != null)
				skillSlots.Add(slot);
		}

		private void LoadWeapon(Weapon weapon)
		{
			AttachSkill(weapon.SkillSettings, IndexDefault);
        }

		public void LevelUpSkill(AbstractSkillSettings skillSettings, int index = 1)
		{
			AbstractSkillSlot slot = GetSlotSkillExist(skillSettings.Skill);
			if( slot != null)
			{
				//Skill Attack
				if (slot.SkillBehavior.gameObject != this.gameObject)
				{
					Destroy(slot.SkillBehavior.gameObject);
					Destroy(slot.SkillBehavior);
					skillSlots.Remove(slot);
					AttachSkill(skillSettings, index);
					return;
				}

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
				if (Slot.Skill.GetType() == skill.GetType())
				{
					return Slot;
				}
			}
			return null;
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

		public MonoBehaviour GetSkillBehavior(Type type)
		{
			foreach (var Slot in skillSlots)
			{
				if (Slot.Skill.GetType() == type)
				{
					return Slot.SkillBehavior;
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

		private void OnDestroy()
		{
			_disposables?.Clear();
		}

	}
}