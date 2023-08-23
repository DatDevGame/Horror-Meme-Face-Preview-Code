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

namespace FancyScrollView.WeaponShopList
{
    class WeaponListUI : MonoBehaviour
    {
        const int QUALITY_WEAPONS_COMING_SOON = 1;
        [SerializeField] ScrollViewShop scrollView = default;
        [SerializeField] Button prevCellButton = default;
        [SerializeField] Button nextCellButton = default;

        [SerializeField]private WeaponShopPanel weaponShopPanel;

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
            ItemShopData[] items = Enumerable.Range(0, weaponSettings.Values.Count + QUALITY_WEAPONS_COMING_SOON)
                .Select(i => new ItemShopData($"Cell {i}"))
                .ToArray();

            scrollView.UpdateData(items);

            var setDataItem = Enumerable.Range(0, weaponSettings.Values.Count)
                .Select(i => new AddData(content, i, weaponInventory))
                .ToArray();

            weaponShopPanel.SetItemHandle();
        }
        private void OnEnable()
        {
            StartCoroutine(OnSelectBefore());
        }
        private int GetWeaponSelectedBefore()
        {
            var weapon = GameManager.Instance.WeaponShop.EquippedWeapon;
            if(weapon != null)
				IndexDefault = weapon.Order;
			return IndexDefault;
        }
        IEnumerator OnSelectBefore()
        {
            int orderMission = GetWeaponSelectedBefore();
            yield return new WaitForSeconds(0.3f);
            scrollView.SelectCell(orderMission - 1);
            var itemShop = content.GetChild(orderMission).GetComponent<WeaponShopItemPanel>();
            itemShop.ShowOrHideButton(true);
        }
        void OnSelectionChanged(int index)
        {
            var itemShop = content.GetChild(index).GetComponent<WeaponShopItemPanel>();
            weaponShopPanel.HideAllFieldButtonShopItem();
            itemShop.ShowOrHideButton(true);
        }
    }
}
