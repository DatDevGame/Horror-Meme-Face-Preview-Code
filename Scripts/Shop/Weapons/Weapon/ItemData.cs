/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using _GAME.Scripts.Inventory;

namespace FancyScrollView.WeaponShopList
{
    class ItemShopData
    {
        public string Message { get; }
        public ItemShopData(string message)
        {
            Message = message;
        }
    }

    class AddData
    {
        public AddData(Transform content, int index, WeaponInventory weaponInventory)
        {
            var Item = content.GetChild(index).GetComponent<ItemCell>();
            //Item.SetDataItem(weaponInventory.ListWeapon[index]);
        }
    }
}
