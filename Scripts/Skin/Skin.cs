using System;
using _SDK.Inventory;
using _SDK.Money;
using _SDK.Shop;
using Assets._SDK.Entities;
using Assets._SDK.Inventory.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME.Scripts.Inventory
{
    [Serializable]
    public class Skin : AbstractEntity, IGameItem, IShopItem
    {
        public override int Id => (nameof(Skin) + Name + Model.name).GetHashCode();

        public Color CubeColor;

        [field: SerializeField]
        public GameObject Model { get; set; }

        [field: SerializeField]
        public MonsterForm MonsterForm;

        [field: SerializeField]
        public JumpScare JumpScareSkin { get; set; }

        [HideLabel, SuffixLabel("Description", true), MultiLineProperty(5), PropertyOrder(-1)]
        public string Description { get; set; }

        [field: SerializeField, HideLabel, PropertyOrder(-3), SuffixLabel("Sprite", true), PreviewField(50, ObjectFieldAlignment.Right)]
        public Sprite Image { get; set; }

        [field: SerializeField]
        public Price Price { get; set; }
        public bool IsBought { get => IsOwned; }
        public bool IsSelected { get => IsActivated; }

        [field: SerializeField] public MissionSettings DefaultMissionSetting { get; set;}

        public void Bought()
        {
           Own();
        }

        public void Selected()
        {
           Activate();
        }

    }

    [Serializable]
    public class JumpScare
    {
        [field: SerializeField]
        public GameObject ModelJumpScare { get; set; }
        public RuntimeAnimatorController JumpScareAnimation;
    }

    public enum MonsterForm
    {
        NextBot,
        Monster
    }

}