using System;
using System.Collections.Generic;
using System.Linq;
using _SDK.Inventory;
using Assets._GAME.Scripts.Game;
using Assets._SDK.Missions;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace _GAME.Scripts.Inventory
{
	public class MissionInventory : AbstractMissionInventory
	{
		private GameResources _resources => GameManager.Instance.Resources;

		private List<Mission> _listMission;
		public List<Mission> ListMission { get 
											{ 
												if(_listMission == null) LoadAllMissions();
												return _listMission;
											}}

		private const int ORDER_DEFAULT = 1;

		public void SetPlayingMissionFromEnemyList(Mission mission)
        {
			int missionOrder = GetIndexActiveMisionSetting(mission) - 1;

			if (!mission.IsOwned) mission.Own();
			SetActiveMission(missionOrder);
		}

		public void SetPlayingSpecialMissionAds()
		{
			Mission mission = _resources.SpecialMissionAds.Mission;
			int missionOrder = GetIndexActiveMisionSetting(mission) - 1;

			if (!mission.IsOwned) mission.Own();
			SetActiveMission(missionOrder);
		}

		public void SetGrimacePlayingSpecialMissionAds()
		{
			Mission mission = _resources.SpecialGrimaceMissionAds.Mission;
			int missionOrder = GetIndexActiveMisionSetting(mission) - 1;

			if (!mission.IsOwned) mission.Own();
			SetActiveMission(missionOrder);
		}

		public Mission GetNextMission()
        {
			int playedMissionId = PlayingMission.Id;
			int nextMissionOrderIndex = _resources.AllMissionOrders.IndexOfValue(playedMissionId) + 1;
			int nextMissionId = _resources.AllMissionOrders.Values.Count > nextMissionOrderIndex ?
                _resources.AllMissionOrders.Values[nextMissionOrderIndex] : _resources.AllMissionOrders.Values[0];
			return _resources.AllMissionSettings[nextMissionId].Mission;
		}

		public int GetIndexActiveMisionSetting(Mission mission = null)
		{
			if(mission != null)
            {
				int missionIndex = mission.Order;
				return missionIndex;
			}

			if (Mission.ActivatedMissionId != 0)
				return _resources.AllMissionSettings[Mission.ActivatedMissionId].Mission.Order;
			else
				return ORDER_DEFAULT;

		}

		public override void LoadAllMissions()
		{
			_listMission = _resources.AllMissionOrders.Values.Select((missionId) => _resources.AllMissionSettings[missionId].Mission).ToList();
		}

		public override void LoadLastPlayedMission()
		{
			int playedMissionId = Mission.ActivatedMissionId;
			if (playedMissionId != 0)
			{
				LastPlayedMission = _resources.AllMissionSettings[playedMissionId].Mission;
			}
		}

		public void SetMissionAsPlayed()
		{
			PlayingMission.ActivatePlayed();
		}

		public override void LoadNextPlayingMission()
		{
			int playedMissionId;
			// Choi lan dau tien
			if (!Mission.HasPlayedMission)
			{
				playedMissionId = _resources.AllMissionOrders.Values[0];
				PlayingMission = _resources.AllMissionSettings[playedMissionId].Mission;
				return;
			}

			// Choi lau sau do va load Next Mission
			playedMissionId = Mission.ActivatedMissionId;
			int nextMissionOrderIndex = _resources.AllMissionOrders.IndexOfValue(playedMissionId) + 1;
			int nextMissionId = _resources.AllMissionOrders.Values.Count > nextMissionOrderIndex ?
				_resources.AllMissionOrders.Values[nextMissionOrderIndex] : _resources.AllMissionOrders.Values[0];
			PlayingMission = _resources.AllMissionSettings[nextMissionId].Mission;
		}

		public void LoadPlayingMission()
		{
			int playedMissionId;
			// Choi lan dau tien
			if (!Mission.HasPlayedMission)
			{
				playedMissionId = _resources.AllMissionOrders.Values[0];
				PlayingMission = _resources.AllMissionSettings[playedMissionId].Mission;
				return;
			}

			// Choi lau sau do va load Playing Mission
			playedMissionId = Mission.ActivatedMissionId;
			PlayingMission = _resources.AllMissionSettings[playedMissionId].Mission;
		}

		public void SetActiveMission(int index)
		{
			if (index >= _listMission.Count)
			{
				int randomMissionIndex = UnityEngine.Random.Range(10, 15);
				index = randomMissionIndex;
				SetWinMission(index);
			}
				
			int playedMissionId = _resources.AllMissionOrders.Values[index];
			int nextMissionOrderIndex = _resources.AllMissionOrders.IndexOfValue(playedMissionId);
			int nextMissionId = _resources.AllMissionOrders.Values.Count > nextMissionOrderIndex ?
				_resources.AllMissionOrders.Values[nextMissionOrderIndex] : _resources.AllMissionOrders.Values[0];
			PlayingMission = _resources.AllMissionSettings[nextMissionId].Mission;
			PlayingMission.ActivatePlayed();
		}

		public void SetOwnMission(int index)
		{
			int playedMissionId = _resources.AllMissionOrders.Values[index];
			PlayingMission = _resources.AllMissionSettings[playedMissionId].Mission;
			PlayingMission.Own();
		}

		public void SetWinMission(int index)
		{
			//Unlock Next Mission
			var PlayingNextMission = ListMission[index];
			PlayingNextMission.Own();
		}
	}
}