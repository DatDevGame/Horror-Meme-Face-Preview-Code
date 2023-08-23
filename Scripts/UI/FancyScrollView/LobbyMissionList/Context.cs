/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using _GAME.Scripts.Inventory;
using System;

namespace FancyScrollView.LobbyMissionList
{
    class Context
    {
        public int SelectedIndex = -1;
        public Action<int> OnCellClicked;
        public Action<Mission> OnCellUnlockRequest;
    }
}
