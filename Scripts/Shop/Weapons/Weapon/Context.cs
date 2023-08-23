/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using _GAME.Scripts.Inventory;
using System;

namespace FancyScrollView.WeaponShopList
{
    class Context
    {
        public int SelectedIndex = -1;
        public Action<int> OnCellClicked;
        public Action<Weapon> OnCellUnlockRequest;
    }
}
