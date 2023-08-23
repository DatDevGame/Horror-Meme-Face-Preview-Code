using _GAME.Scripts.Inventory;
using Assets._SDK.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SDK.Shop
{
    public class AbstractShopItemPanel : AbstractEntityPanel<IShopItem>
    {
        public Image           Image;
        public TextMeshProUGUI Name,         Description, Price;
        public Button BuyWithCoinButton, BuyWithAdsButton, SelectButton;

        public override void DataChanged()
        {
            UpdateButtonStatus();
        }

        protected void UpdateButtonStatus()
        {
            BuyWithAdsButton.gameObject.SetActive(!_item.IsBought);
            BuyWithCoinButton.gameObject.SetActive(!_item.IsBought);
            SelectButton.gameObject.SetActive(_item.IsBought);
            SelectButton.interactable = !_item.IsSelected;
        }

        public virtual void SetEventHandler(Func<IShopItem, bool> buyByCoin,
            Action<IShopItem> buyByAds, Action<IShopItem> selectItem)
        {
            BuyWithCoinButton?.onClick.AddListener(() => buyByCoin(_item));

            BuyWithAdsButton?.onClick.AddListener(() => buyByAds(_item));

            SelectButton?.onClick.AddListener(() => selectItem(_item));
        }

    }
}