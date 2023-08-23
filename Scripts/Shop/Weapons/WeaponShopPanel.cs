using System.Collections.Generic;
using System.Linq;
using _GAME.Scripts.Inventory;
using _SDK.Entities;
using _SDK.Shop;
using Assets._GAME.Scripts.Shop;
using Assets._SDK.Game;
using Assets._SDK.Logger;
using Assets._SDK.Shop;
using Assets._SDK.UI;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using Button = UnityEngine.UI.Button;

namespace _GAME.Scripts.Shop
{
    public class WeaponShopPanel : AbstractShoppingPanel<WeaponShop>
    {
        [SerializeField]
        private WeaponShopItemPanel shopItemSlotPrefab;

        [SerializeField]
        private Transform shopListTransform;

        [SerializeField]
        private Button backButton;

        [SerializeField]
        private List<Weapon> listOrderWeapons;

        [SerializeField]
        private TMP_Text _coinText;

        Animator coinTextAnimator;

        private List<WeaponShopItemPanel> _weaponShopItemPanels;
        void Start()
        {
            coinTextAnimator = _coinText.transform.parent.GetComponent<Animator>();

            GameManager.Instance.Wallet.DefaultAccount.Balance.Subscribe((value) =>
            {
                _coinText.SetText(value.ToString());
            }).AddTo(this);

            backButton?.onClick.AddListener(() => BackToLobby());
        }
        public void SetItemHandle()
        {
            LoadOrderWeapon();

            Shop = new WeaponShop();

            Shop.Load();

            ShopItemPanels = new List<AbstractEntityPanel<IShopItem>>();

            _weaponShopItemPanels = new List<WeaponShopItemPanel>();

            for (int i = 0; i < Shop.Items.Count; i++)
                AddHandleItem(shopListTransform?.GetChild(i)?.GetComponent<WeaponShopItemPanel>(), listOrderWeapons[i], i);

            AddWeaponComingSoon();
        }
        private void AddWeaponComingSoon()
        {
            var weaponCommingSoon = GameManager.Instance.Resources.WeaponComingSoonSetting.weapon;
            var panel = shopListTransform?.GetChild(Shop.Items.Count)?.GetComponent<WeaponShopItemPanel>();
            panel.Weapon = weaponCommingSoon;
            panel.SetData(weaponCommingSoon);
            panel.DestroyFieldButton();
        }
        private void AddHandleItem(WeaponShopItemPanel objects, Weapon weapon, int childCout)
        {
            if (objects == null || weapon == null) return;

            var panel = objects;
            panel.Weapon = weapon;
            panel.SetData(Shop.Items[childCout]);
            panel.SetEventHandler(BuyByCoin, BuyByAds, SelectItem);
            panel.DataChanged();
            ShopItemPanels.Add(panel);

            _weaponShopItemPanels.Add(panel);
        }
        internal void HideAllFieldButtonShopItem() => _weaponShopItemPanels.ForEach(v => v.ShowOrHideButton(false));
        public void LoadOrderWeapon()
        {
            var resource = GameManager.Instance.Resources;
            listOrderWeapons = resource.AllWeaponOrders.Values.Select((weaponId) => resource.AllWeaponsSettings[weaponId].weapon).ToList();
        }
        protected void BackToLobby()
        {
            GameManager.Instance.Fire(GameTrigger.BackToLobbyHome);
        }

        protected override void OnBuyFailed(int itemId)
        {
            coinTextAnimator.SetTrigger("BuyFailed");
        }

        protected override void OnBuySuccess(int itemId)
        {
            GetPanelBy(itemId).DataChanged();
        }

        protected override void OnItemSelected(int itemId)
        {
            IEntity deSelectedItem = Shop.Deselect();

			//Shop.Select(itemId);
			Weapon weapon = (Weapon)Shop.Items.Find(item => item.Id == itemId);
            //weapon.Selected();

			weapon.ActivatePlayed();

            GetPanelBy(itemId).DataChanged();

            if (deSelectedItem != null)
            {
                GetPanelBy(deSelectedItem.Id).DataChanged();
            }
            
        }

        protected override void OnPageNavigated(int pageID)
        {
        }
    }
}