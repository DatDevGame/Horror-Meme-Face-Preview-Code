using System;
using System.Collections.Generic;
using System.Linq;
using _SDK.Inventory;
using _SDK.Money;
using _SDK.Shop;
using Assets._GAME.Scripts.Enemies;
using Assets._GAME.Scripts.Game;
using Assets._GAME.Scripts.GamePlay;
using Assets._GAME.Scripts.Skills.Move;
using Assets._SDK.Entities;
using Assets._SDK.Inventory.Interfaces;
using Assets._SDK.Missions;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace _GAME.Scripts.Inventory
{
    [Serializable]
    public class Mission : AbstractMission, IShopItem
    {
        [field: SerializeField, PropertyOrder(0)]
        public bool IsSpecial { get; set; }

		[field: SerializeField, PropertyOrder(0)]
        public int TargetWin { get; set; }

        [field: SerializeField, PropertyOrder(0)]
        public string Description { get; set; }

        [field: SerializeField, PropertyOrder(0)]
        public int Prize { get; set; }

		[field: SerializeField, PropertyOrder(0)]
		public int MoneyToUnlock { get; set; }

		[field: SerializeField, SuffixLabel("Avatar", true), PropertyOrder(1), PreviewField(50, ObjectFieldAlignment.Right)]
        public Sprite Avatar { get; set; }

        [ShowInInspector, SuffixLabel("DiffcultyAvatar", true), PropertyOrder(2), PreviewField(50, ObjectFieldAlignment.Right)]
        public Sprite DiffcultyAvatar => SpriteDifficulty();

        [field: SerializeField, SuffixLabel("DiffcultyImage", true), PropertyOrder(3), PreviewField(50, ObjectFieldAlignment.Right)]
        public Sprite[] DiffcultyImage { get; set; }

        public Price Price => new Price(Currency.Coin, MoneyToUnlock);

        public Price AdsPrice => new Price(Currency.Ads, 1);

        public bool IsBought { get => IsOwned; }

        public bool IsSelected { get => IsActivated; }

        [ShowInInspector, PropertyOrder(4)]
        public TypeDifficultMission TypeDifficultMission;

        [SerializeField]
        public MapSettings MapSetting;

		[SerializeField]
		public ModeGameMission ModeGamePlay;

        [ShowInInspector, SuffixLabel("DiffcultyAvatar", true), PropertyOrder(2), PreviewField(50, ObjectFieldAlignment.Right)]
        public Sprite ModeGamePlayAvatar => SpriteGamePlayDescription();

        [field: SerializeField, SuffixLabel("DiffcultyImage", true), PropertyOrder(3), PreviewField(50, ObjectFieldAlignment.Right)]
        public Sprite[] GamePlayDescriptionImages { get; set; }


        [TableList(ShowIndexLabels = true), SerializeField, PropertyOrder(99)]
        public List<EnemyGroup> EnemyGroups;

		[SerializeField, PropertyOrder(99)]
		public float PercentSleepEnemy = 20f;
		[SerializeField, PropertyOrder(99)]
		public float PercentPatrolEnemy = 20f;

		[field: SerializeField, PropertyOrder(99)]
		public GameObject TutorialUIEnemy { get; set; }

        [field: SerializeField, PropertyOrder(99)]
        public GameObject TutorialGameObject { get; set; }

        [SerializeField]
		public MaxStaminaSettings MaxStaminaSettings;

		[SerializeField]
		public List<GameObject> SkillSupportItems;

		[SerializeField]
		public List<SupportItemsSettings> SupportItemsSettings;

		public PhotoTypeMission PhotoTypeMission;
        public string TutorialString;
        public string TutorialStringImportant;

        public List<NextBotJumpScareOnMap> NextBotJumpScareSpawnPoint;
        public List<MonsterJumpScareOnMap> MonsterJumpScareSpawnPoint;
        public List<HeadMonsterJumpScareOnMap> HeadMonsterJumpScareSpawnPoint;

        public bool MustReduceHalfEnemy = false;
        public List<int> RESpawnPoints;
		public List<int> GESpawnPoints;
        public int RunnerPosition;
        public int ExitDoorPosition;

        List<GameObject> TrapObj = new List<GameObject>();
        List<GameObject> SkillSupportObjects;

		public Sprite SpriteDifficulty()
        {
            Sprite sprite;
            if (DiffcultyImage.Length < Enum.GetValues(typeof(TypeDifficultMission)).Length) return null;
            else
            {
                switch (TypeDifficultMission)
                {
                    case TypeDifficultMission.Easy:
                        sprite = DiffcultyImage[0];
                        break;

                    case TypeDifficultMission.Medium:
                        sprite = DiffcultyImage[1];
                        break;

                    case TypeDifficultMission.Hard:
                        sprite = DiffcultyImage[2];
                        break;

                    case TypeDifficultMission.Nightmare:
                        sprite = DiffcultyImage[3];
                        break;
                    default:
                        sprite = null;
                        break;
                }
                return sprite;
            }
        }

        public Sprite SpriteGamePlayDescription()
        {
            Sprite sprite;
            //if (GamePlayDescriptionImages.Length < Enum.GetValues(typeof(ModeGameMission)).Length) return null;
			switch (ModeGamePlay)
			{
				case ModeGameMission.Survival:
					sprite = GamePlayDescriptionImages[0];
					break;

				case ModeGameMission.SeekAndKill:
					sprite = GamePlayDescriptionImages[1];
					break;

				case ModeGameMission.Treasure:
					sprite = GamePlayDescriptionImages[2];
					break;
				case ModeGameMission.SeekAndKillCollect:
					sprite = GamePlayDescriptionImages[3];
					break;
				default:
					sprite = null;
					break;
			}
			return sprite;
        }
        public void Bought()
        {
            Own();
        }

        public void Selected()
        {
            Activate();
        }

        public List<GameObject> InitJumpScare()
        {
            TrapObj = new List<GameObject>();

            HandleNextBotJumpScare();
            HandleMonsterJumpScare();
            HandleHeadMonsterJumpScare();

            return TrapObj;
        }
        private void HandleNextBotJumpScare()
        {
            for (int i = 0; i < NextBotJumpScareSpawnPoint.Count; i++)
            {
                int order = NextBotJumpScareSpawnPoint[i].pointSpawn - 1;

                var obj = UnityEngine.Object.Instantiate(NextBotJumpScareSpawnPoint[i].modelNextBotJumpScare
                                , MapSetting.map.NextBotJumpScarePositions[order].transform.position
                                , Quaternion.Euler(MapSetting.map.NextBotJumpScarePositions[order].transform.eulerAngles)
                                , GamePlayLoader.Instance.mapHolder.transform);
                TrapObj.Add(obj);
            }
        }
        private void HandleMonsterJumpScare()
        {
            for (int i = 0; i < MonsterJumpScareSpawnPoint.Count; i++)
            {
                int order = MonsterJumpScareSpawnPoint[i].pointSpawn - 1;

                var obj = UnityEngine.Object.Instantiate(MonsterJumpScareSpawnPoint[i].modelMonsterJumpScare
                                , MapSetting.map.MonsterjumpScarePositions[order].transform.position
                                , Quaternion.Euler(MapSetting.map.MonsterjumpScarePositions[order].transform.eulerAngles)
                                , GamePlayLoader.Instance.mapHolder.transform);
                TrapObj.Add(obj);
            }
        }
        private void HandleHeadMonsterJumpScare()
        {
            for (int i = 0; i < HeadMonsterJumpScareSpawnPoint.Count; i++)
            {
                int order = HeadMonsterJumpScareSpawnPoint[i].pointSpawn - 1;

                var obj = UnityEngine.Object.Instantiate(HeadMonsterJumpScareSpawnPoint[i].modelHeadMonsterJumpScare
                                , MapSetting.map.HeadMonsterJumpScarePositions[order].transform.position
                                , Quaternion.Euler(MapSetting.map.HeadMonsterJumpScarePositions[order].transform.eulerAngles)
                                , GamePlayLoader.Instance.mapHolder.transform);
                TrapObj.Add(obj);
            }
        }
        public List<GameObject> InitSkillSupportItems(Transform transform, Transform parentTranform)
        {
            SkillSupportObjects = new List<GameObject>();

			if (SkillSupportItems?.Count > 0)
			{
				for (int i = 0; i < SkillSupportItems.Count; i++)
                {
					GameObject skillPrefab = UnityEngine.Object.Instantiate(SkillSupportItems[i], transform.position, Quaternion.identity, parentTranform);

                    //float x = position.x + 2.5f * (i + 1) * ((i % 2 == 0) ? 1 : -1);
                    //               skillPrefab.transform.position += new Vector3(x, 0, 0);
                    //position = transform.localPosition
                    int countDoupble = (i + 2) / 2;

					skillPrefab.transform.position += transform.forward * 5;
					skillPrefab.transform.position += ((i % 2 == 0) ? 1 : -1) * 2.5f * countDoupble * transform.right;
                    skillPrefab.transform.position += new Vector3(0, - 0.5f, 0); 

					SkillSupportObjects.Add(skillPrefab);
				}
			}
			return SkillSupportObjects;
		}

		public List<GameObject> InitGameObjectSupportItems(List<GameObject> spawnPoints, Transform parentTranform)
		{
			SkillSupportObjects = new List<GameObject>();

			if (SupportItemsSettings?.Count > 0)
			{
				foreach (var SupportItem in SupportItemsSettings)
				{
					int AmountItems = (int)(SupportItem.percentSpawn * spawnPoints.Count / 100);
                    if(AmountItems < 1)
						AmountItems = 1;

					for (int i = 0; i < AmountItems; i++)
                    {
					    GameObject skillPrefab = UnityEngine.Object.Instantiate(SupportItem.GameObjectSupportItems, spawnPoints[i].transform.position, Quaternion.identity, parentTranform);

						SkillSupportObjects.Add(skillPrefab);
                        spawnPoints.RemoveAt(i);
                    }
                    if (spawnPoints.Count <= 0) return SkillSupportObjects;
                }
            }
			return SkillSupportObjects;
		}
	}

    [Serializable]
    public class NextBotJumpScareOnMap
    {
        public GameObject modelNextBotJumpScare;
        public int pointSpawn;
    }
    [Serializable]
    public class MonsterJumpScareOnMap
    {
        public GameObject modelMonsterJumpScare;
        public int pointSpawn;
    }
    [Serializable]
    public class HeadMonsterJumpScareOnMap
    {
        public GameObject modelHeadMonsterJumpScare;
        public int pointSpawn;
    }

	[Serializable]
	public class SupportItemsSettings
	{
		public GameObject GameObjectSupportItems;
		public int percentSpawn;
	}


	public enum TypeDifficultMission
    {
        Easy,
        Medium,
        Hard,
        Nightmare
    }

	public enum ModeGameMission
	{
		Survival,
		SeekAndKill,
		Treasure,
		SeekAndKillCollect,
	}

    public enum PhotoTypeMission
    {
        None,
        TakePhoto,
        UploadPhoto,
    }
}