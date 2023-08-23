using _SDK.Entities;
using Assets._SDK.Entities;
using Assets._SDK.Inventory;
using Assets._SDK.Skills;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets._SDK.Game
{
    public abstract class AbstractGameResources : SerializedMonoBehaviour
    {
#if UNITY_EDITOR
		protected Dictionary<int, TEntitySettings> LoadEntitySettings<TEntitySettings, TEntity>(string path, string filter = "")
				where TEntity : IEntity
				where TEntitySettings : AbstractEntitySettings<TEntity>
		{
			var assets = AssetDatabase.FindAssets(filter, new string[] { path });
			Dictionary<int, TEntitySettings> dictionary = new Dictionary<int, TEntitySettings>();

			for (int i = 0; i < assets.Length; i++)
			{
				TEntitySettings itemSetting = AssetDatabase.LoadAssetAtPath<TEntitySettings>(AssetDatabase.GUIDToAssetPath(assets[i]));
				dictionary.TryAdd(itemSetting.Entity.Id, itemSetting);
			}

			return dictionary;
		}

		protected List<T> LoadScriptableObjectSettings<T>(string path, string filter = "") where T : SerializedScriptableObject
		{
			var assets = AssetDatabase.FindAssets(filter, new string[] { path });
			List<T> list = new List<T>();

			for (int i = 0; i < assets.Length; i++)
			{
				T itemSetting = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(assets[i]));
                if(itemSetting != null)
					list.Add(itemSetting);
			}

			return list;
		}

		protected ScriptableObject LoadSettings(string fullFilePath)
		{
			return (ScriptableObject)AssetDatabase.LoadAllAssetsAtPath(fullFilePath).First();
		}
#endif
	}
}