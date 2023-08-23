using _GAME.Scripts.Inventory;
using _SDK.Shop;
using Assets._SDK.Shop;
using Assets._SDK.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._GAME.Scripts.Shop
{
    public class WeaponShop : AbstractShop<IShopItem>
    {
        public Weapon EquippedWeapon;

        public override void Load()
        {
            Items = GameManager.Instance.Resources.AllWeaponsSettings.Values.Select(settings =>
            {
                var weapon = (Weapon)settings.Entity;

                if (weapon.IsEquipped)
                    EquippedWeapon = weapon;

                return (IShopItem)weapon;
                }).ToList();

        }
        public void SetEquippedWeapon(Weapon weapon)
        {
            EquippedWeapon = weapon;
            EquippedWeapon.ActivatePlayed();

            Load();
            GameManager.Instance.WeaponInventory?.Load();
        }
    }
}