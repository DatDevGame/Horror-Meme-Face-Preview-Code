/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using _GAME.Scripts.Inventory;
using _SDK.Shop;
using Assets._GAME.Scripts.Shop;

namespace FancyScrollView.WeaponShopList
{
    class ItemCell : FancyCell<ItemShopData, Context>
    {
        [SerializeField] Animator animator = default;
        [SerializeField] Text message = default;
        [SerializeField] Image image = default;
        [SerializeField] Button button = default;
        [SerializeField] Image SelectImage;
        [SerializeField] Sprite sprite;

        [SerializeField] private WeaponShopItemPanel weaponShopItemPanel;
        private Weapon _weapon;
        static class AnimatorHash
        {
            public static readonly int Scroll = Animator.StringToHash("scroll");
        }
        public override void Initialize()
        {
            button.onClick.AddListener(() => Context.OnCellClicked?.Invoke(Index));
        }
        public override void UpdateContent(ItemShopData itemData = null)
        {
            message.text = itemData?.Message;

            var isSelected = Context.SelectedIndex == Index;

            SelectImage.gameObject.SetActive(isSelected);
        }
        public override void UpdatePosition(float position)
        {
            currentPosition = position;

            if (animator.isActiveAndEnabled)
            {
                animator.Play(AnimatorHash.Scroll, -1, position);
            }
            animator.speed = 0;
        }
        float currentPosition = 0;

        void OnEnable() => UpdatePosition(currentPosition);
    }
}
