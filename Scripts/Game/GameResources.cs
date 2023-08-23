using _GAME.Scripts.Inventory;
using Assets._GAME.Scripts.GamePlay;
using Assets._GAME.Scripts.Skills.Move;
using Assets._SDK.Game;
using Assets._SDK.Skills;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets._GAME.Scripts.Game
{
    [ExecuteInEditMode]
    public class GameResources : AbstractGameResources
    {
		#region WEAPONS
		private const string WEAPONS_SETTINGS_FOLDER = "Assets/_GAME/Scripts/Weapon/Settings";
		public Dictionary<int, WeaponSettings> AllWeaponsSettings;
		public SortedList<int, int> AllWeaponOrders;
		public WeaponSettings WeaponDefaultSetting;
		public WeaponSettings WeaponComingSoonSetting;
		#endregion

		#region SKIN
		private const string ENEMY_SETTINGS_FOLDER = "Assets/_GAME/Scripts/Skin/Settings";
		private const string PICTURE_ENEMY_SETTINGS_FILE = "Assets/_GAME/Scripts/Skin/PictureEnemySetting/PictureEnemy.asset";
		public Dictionary<int, SkinSettings> AllEnemySettings;
        public SkinSettings PictureEnemySetting;
		#endregion

		#region Mission
		private const string MISSION_SETTINGS_FOLDER = "Assets/_GAME/Scripts/Mission/Settings";
		public MissionSettings SpecialMissionAds;
		public MissionSettings SpecialGrimaceMissionAds;
		public Dictionary<int, MissionSettings> AllMissionSettings;
		public SortedList<int, int> AllMissionOrders;
		#endregion

		#region Runner
		const string RUNNER_SETTINGS_FILE = "Assets/_GAME/Scripts/Runner/Settings/Runner.asset";
		public RunnerSettings RunnerSettings;
		#endregion

		#region GlobalEnemy
		private const string GLOBAL_ENEMY_SETTINGS_FILE = "Assets/_GAME/Scripts/Enemies/GlobalEnemy/Settings/GlobalEnemy.asset";
		public GlobalEnemySettings GlobalEnemySettings;
		#endregion

		#region RegionalEnemy
		private const string REGIONAL_ENEMY_SETTINGS_FILE = "Assets/_GAME/Scripts/Enemies/RegionalEnemy/Settings/RegionalEnemy.asset";
		public RegionalEnemySettings RegionalEnemySettings;

		private const string REGIONAL_ENEMY_PATROL_SETTINGS_FILE = "Assets/_GAME/Scripts/Enemies/RegionalEnemy/Settings/RegionalEnemyPatrol.asset";
		public RegionalEnemySettings RegionalEnemyPatrolSettings;

		private const string REGIONAL_ENEMY_SLEEP_SETTINGS_FILE = "Assets/_GAME/Scripts/Enemies/RegionalEnemy/Settings/RegionalEnemySleep.asset";
		public RegionalEnemySettings RegionalEnemySleepSettings;
		#endregion

		#region Sound
		private const string SOUND_SETTINGS_FILE = "Assets/_GAME/Scripts/Sound/Settings/SoundSettings.asset";
		public SoundSettings SoundSettings;
        #endregion

		#region Treasure
		private const string TREASURE_GAMEOBJECT_FILE = "Assets/_GAME/Prefabs/GameItems/Treasure.prefab";
		public GameObject Treasure;
		#endregion

		#region CoinGameObject
		private const string COIN_GAMEOBJECT_FILE = "Assets/_GAME/Prefabs/GameItems/CoinGameItem.prefab";
		public GameObject CoinGameItem;
		#endregion

		#region ExitDoor
		private const string EXIT_DOOR_FILE = "Assets/_GAME/Prefabs/Map/ExitDoorGameObject.prefab";
		public GameObject ExitDoorGameObject;
        #endregion


        #region Key
        private const string KEY_GAMEOBJECT_FILE = "Assets/_GAME/Prefabs/GameItems/KeyPrefab.prefab";
        public GameObject Key;
        #endregion

#if UNITY_EDITOR


        private void LoadWeapons()
		{
			AllWeaponsSettings ??= new Dictionary<int, WeaponSettings>();
			AllWeaponsSettings = base.LoadEntitySettings<WeaponSettings, Weapon>(WEAPONS_SETTINGS_FOLDER);

			AllWeaponOrders ??= new SortedList<int, int>();
			AllWeaponOrders.Clear();

			foreach (WeaponSettings settings in AllWeaponsSettings.Values)
			{
				AllWeaponOrders.Add(settings.weapon.Order, settings.Entity.Id);
			}
		}
		private void LoadSkins()
        {
			PictureEnemySetting ??= (SkinSettings) base.LoadSettings(PICTURE_ENEMY_SETTINGS_FILE);
			AllEnemySettings ??= new Dictionary<int, SkinSettings>();
            AllEnemySettings = base.LoadEntitySettings<SkinSettings, Skin>(ENEMY_SETTINGS_FOLDER);
		}

        private void LoadRunner()
		{
			RunnerSettings ??= (RunnerSettings)base.LoadSettings(RUNNER_SETTINGS_FILE);
		}

		private void LoadGlobalEnemy ()
		{
			GlobalEnemySettings ??= (GlobalEnemySettings)base.LoadSettings(GLOBAL_ENEMY_SETTINGS_FILE);
		}

		private void LoadRegionalEnemy()
		{
			RegionalEnemySettings ??= (RegionalEnemySettings)base.LoadSettings(REGIONAL_ENEMY_SETTINGS_FILE);
			RegionalEnemyPatrolSettings ??= (RegionalEnemySettings)base.LoadSettings(REGIONAL_ENEMY_PATROL_SETTINGS_FILE);
			RegionalEnemySleepSettings ??= (RegionalEnemySettings)base.LoadSettings(REGIONAL_ENEMY_SLEEP_SETTINGS_FILE);
		}

		private void LoadSounds()
		{
			SoundSettings ??= (SoundSettings)base.LoadSettings(SOUND_SETTINGS_FILE);
		}

		private void LoadMissions()
		{
			AllMissionSettings ??= new Dictionary<int, MissionSettings>();
			AllMissionSettings = base.LoadEntitySettings<MissionSettings, Mission>(MISSION_SETTINGS_FOLDER);

			AllMissionOrders ??= new SortedList<int, int>();
			AllMissionOrders.Clear();

			foreach (MissionSettings settings in AllMissionSettings.Values)
			{
				AllMissionOrders.Add(settings.Mission.Order, settings.Entity.Id);
			}
		}
		private void LoadTreasure()
		{
			Treasure = AssetDatabase.LoadAssetAtPath(TREASURE_GAMEOBJECT_FILE, typeof(GameObject)) as GameObject;
		}

		private void LoadCoinGameItem()
		{
			CoinGameItem = AssetDatabase.LoadAssetAtPath(COIN_GAMEOBJECT_FILE, typeof(GameObject)) as GameObject;
		}
		private void LoadExitDoor()
		{
			ExitDoorGameObject = AssetDatabase.LoadAssetAtPath(EXIT_DOOR_FILE, typeof(GameObject)) as GameObject;
		}
        private void LoadKey()
        {
            Key = AssetDatabase.LoadAssetAtPath(KEY_GAMEOBJECT_FILE, typeof(GameObject)) as GameObject;
        }

        [Button("Load Resources", ButtonSizes.Medium)]
        public void LoadResources()
        {
            LoadSkins();
			LoadWeapons();
			LoadRunner();
			LoadMissions();
			LoadRegionalEnemy();
			LoadGlobalEnemy();
			LoadSounds();
			LoadTreasure();
			LoadCoinGameItem();
			LoadExitDoor();
			LoadKey();

        }
#endif
    }

}