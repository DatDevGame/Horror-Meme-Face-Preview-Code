using _SDK.Entities;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace Assets._SDK.Entities
{
    public abstract class AbstractEntity : IEntity
    {
        private string ownedItemKey => "Owned" + Id.ToString();

        [ShowInInspector, LabelWidth(50), GUIColor(1, .5f, .5f), PropertyOrder(-2)]
        public abstract int Id { get; }

        [field: SerializeField, LabelWidth(50), PropertyOrder(-1)]
        public string Name { get; set; }

		[HideInInspector]
        public bool IsOwned => PlayerPrefs.HasKey(ownedItemKey);

        [ShowInInspector]
        public bool IsActivated => IsActivatedKey(Id);

        public void Own()
        {
            PlayerPrefs.SetInt(ownedItemKey, 1);
        }

        public void DeOwn()
        {
            PlayerPrefs.DeleteKey(ownedItemKey);
        }
        public void Activate()
        {
            PlayerPrefs.SetInt(GetActivatedKey(Id), 1);
        }

        public void DeActivate()
        {
            PlayerPrefs.DeleteKey(GetActivatedKey(Id));
        }

        public static string GetActivatedKey(int id)
        {
            return "Activated" + id.ToString();
        }
        public static bool IsActivatedKey(int id)
        {
            return PlayerPrefs.HasKey(GetActivatedKey(id));
        }


    }
}