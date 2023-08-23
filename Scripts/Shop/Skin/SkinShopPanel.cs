using System.Collections.Generic;
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

namespace _GAME.Scripts.Shop
{
    public class SkinShopPanel : AbstractShoppingPanel<SkinShop>
    {

        [SerializeField]
        private TextMeshProUGUI shopTitle;

        [SerializeField]
        private SkinShopItemPanel shopItemSlotPrefab;

        [SerializeField]
        private Transform shopListTransform;

        [SerializeField]
        private Button backButton;


        void Start()
        {
            Shop = new SkinShop();

            Shop.Load();

            ShopItemPanels = new List<AbstractEntityPanel<IShopItem>>();

            foreach (var item in Shop.Items)
            {
               var panel = Instantiate(shopItemSlotPrefab, Vector3.zero, Quaternion.identity, shopListTransform);

                panel.SetData(item);
                panel.SetEventHandler(BuyByCoin, BuyByAds, SelectItem);
                panel.DataChanged();
                ShopItemPanels.Add(panel);
            }

            GameManager.Instance.Wallet.DefaultAccount.Balance.Subscribe((value) =>
            {
                shopTitle.text = "Shop with Coin: " + value.ToString();
            }).AddTo(gameObject);

            backButton?.onClick.AddListener(() => BackToLobby());
        }

        protected void BackToLobby()
        {
            GameManager.Instance.Fire(GameTrigger.BackToLobbyHome);
        }

        protected override void OnBuyFailed(int itemId)
        {
            DebugPro.BrownBold("Buy failed. You may not have enough coin");
        }

        protected override void OnBuySuccess(int itemId)
        {
            GetPanelBy(itemId).DataChanged();
        }

        protected override void OnItemSelected(int itemId)
        {
            IEntity deSelectedItem = Shop.Deselect();

            Shop.Select(itemId);

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