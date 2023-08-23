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
    public class SupportItem : AbstractEntity, IGameItem
    {
        public override int Id => (nameof(SupportItem) + Name).GetHashCode();

        public Color CubeColor;

		[field: SerializeField, HideLabel, PropertyOrder(-3), SuffixLabel("Sprite", true), PreviewField(50, ObjectFieldAlignment.Right)]
		public Sprite Image { get; set; }

		[field: SerializeField]
		public GameObject Model { get; set; }

        [HideLabel, SuffixLabel("Description", true), MultiLineProperty(5), PropertyOrder(-1)]
        public string Description { get; set; }
    }
}