using _GAME.Scripts.Inventory;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SDK.Shop
{
    public class SkinShopItemPanel : AbstractShopItemPanel
    {
        private Skin skin;

        public override void SetData(IShopItem item)
        {
            base.SetData(item);

            skin = (Skin)item;
            Image.sprite     = skin.Image;
            skin.CubeColor.a = 1;
            Image.color = skin.CubeColor;
            Name.text        = skin.Name;
            Description.text = skin.Description;
            Price.text       = skin.Price.Value.ToString();

            DataChanged();
        }

        public override void SetEventHandler(Func<IShopItem, bool> buyByCoin,
            Action<IShopItem> buyByAds,
            Action<IShopItem> selectItem)
        {
            base.SetEventHandler(buyByCoin, buyByAds, selectItem);
        }

    }
}