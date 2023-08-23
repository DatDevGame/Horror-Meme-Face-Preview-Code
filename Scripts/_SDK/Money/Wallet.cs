using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;

namespace _SDK.Money
{
    public class Wallet
    {
        public List<Account> Accounts { get; private set; }

        private const Currency DEFAULT_CURRENCY = Currency.Coin;
        public Account DefaultAccount => GetAccountBy(DEFAULT_CURRENCY);

        private readonly List<Currency> _availableCurrencies;

        public Wallet()
        {
            _availableCurrencies = new List<Currency> { DEFAULT_CURRENCY };

            Accounts = _availableCurrencies.Select(currency => new Account(currency)).ToList();

            Load();
        }

        public void Deposit(Price price)
        {
            GetAccountBy(price.Currency).Deposit(price.Value);
        }

        public void Deposit(float amount)
        {
            GetAccountBy(DEFAULT_CURRENCY).Deposit(amount);
        }

        public bool Credit(Price price)
        {
            return GetAccountBy(price.Currency).Credit(price.Value);
        }

        public bool Credit(float amount)
        {
            return GetAccountBy(DEFAULT_CURRENCY).Credit(amount);
        }

        public Account GetAccountBy(Currency type)
        {
            return Accounts.Find(account => account.Currency == type);
        }
        private void Load()
        {
            Accounts.ForEach(account => account.LoadPlayerPrefs());
        }
    }
}