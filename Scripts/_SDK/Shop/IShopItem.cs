using _SDK.Entities;
using _SDK.Money;
using Assets._SDK.Entities;

namespace _SDK.Shop
{
    public interface IShopItem : IEntity
    {
        public Price Price { get; }

        public bool IsBought { get; }

        public bool IsSelected { get; }

        void Bought();
        void Selected();
    }
}