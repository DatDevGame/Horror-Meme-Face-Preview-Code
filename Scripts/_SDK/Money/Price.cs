using System;

namespace _SDK.Money
{
    [Serializable]
    public struct Price
    {
        public Currency Currency;
        public float    Value;

        public Price(Currency currency, float value)
        {
            Currency = currency;
            Value = value;
        }
    }
}