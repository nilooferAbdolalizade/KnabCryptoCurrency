using Domain.Model;
using System;

namespace Infrastructure.ExternalService.Entity
{
    public class CachedCryptoCurrency : CryptoCurrency
    {
        public CachedCryptoCurrency(CryptoCurrency cryptoCurrency, DateTime cachedTime)
        {
            Id = cryptoCurrency.Id;
            CryptoCode = cryptoCurrency.CryptoCode;
            CurrencyQuotes = cryptoCurrency.CurrencyQuotes;
            CachedTime = cachedTime;
        }
        public DateTime CachedTime { get; private set; }
    }
}
