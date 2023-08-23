/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using _GAME.Scripts.Inventory;
using System.Collections.Generic;
using System.Collections;
using _GAME.Scripts.Shop;
using _SDK.Shop;
using System;

namespace FancyScrollView.WeaponShopList
{
    class WeaponsListLoadingUI : MonoBehaviour
    {
        const int QUALITY_WEAPONS_COMING_SOON = 1;
        [SerializeField] ScrollViewShop scrollView = default;
        [SerializeField] Button prevCellButton = default;
        [SerializeField] Button nextCellButton = default;

        [SerializeField] private WeaponsShopLoadingPanel _weaponsShopLoadingPanel;

        public Transform content;

        private Dictionary<int, WeaponSettings> weaponSettings;
        private WeaponInventory weaponInventory => GameManager.Instance.WeaponInventory;

        private int IndexDefault = 0;

        public ItemCell CurrentItemCell { get; set; }

        void Start()
        {
            prevCellButton?.onClick.AddListener(scrollView.SelectPrevCell);
            nextCellButton?.onClick.AddListener(scrollView.SelectNextCell);
			scrollView.OnSelectionChanged(OnSelectionChanged);

            weaponSettings = GameManager.Instance.Resources.AllWeaponsSettings;
            int countItemNotOwn = weaponSettings.Values
                .Where(v => !v.weapon.IsOwned)
                .Select(v => v).ToList().Count;

            if (countItemNotOwn <= 0)
            {
                this.gameObject.SetActive(false);
                return;
            }

            ItemShopData[] items = Enumerable.Range(0, countItemNotOwn)
                .Select(i => new ItemShopData($"Cell {i}"))
                .ToArray();

            scrollView.UpdateData(items);

            var setDataItem = Enumerable.Range(0, countItemNotOwn)
                .Select(i => new AddData(content, i, weaponInventory))
                .ToArray();

            _weaponsShopLoadingPanel.SetItemHandle();
            OnSelectionChanged(IndexDefault);
            scrollView.SelectCell(IndexDefault);
		}
        void OnSelectionChanged(int index)
        {
            if (content.childCount <= 0) return;

            var itemShop = content.GetChild(index).GetComponent<WeaponShopItemPanel>();
            _weaponsShopLoadingPanel.SetEventGetWeapon(itemShop.ActionBuyAds, itemShop.Weapon);
            _weaponsShopLoadingPanel.SetActiveGetWeaponButton(!itemShop.Weapon.IsOwned);
		}
    }
}
