using _GAME.Scripts.Inventory;
using Assets._SDK.Skills;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._GAME.Scripts.Enemies
{
    public enum EnemyType
    {
        Global,
        Regional
    }
	public enum RegionalEnemyType
	{
		RE0,
		RE1,
		RE2,
        NONE
	}

	public enum EnemyLevel
    {
        Noob = 1,
        Middle = 2,
        Pro = 3
    }

    [Serializable]
    public class EnemyGroup
    {
        public EnemyType Type;

        [VerticalGroup("NameEnemy"), HideLabel]
        public SkinSettings EnemySettings;

        [VerticalGroup("QualityNumber"), HideLabel]
        public int Amount;

		[VerticalGroup("HasKey"), HideLabel]
		public bool HasKey;

		public EnemyLevel level = EnemyLevel.Noob;
    }
}