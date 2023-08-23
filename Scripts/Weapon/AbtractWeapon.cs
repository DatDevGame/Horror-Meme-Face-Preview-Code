using UnityEngine;
using System;
using Sirenix.OdinInspector;
using Assets._SDK.Entities;
using _SDK.Entities;

namespace Assets._SDK.Weapon
{
    [Serializable]
    public class AbstractWeapon : AbstractEntity, IWeapon
    {
        public static readonly string ActivatedWeaponKey = "ActivatedWeaponShopKey";
        public override int Id => (nameof(AbstractWeapon) + Name).GetHashCode();
        [field: SerializeField]
        public int Order { get; set; }

        public bool IsPlayed => ActivatedWeaponId == Id;

        public bool IsEquipped => ActivatedWeaponId == Id;
        public void ActivatePlayed()
        {
            PlayerPrefs.SetInt(ActivatedWeaponKey, Id);
        }

        public void DeActivatePlayed()
        {
            PlayerPrefs.DeleteKey(ActivatedWeaponKey);
        }

        public static bool HasPlayedWeapon => PlayerPrefs.HasKey(ActivatedWeaponKey);
        public static int ActivatedWeaponId => PlayerPrefs.GetInt(ActivatedWeaponKey);
    }
}