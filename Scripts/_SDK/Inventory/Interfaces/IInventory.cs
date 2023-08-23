using Assets._SDK.Inventory.Interfaces;
using System.Collections.Generic;

namespace _SDK.Inventory
{
    // Quan ly cac item trong game bao gom item trong shop, item trong game
    public interface IInventory
    {
        public List<IGameItem> UsingItems { get; }
        public List<IGameItem> OwnedItems { get; }
    }
}