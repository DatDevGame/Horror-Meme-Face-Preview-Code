using System;
using System.Collections.Generic;
using _SDK.Inventory;
using _SDK.Money;
using _SDK.Shop;
using Assets._SDK.Entities;
using Assets._SDK.Inventory.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME.Scripts
{
    [Serializable]
    public class Map : AbstractEntity
    {
        public override int Id => (nameof(Map) + Name).GetHashCode();

        [field: SerializeField, SuffixLabel("Sprite", true), PreviewField(50, ObjectFieldAlignment.Right), PropertyOrder(-1)]
        public Sprite Image { get; set; }

		[field: SerializeField, PropertyOrder(-1)]
		public GameObject ExitDoorGameObject { get; set; }

		[HideLabel, SuffixLabel("Description", true), MultiLineProperty(5), PropertyOrder(-1)]
        public string Description { get; set; }

		private const string GLOBALENEMIES = "ListGlobalEnemies";
        private const string RIGIONALENEMIES = "ListRegionalEnemies";
        private const string RUNNER = "ListRunner";
		private const string TREASURES = "ListPointSpawnCoin";
		private const string NEXTBOT_JUMPSCARE = "ListNextbotJumpScarePoints";
		private const string MONSTER_JUMPSCARE = "ListMonsterJumpScarePoints";
		private const string HEAD_MONSTER_JUMPSCARE = "ListHeadMonsterJumpScarePoints";
		private const string SUPPORT_ITEMS = "ListPointSpawnSupportItems";
		private const string EXIT_DOORS = "ListExitDoor";


		[ShowInInspector, LabelWidth(50)]
        public List<GameObject> globalEnemyPositions;
        [ShowInInspector, LabelWidth(50)]
        public List<GameObject> regionalEnemyPositions;
        [ShowInInspector, LabelWidth(50)]
        public List<GameObject> runerPositions;
		[ShowInInspector, LabelWidth(50)]
		public List<GameObject> TreasurePositions;
        [ShowInInspector, LabelWidth(50)]
        public List<GameObject> NextBotJumpScarePositions;
        [ShowInInspector, LabelWidth(50)]
        public List<GameObject> MonsterjumpScarePositions;
        [ShowInInspector, LabelWidth(50)]
        public List<GameObject> HeadMonsterJumpScarePositions;
		[ShowInInspector, LabelWidth(50)]
		public List<GameObject> SupportItems;
		[ShowInInspector, LabelWidth(50)]
		public List<GameObject> ExitDoors;

#if UNITY_EDITOR

		[field: SerializeField, PropertyOrder(-1)]
		public GameObject mapModel { get; set; }

		[BoxGroup("Function")]
        [Button("Mapping ObjectSpawnEnemy", ButtonSizes.Medium)]
        void MappingRoomObjectSpawnEnemy()
        {
            runerPositions = new List<GameObject>(MappingPosition(RUNNER));
            globalEnemyPositions = new List<GameObject>(MappingPosition(GLOBALENEMIES));
            regionalEnemyPositions = new List<GameObject>(MappingPosition(RIGIONALENEMIES));
            TreasurePositions = new List<GameObject>(MappingPosition(TREASURES));
            NextBotJumpScarePositions = new List<GameObject>(MappingPosition(NEXTBOT_JUMPSCARE));
            MonsterjumpScarePositions = new List<GameObject>(MappingPosition(MONSTER_JUMPSCARE));
            HeadMonsterJumpScarePositions = new List<GameObject>(MappingPosition(HEAD_MONSTER_JUMPSCARE)) ;
			SupportItems = new List<GameObject>(MappingPosition(SUPPORT_ITEMS));
			ExitDoors = new List<GameObject>(MappingPosition(EXIT_DOORS));
		}

        private List<GameObject> MappingPosition(string mapElementType)
        {
            List<GameObject> positions = new List<GameObject>();
            Transform areaHolder = mapModel.transform.Find(mapElementType).transform;
            foreach (Transform child in areaHolder)
            {
                positions.Add(child.gameObject);
            }
            return positions;
        }
#endif
	}
}