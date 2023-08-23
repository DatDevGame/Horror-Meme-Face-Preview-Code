using _SDK.Entities;
using Assets._SDK.Entities;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using Assets._GAME.Scripts.Enemies;
using UnityEngine.UIElements;

namespace _GAME.Scripts.Inventory
{
	[CreateAssetMenu(menuName = "GameItems/Mode", fileName = "Mode")]
	public class MissionSettings : AbstractEntitySettings<Mission>
	{
		[HideLabel]
		public Mission Mission;
		public override IEntity Entity => Mission;

#if UNITY_EDITOR
		const string PATH_LIST_ENEMY = "Assets/_GAME/Scripts/Skin/Settings";

		public List<EnemyGroup> GetListEnemy()
		{
			var assets = AssetDatabase.FindAssets("", new string[] { PATH_LIST_ENEMY });
			List<EnemyGroup> list = new List<EnemyGroup>();

			for (int i = 0; i < assets.Length; i++)
			{
				SkinSettings Enemy = AssetDatabase.LoadAssetAtPath<SkinSettings>(AssetDatabase.GUIDToAssetPath(assets[i]));
				EnemyGroup dataEnemy = new EnemyGroup();
				dataEnemy.EnemySettings = Enemy;
				//dataEnemy.Name = Enemy.skin.Name;
				list.Add(dataEnemy);
			}
			return list;
		}

		public List<EnemyGroup> CheckListEnemy(List<EnemyGroup> listEnemyCurrent)
		{
			var assets = AssetDatabase.FindAssets("", new string[] { PATH_LIST_ENEMY });

			for (int i = 0; i < assets.Length; i++)
			{
				SkinSettings Enemy = AssetDatabase.LoadAssetAtPath<SkinSettings>(AssetDatabase.GUIDToAssetPath(assets[i]));
				EnemyGroup dataEnemy = new EnemyGroup();
				dataEnemy.EnemySettings = Enemy;
				//dataEnemy.NameEnemy = Enemy.skin.Name;
				if(listEnemyCurrent.FirstOrDefault(item => item.EnemySettings == Enemy) == null)
					listEnemyCurrent.Add(dataEnemy);
			}
			return listEnemyCurrent;
		}


		[Button("Load Resources Mission", ButtonSizes.Medium)]
		public void LoadResources()
		{
			if(Mission.EnemyGroups == null)
				Mission.EnemyGroups = GetListEnemy();
			else
				Mission.EnemyGroups = new List<EnemyGroup>(CheckListEnemy(Mission.EnemyGroups));

		}

		[Button("Genarate Position Auto", ButtonSizes.Medium)]
		public void GenarateAutoPosition ()
		{
			int CountRE = 0;
			int CountGE = 0;
			Mission.RESpawnPoints.Clear();
			Mission.GESpawnPoints.Clear();

			for (int i = 0; i < Mission.EnemyGroups.Count; i++)
			{
				if(Mission.EnemyGroups[i].Type == EnemyType.Regional && Mission.EnemyGroups[i].Amount > 0)
				{
					for(int j = 0; j < Mission.EnemyGroups[i].Amount; j++)
					{
						Mission.RESpawnPoints.Add(CountRE + j + 1);
					}

					CountRE += Mission.EnemyGroups[i].Amount;
				}

				if (Mission.EnemyGroups[i].Type == EnemyType.Global && Mission.EnemyGroups[i].Amount > 0)
				{
					for (int j = 0; j < Mission.EnemyGroups[i].Amount; j++)
					{
						Mission.GESpawnPoints.Add(CountGE + j + 1);
					}

					CountGE += Mission.EnemyGroups[i].Amount;
				}
			}
		}
#endif
	}
}