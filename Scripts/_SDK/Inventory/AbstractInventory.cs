using _SDK.Entities;
using Assets._SDK.Game;
using Assets._SDK.Inventory;
using Assets._SDK.Inventory.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _SDK.Inventory
{
    public abstract class AbstractInventory<T> where T : IGameItem
    {
        public List<T> Items { get; protected set; }

        public abstract void Load();

        public T ActivatingItem => Items.Find(item => item.IsActivated);

        public abstract T DefaultItem { get; }
        public void Activate(int itemId)
        {
            T selectedItem = Items.Find(item => item.Id == itemId);
            selectedItem.Activate();
            
        }


    }
}