using System;
using System.Collections.Generic;
using System.Linq;
using _GAME.Scripts.Inventory;
using _SDK.Entities;
using _SDK.Shop;
using Assets._GAME.Scripts.Shop;
using Assets._SDK.Ads;
using Assets._SDK.Analytics;
using Assets._SDK.Game;
using Assets._SDK.UI;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _GAME.Scripts.Shop
{
    public class WeaponsShopLoadingPanel : AbstractShoppingPanel<WeaponShop>
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

        [SerializeField]
        private Button _getWeaponButton;

        Animator coinTextAnimator;

        private bool _isCallOneTime = false;

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

            for (int i = 0; i < listOrderWeapons.Count; i++)
                AddHandleItem(shopListTransform?.GetChild(i)?.GetComponent<WeaponShopItemPanel>(), listOrderWeapons[i], i);
        }
        public void SetEventGetWeapon(Action<IShopItem> eventGetWeapon, Weapon item)
        {
            _getWeaponButton.onClick.RemoveAllListeners();
            _getWeaponButton?.onClick.AddListener(() => 
            {
				AdsManager.Instance.ShowRewarded(isSuccess =>
				{
					if (isSuccess == AdsResult.Success)
					{
						item.Bought();
						item.Selected();
						OnItemSelected(item.Id);
						eventGetWeapon(item);
						_getWeaponButton.gameObject.SetActive(false);
					}

				}, GameManager.Instance.MissionInventory.PlayingMission.Order, AnalyticParamKey.REWARD_CHOOSE_GUN);
            });
        }
        public void SetActiveGetWeaponButton(bool isActive) => _getWeaponButton.gameObject.SetActive(isActive);
        private void AddHandleItem(WeaponShopItemPanel objects, Weapon weapon, int childCout)
        {
            if (objects == null || weapon == null) return;

            var panel = objects;
            panel.Weapon = weapon;
            panel.SetData(Shop.Items[childCout]);
            panel.SetEventHandler(BuyByCoin, BuyByAds, SelectItem);
            panel.DataChanged();
            ShopItemPanels.Add(panel);

            panel.BuyWithAdsButton.gameObject.SetActive(false);
            panel.BuyWithCoinButton.gameObject.SetActive(false);
        }
        public void LoadOrderWeapon()
        {
            var resource = GameManager.Instance.Resources;
            listOrderWeapons = resource.AllWeaponOrders.Values
                .Where((weaponId) => !resource.AllWeaponsSettings[weaponId].weapon.IsOwned)
                .Select((weaponId) => resource.AllWeaponsSettings[weaponId].weapon).ToList();

            _getWeaponButton.gameObject.SetActive(listOrderWeapons.Count > 0);
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
            IEntity deSelectedItem = _isCallOneTime ?  Shop.Deselect() : null;
            _isCallOneTime = true;
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