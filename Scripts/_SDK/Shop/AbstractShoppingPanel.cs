using Assets._SDK.Game;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _SDK.UI;
using UnityEngine;
using _SDK.Money;
using Assets._SDK.Ads;
using _SDK.Entities;
using Assets._SDK.UI;
using Assets._SDK.Shop;
using Assets._SDK.Analytics;
using Assets._SDK.Ads;

namespace _SDK.Shop
{
    public abstract class AbstractShoppingPanel<T> : AbstractPanel where T : AbstractShop<IShopItem>
    {
        protected T Shop { get; set; }
        protected List<AbstractEntityPanel<IShopItem>> ShopItemPanels { get; set; }
        public List<IShopItem> Items { get; protected set; }

        public AbstractEntityPanel<IShopItem> GetPanelBy(int itemId)
        {
            if (ShopItemPanels == null || ShopItemPanels.Count == 0) return null;

            return ShopItemPanels.Find(panel => panel.ItemId == itemId);
        }

        protected bool BuyByCoin(IShopItem item)
        {
            bool isSuccessful = Shop.Buy(item);

            if (isSuccessful)
                OnBuySuccess(item.Id);
            else
                OnBuyFailed(item.Id);

            return isSuccessful;
        }

        protected void BuyByAds(IShopItem item)
        {
            AdsManager.Instance.ShowRewarded(isSuccess =>
                             {
                                 if (isSuccess == AdsResult.Success)
                                 {
                                     item.Bought();
									 OnBuySuccess(item.Id);
                                 }
								 else OnBuyFailed(item.Id);

							 }, 0, AnalyticParamKey.REWARD_WEAPON_SHOP);

		}

        protected void SelectItem(IShopItem item)
        {
            OnItemSelected(item.Id);
        }

        protected abstract void OnBuyFailed(int itemId);

        protected abstract void OnBuySuccess(int itemId);


        protected abstract void OnItemSelected(int itemId);

        protected abstract void OnPageNavigated(int itemId);
    }
}