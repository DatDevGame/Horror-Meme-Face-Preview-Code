using _GAME.Scripts.Inventory;
using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

namespace _SDK.Shop
{
    public class WeaponShopItemPanel : AbstractShopItemPanel
    {
        [SerializeField] private GameObject _fieldButton;
        [SerializeField] private Sprite equipSprite;
        [SerializeField] private Sprite equippedSprite;

        public Weapon Weapon;
        public Action<IShopItem> ActionBuyAds => _actionBuyAds;
        private Action<IShopItem> _actionBuyAds;
        public override void SetData(IShopItem item)
        {
            base.SetData(Weapon);
            Image.sprite     = Weapon.Image;
            Name.text        = Weapon.Name;
            Description.text = Weapon.Description;
            Price.text       = Weapon.Price.Value.ToString();
            DataChanged();

            ShowOrHideButton(false);
        }
        public override void DataChanged()
        {
            base.DataChanged();
            SetEquippedWeapon();
            SetStatesButtonSelect();
        }
        public void ShowOrHideButton(bool isActive)
        {
            if (_fieldButton != null)
                _fieldButton.SetActive(isActive);
        }
        public void DestroyFieldButton() => Destroy(_fieldButton);
        private void SetEquippedWeapon()
        {
            var weaponInventory = GameManager.Instance.WeaponShop;
            if (weaponInventory == null) return;

            if (_item.IsSelected)
                weaponInventory.SetEquippedWeapon(Weapon);

        }

        private void SetStatesButtonSelect()
        {
            if (_item.IsSelected)
                SelectButton.GetComponent<Image>().sprite = equippedSprite;
            else
                SelectButton.GetComponent<Image>().sprite = equipSprite;
        }


        public override void SetEventHandler(Func<IShopItem, bool> buyByCoin,
            Action<IShopItem> buyByAds,
            Action<IShopItem> selectItem)
        {
            base.SetEventHandler(buyByCoin, buyByAds, selectItem);

            _actionBuyAds = buyByAds;

            BuyWithCoinButton.gameObject.SetActive(false);
            BuyWithCoinButton.gameObject.SetActive(false);
        }
    }
}