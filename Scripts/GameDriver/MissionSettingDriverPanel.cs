using _GAME.Scripts.Inventory;
using Assets._GAME.Scripts.Enemies;
using Assets._GAME.Scripts.Enemies.Skills;
using Assets._SDK.Game;
using UnityEngine;
using UnityEngine.UI;
using Assets._SDK.Skills.Attributes;
using Assets._GAME.Scripts.Skills.Move;
using Assets._GAME.Scripts.Skills.Live;
using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace Assets._GAME.Scripts.Game
{
    public class MissionSettingDriverPanel : MonoBehaviour
	{
		public TMP_InputField NumberREInputField;
		public TMP_InputField SpeedREInputField;
		public TMP_InputField DamageREInputField;
		public TMP_InputField SpawnREPointsInputField;
		public TMP_InputField RateWakeUpREInputField;
		[SerializeField]
		private Toggle _applyBalanceTogglePrivate;
		public Toggle ApplyBalanceToggle => _applyBalanceTogglePrivate;

		public TMP_InputField NumberGEInputField;
		public TMP_InputField SpeedGEInputField;
		public TMP_InputField DamageGEInputField;
		public TMP_InputField SpawnGEPointsInputField;

		public TMP_InputField RangeActionWalkInputField;
		public TMP_InputField RangeActionRunInputField;
		public TMP_InputField SpeedWalkRunnerInputField;
		public TMP_InputField SpeedRunRunnerInputField;
		public TMP_InputField DecreaseStaminaInputField;
        public TMP_InputField IncreaseHPInputField;

		public TMP_InputField MissionOrderInputField;

		public Button Apply;
		public Button UnlockMissionBtn;

		public int NumberRE, NumberGE, MissionOrder;

		private bool _isApplyActive = false;
		float SpeedRE, DamageRE, SpeedGE, DamageGE, RangeActionWalkRE, RangeActionRunRE, SpeedWalkRunner, SpeedRunRunner, DecreaseStamina, IncreaseHP, RateWakeUpRE;

		string SpawnREPoints, SpawnGEPoints;

		public bool IsApplyActive => _isApplyActive;

		GlobalEnemy GE;
		RegionalEnemy RE;
		Runner Runner;
		// Use this for initialization
		void Start()
        {
			Apply?.onClick.AddListener(() => DetectButton(Apply, _isApplyActive = !_isApplyActive));
			_isApplyActive = false;

			UnlockMissionBtn?.onClick.AddListener(() => UnlockMission());

			SpawnREPointsInputField.onEndEdit.AddListener(UpdateNumberRE);
			SpawnGEPointsInputField.onEndEdit.AddListener(UpdateNumberGE);

			DetectButton(Apply, _isApplyActive);
			NumberREInputField.readOnly = true;
			NumberGEInputField.readOnly = true;
		}

		private void OnEnable()
		{
			DetectButton(Apply, false);
			if (GameManager.Instance.MissionInventory == null) return;

			if(GE == null)
				GE = GameManager.Instance.Resources.GlobalEnemySettings.GlobalEnemy;
			if(RE == null)
				RE = GameManager.Instance.Resources.RegionalEnemySettings.RegionalEnemy;
			if (Runner == null)
				Runner = GameManager.Instance.Resources.RunnerSettings.Runner;

			LoadMissionSetting();
			ShowSettingMission();
		}

		public void DetectButton(Button btn, bool isActive)
		{
			if (MissionOrder < 1) return;

			btn.GetComponent<Image>().color = isActive ? Color.gray : Color.white;

			if (isActive)
			{
				GetSettingMissionFromUI();
				SaveMissionSetting(MissionOrder);
			}
		}

		private void UnlockMission()
		{
			GameDriver.I.OrderMission = MissionOrder;
			GameDriver.I.OwnMisson();
			GameDriver.I.ActiveMisson();

		}

		public void UpdateNumberRE(string text)
		{
			NumberREInputField.text = text.Split(',')
									.Where(x => int.TryParse(x, out _))
									.Select(int.Parse)
									.ToList().Count.ToString();
		}
		public void UpdateNumberGE(string text)
		{
			NumberGEInputField.text = text.Split(',')
									.Where(x => int.TryParse(x, out _))
									.Select(int.Parse)
									.ToList().Count.ToString();
		}



		public void SaveMissionSetting(int orderMission)
		{
			if (orderMission < 1) return;

			GameDriver.I.OrderMission = orderMission;
			GameDriver.I.OwnMisson();
			GameDriver.I.ActiveMisson();
			GameManager.Instance.MissionInventory.LoadPlayingMission();
			Mission missionActive = (Mission)GameManager.Instance.MissionInventory.PlayingMission;

			#region EnemySave
			if(SpeedGE > 0)
			{
				MoveEnemySkillLevel MoveSkillLevelNoobGE = (MoveEnemySkillLevel)GE.MoveEnemySkillSettings.MoveSkill.SkillLevels[0];
				MoveSkillLevelNoobGE.SpeedPoint = new PercentAttribute(SpeedGE, 0);
			}
			
			if(SpeedRE > 0)
			{
				MoveEnemySkillLevel MoveSkillLevelNoobRE = (MoveEnemySkillLevel)RE.MoveEnemySkillSettings.MoveSkill.SkillLevels[0];
				MoveSkillLevelNoobRE.SpeedPoint = new PercentAttribute(SpeedRE, 0);
			}

			if (DamageGE > 0)
			{
				AttackSkillLevel AttackSkillLevelNoobGE = (AttackSkillLevel)GE.AttackSkillSettings.AttackSkill.SkillLevels[0];
				AttackSkillLevelNoobGE.DamagePoint = new PercentAttribute(DamageGE, 0);
			}

			if(DamageRE > 0)
			{ 
				AttackSkillLevel AttackSkillLevelNoobRE = (AttackSkillLevel)RE.AttackSkillSettings.AttackSkill.SkillLevels[0];
				AttackSkillLevelNoobRE.DamagePoint = new PercentAttribute(DamageRE, 0);
			}

			if (RateWakeUpRE > 0)
			{
				SleepSkillLevel SleepSkillLevel = (SleepSkillLevel)RE.SleepSkillSettings.SleepSkill.SkillLevels[0];
				SleepSkillLevel.RateWakeUp = new PercentAttribute(RateWakeUpRE, 0);
			}

			for (int i = 0; i < missionActive.EnemyGroups.Count; i++)
			{
				if (missionActive.EnemyGroups[i].Type == EnemyType.Global && NumberGE > 0)
				{
					missionActive.EnemyGroups[i].Amount = NumberGE;
					missionActive.EnemyGroups[i].level = EnemyLevel.Noob;
				}

				if (missionActive.EnemyGroups[i].Type == EnemyType.Regional && NumberRE > 0)
				{
					missionActive.EnemyGroups[i].Amount = NumberRE;
					missionActive.EnemyGroups[i].level = EnemyLevel.Noob;
				}
			}

			if (string.IsNullOrEmpty(SpawnREPoints) == false)
			{
				missionActive.RESpawnPoints.Clear();
				missionActive.RESpawnPoints = SpawnREPoints
										.Split(',')
										.Where(x => int.TryParse(x, out _))
										.Select(int.Parse)
										.ToList();
			}

			if (string.IsNullOrEmpty(SpawnGEPoints) == false)
			{
				missionActive.GESpawnPoints.Clear();
				missionActive.GESpawnPoints = SpawnGEPoints
										.Split(',')
										.Where(x => int.TryParse(x, out _))
										.Select(int.Parse)
										.ToList();
			}


			#endregion

			#region RunnerSave

			if (SpeedWalkRunner > 0)
			{
				WalkRunnerSkillLevel SpeedWalkSkillLevelNoobRunner = (WalkRunnerSkillLevel)Runner.WalkRunnerSkillSettings.moveSkill.SkillLevels[0];
				SpeedWalkSkillLevelNoobRunner.Speed = new PercentAttribute(SpeedWalkRunner, 0);
				Runner.WalkRunnerSkillLevel = 1;
			}
			
			if(SpeedRunRunner > 0)
			{
				RunRunnerSkillLevel SpeedRunSkillLevelNoobRunner = (RunRunnerSkillLevel)Runner.RunRunnerSkillSettings.moveSkill.SkillLevels[0];
				SpeedRunSkillLevelNoobRunner.Speed = new PercentAttribute(SpeedRunRunner, 0);
				Runner.RunRunnerSkillLevel = 1;
			}
			
			
				MakeMovingSoundSkillLevel MakeSoundSkillLevelNoobRunner = (MakeMovingSoundSkillLevel)Runner.MakeMovingSoundSkillSettings.MakeSoundMovingSkillSkill.SkillLevels[0];
			if (RangeActionWalkRE > 0)
			{
				MakeSoundSkillLevelNoobRunner.MaxRadiusWalk = new PercentAttribute(RangeActionWalkRE, 0);
			}
			if (RangeActionRunRE > 0)
			{
				MakeSoundSkillLevelNoobRunner.MaxRadiusRun = new PercentAttribute(RangeActionRunRE, 0);
			}

				Runner.MakeMovingSoundSkillLevel = 1;
			
			if(DecreaseStamina > 0)
			{
				StaminaSkillLevel StaminaSkillLevelNoobRunner = (StaminaSkillLevel)Runner.StaminaSkillSettings.StaminaSkill.SkillLevels[0];
				StaminaSkillLevelNoobRunner.DecreaseStamina = new PercentAttribute(DecreaseStamina, 0);
				Runner.StaminaSkillLevel = 1;

			}
			
			if(IncreaseHP > 0)
			{
				LiveRunnerSkillLevel LiveSkillLevelNoobRunner = (LiveRunnerSkillLevel)Runner.LiveRunnerSkillSettings.LiveSkill.SkillLevels[0];
				LiveSkillLevelNoobRunner.HealthRegeneration = new PercentAttribute(IncreaseHP, 0);
				Runner.LiveRunnerSkillLevel = 1;

			}
			

			#endregion

		}

		public void LoadMissionSetting()
		{
			GameManager.Instance.MissionInventory.LoadPlayingMission();

			Mission missionActive = (Mission)GameManager.Instance.MissionInventory.PlayingMission;
			MissionOrder = missionActive.Order;

			int levelEnemy = (int)missionActive.EnemyGroups[0].level;
			Debug.Log(levelEnemy);

			#region EnemyLoad
				MoveEnemySkillLevel MoveSkillLevelNoobGE = (MoveEnemySkillLevel)GE.MoveEnemySkillSettings.MoveSkill.SkillLevels[0];
				SpeedGE = (int)MoveSkillLevelNoobGE.SpeedPoint.Point;
			
				MoveEnemySkillLevel MoveSkillLevelNoobRE = (MoveEnemySkillLevel)RE.MoveEnemySkillSettings.MoveSkill.SkillLevels[0];
				SpeedRE = (int)MoveSkillLevelNoobRE.SpeedPoint.Point;
			
				AttackSkillLevel AttackSkillLevelNoobGE = (AttackSkillLevel)GE.AttackSkillSettings.AttackSkill.SkillLevels[0];
				DamageGE =AttackSkillLevelNoobGE.DamagePoint.Point;

				AttackSkillLevel AttackSkillLevelNoobRE = (AttackSkillLevel)RE.AttackSkillSettings.AttackSkill.SkillLevels[0];
				DamageRE = AttackSkillLevelNoobRE.DamagePoint.Point;

				SleepSkillLevel SleepSkillLevelRE = (SleepSkillLevel)RE.SleepSkillSettings.SleepSkill.SkillLevels[levelEnemy-1];
				RateWakeUpRE = SleepSkillLevelRE.RateWakeUp.Point;


			for (int i = 0; i < missionActive.EnemyGroups.Count; i++)
				{
					if (missionActive.EnemyGroups[i].Type == EnemyType.Global)
					{
						NumberGE = missionActive.EnemyGroups[i].Amount;
						missionActive.EnemyGroups[i].level = EnemyLevel.Noob;
					}

					if (missionActive.EnemyGroups[i].Type == EnemyType.Regional)
					{
						NumberRE = missionActive.EnemyGroups[i].Amount;
						missionActive.EnemyGroups[i].level = EnemyLevel.Noob;
					}
				}

			if (missionActive.RESpawnPoints.Count > 0)
			{
				SpawnREPoints = "";
				foreach (var point in missionActive.RESpawnPoints)
					SpawnREPoints += point.ToString() + ",";
				SpawnREPoints = SpawnREPoints.Remove(SpawnREPoints.Length - 1);
			}
				

			if (missionActive.GESpawnPoints.Count > 0)
			{
				SpawnGEPoints = "";
				foreach (var point in missionActive.GESpawnPoints)
					SpawnGEPoints += point.ToString() + ",";
				SpawnGEPoints = SpawnGEPoints.Remove(SpawnGEPoints.Length - 1);
			}
				

			#endregion

			#region RunnerLoad


			WalkRunnerSkillLevel SpeedWalkSkillLevelNoobRunner = (WalkRunnerSkillLevel)Runner.WalkRunnerSkillSettings.moveSkill.SkillLevels[0];
				SpeedWalkRunner = SpeedWalkSkillLevelNoobRunner.Speed.Point;
				Runner.WalkRunnerSkillLevel = 1;

				RunRunnerSkillLevel SpeedRunSkillLevelNoobRunner = (RunRunnerSkillLevel)Runner.RunRunnerSkillSettings.moveSkill.SkillLevels[0];
				SpeedRunRunner = SpeedRunSkillLevelNoobRunner.Speed.Point;
				Runner.RunRunnerSkillLevel = 1;

			
				MakeMovingSoundSkillLevel MakeSoundSkillLevelNoobRunner = (MakeMovingSoundSkillLevel)Runner.MakeMovingSoundSkillSettings.MakeSoundMovingSkillSkill.SkillLevels[0];
				RangeActionWalkRE = MakeSoundSkillLevelNoobRunner.MaxRadiusWalk.Point;
				RangeActionRunRE = MakeSoundSkillLevelNoobRunner.MaxRadiusRun.Point;
				Runner.MakeMovingSoundSkillLevel = 1;

			
				StaminaSkillLevel StaminaSkillLevelNoobRunner = (StaminaSkillLevel)Runner.StaminaSkillSettings.StaminaSkill.SkillLevels[0];
				DecreaseStamina = StaminaSkillLevelNoobRunner.DecreaseStamina.Point;
				Runner.StaminaSkillLevel = 1;

			
				LiveRunnerSkillLevel LiveSkillLevelNoobRunner = (LiveRunnerSkillLevel)Runner.LiveRunnerSkillSettings.LiveSkill.SkillLevels[0];
				IncreaseHP = LiveSkillLevelNoobRunner.HealthRegeneration.Point;
				Runner.LiveRunnerSkillLevel = 1;

			#endregion

		}

		public void GetSettingMissionFromUI()
		{
			int.TryParse(NumberREInputField.text, out NumberRE);
			float.TryParse(SpeedREInputField.text, out SpeedRE);
			float.TryParse(DamageREInputField.text, out DamageRE);
			int.TryParse(NumberGEInputField.text, out NumberGE);
			float.TryParse(SpeedGEInputField.text, out SpeedGE);
			float.TryParse(DamageGEInputField.text, out DamageGE);
			float.TryParse(RangeActionWalkInputField.text, out RangeActionWalkRE);
			float.TryParse(RangeActionRunInputField.text, out RangeActionRunRE);
			float.TryParse(SpeedWalkRunnerInputField.text, out SpeedWalkRunner);
			float.TryParse(SpeedRunRunnerInputField.text, out SpeedRunRunner);
			float.TryParse(DecreaseStaminaInputField.text, out DecreaseStamina);
			float.TryParse(IncreaseHPInputField.text, out IncreaseHP);
			int.TryParse(MissionOrderInputField.text, out MissionOrder);
			SpawnREPoints = SpawnREPointsInputField.text;
			SpawnGEPoints = SpawnGEPointsInputField.text;

			float.TryParse(RateWakeUpREInputField.text, out RateWakeUpRE);
		}

		public void ShowSettingMission()
		{
			NumberREInputField.text = NumberRE.ToString();
			SpeedREInputField.text = SpeedRE.ToString();
			DamageREInputField.text = DamageRE.ToString();
			NumberGEInputField.text = NumberGE.ToString();
			SpeedGEInputField.text = SpeedGE.ToString();
			DamageGEInputField.text = DamageGE.ToString();
			RangeActionWalkInputField.text = RangeActionWalkRE.ToString();
			RangeActionRunInputField.text = RangeActionRunRE.ToString();
			SpeedWalkRunnerInputField.text = SpeedWalkRunner.ToString();
			SpeedRunRunnerInputField.text = SpeedRunRunner.ToString();
			DecreaseStaminaInputField.text = DecreaseStamina.ToString();
			IncreaseHPInputField.text = IncreaseHP.ToString();
			MissionOrderInputField.text = MissionOrder.ToString();
			SpawnREPointsInputField.text = SpawnREPoints;
			SpawnREPointsInputField.textComponent.enableWordWrapping = true;
			SpawnGEPointsInputField.text = SpawnGEPoints;
			SpawnGEPointsInputField.textComponent.enableWordWrapping = true;

			RateWakeUpREInputField.text = RateWakeUpRE.ToString();
		}
	}
}