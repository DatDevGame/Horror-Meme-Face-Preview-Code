using System;
using _SDK.Inventory;
using _SDK.Money;
using _SDK.Shop;
using Assets._SDK.Entities;
using Assets._SDK.Inventory.Interfaces;
using Assets._SDK.Skills;
using Assets._SDK.Weapon;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME.Scripts.Inventory
{
    [Serializable]
    public class Weapon : AbstractWeapon, IShopItem, IGameItem, IGameItemSuport
    {
        public override int Id => (nameof(Weapon) + Name).GetHashCode();

        [field: HideInInlineEditors]
        public GameObject Model { get; set; }

		[field: SerializeField]
        public AbstractSkillSettings SkillSettings { get; set; }

        [field: SerializeField, SuffixLabel("Description", true), MultiLineProperty(5), PropertyOrder(-1)]
        public string Description { get; set; }

        [field: SerializeField, HideLabel, PropertyOrder(-3), SuffixLabel("Sprite", true), PreviewField(50, ObjectFieldAlignment.Right)]
        public Sprite Image { get; set; }

        [field: SerializeField]
        public Price Price { get; set; }

		//TODO: Need refactor change to AbstractWeapon.IsEquipped
		public bool IsBought { get => IsOwned; }
        public bool IsSelected { get => IsPlayed; }

        public AbstractSkillSettings SkillUpgrade { get => SkillSettings; }
	    [field: SerializeField]
		public GameObject ModelItemSupport { get; set; }
		[field: SerializeField]
		public ItemSupportSkillUpgrade.TypeItem TypeItems { get; set; }
		public Sprite AvatarSkill { get => Image; }
		[field: SerializeField]
		public int LevelIndex { get; set; }

		public void Bought()
        {
           Own();
        }

        public void Selected()
        {
           Activate();
        }
    }
}