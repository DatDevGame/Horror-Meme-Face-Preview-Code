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
    public class SkinShop : AbstractShop<IShopItem>
    {
        public override void Load()
        {
            Items = GameManager.Instance.Resources.AllEnemySettings.Values.Select(settings => (IShopItem)settings.Entity).ToList();
        }
    }
}