/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using _GAME.Scripts.Inventory;

namespace FancyScrollView.LobbyMissionList
{
    class ItemData
    {
        public string Message { get; }
        public ItemData(string message)
        {
            Message = message;
        }
    }

    class AddData
    {
        public AddData(Transform content, int index, MissionInventory missionInventory)
        {
            var Item = content.GetChild(index).GetComponent<MissionCell>();
            Item.SetDataItem(missionInventory.ListMission[index]);
        }
    }
}
