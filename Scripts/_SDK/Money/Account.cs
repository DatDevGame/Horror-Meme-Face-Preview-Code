using System;
using UniRx;
using _SDK.Entities;
using UnityEngine;
using Assets._SDK.Entities;

namespace _SDK.Money
{
    public enum Currency
    {
        Coin    = 0,
        Diamond = 1,
        Ads     = 2,
    }

    [Serializable]
    public class Account : AbstractEntity
    {
        public Currency Currency { get; private set; }
        public override int Id => (nameof(Account) + Name).GetHashCode();

        public ReactiveProperty<float> Balance { get; private set; }

        public Account(Currency currency)
        {
            Currency = currency;
            Name = nameof(Account) + currency.ToString();
            Balance = new ReactiveProperty<float>(0);
        }

        public Account(Currency currency, float value) : this(currency)
        {
            Currency = currency;
            Name = nameof(Account) + currency.ToString();
            Balance.Value = value;
        }

        public void Deposit(float addedValue)
        {
            Balance.Value += addedValue;
            Save();
        }

        public bool Credit(float reducingValue)
        {
            if (Balance.Value < reducingValue) return false;

            Balance.Value -= reducingValue;

            Save();
            return true;
        }

        public void Save(string keyPrefix = "")
        {
            PlayerPrefs.SetFloat(keyPrefix + Id, Balance.Value);
        }

        public void LoadPlayerPrefs(string keyPrefix ="")
        {
            Balance.Value = PlayerPrefs.GetFloat(keyPrefix + Id, 0);
        }
    }
}