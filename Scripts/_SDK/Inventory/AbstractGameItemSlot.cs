using System;
using _GAME.Scripts.Inventory;
using _SDK.Inventory.Interfaces;
using Assets._SDK.Inventory.Interfaces;
using UnityEngine;

namespace _SDK.Inventory
{
    public abstract class AbstractGameItemSlot : IGameItemSlot
    {
        public IGameItem Item { get; protected set; }

        public AbstractGameItemSlot(IGameItem item)
        {
            Item = item;
        }


        public void AttachTo(GameObject gameObject)
        {
            AddBehaviorIfNull(gameObject);
            Activate(gameObject);
        }

        protected abstract void AddBehaviorIfNull(GameObject toGameObject);
        protected abstract void Activate(GameObject toGameObject);
    }
}