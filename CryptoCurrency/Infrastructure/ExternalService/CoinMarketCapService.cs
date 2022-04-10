using Domain.Entity;
using Domain.IExternalService;
using Domain.Model;
using Infrastructure.ExternalService.Entity;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class CoinMarketCapService : ICoinMarketCapService
    {
        private readonly CoinMarketCapConfig _coinMarketCapConfig;
        private readonly ConcurrentDictionary<string, CachedCryptoCurrency> _cache;
        private readonly ICoinMarketCapCallService _markeCapCallService;
        public CoinMarketCapService(CoinMarketCapConfig coinMarketCapConfig, ICoinMarketCapCallService markeCapCallService)
        {
            _markeCapCallService = markeCapCallService;
            _coinMarketCapConfig = coinMarketCapConfig;
            _cache = new ConcurrentDictionary<string, CachedCryptoCurrency>();
        }

        public async Task<List<CryptoCurrency>> GetLastestQuoteListsAsync(List<CryptoCurrency> cryptoCurrencies)
        {
            var currentCacheTime = DateTime.Now;
            foreach (var crypto in cryptoCurrencies)
            {
                if (!CheckCachedData(crypto.CryptoCode, currentCacheTime))
                {
                    var newCrypto = await GetLastestQuotesByCodeAsync(crypto.CryptoCode);
                    AddNewCryptoToCache(newCrypto, currentCacheTime);
                }
            }

            var currencies = _cache.Values
                .Select(x => new CryptoCurrency()
                {
                    Id = x.Id,
                    CryptoCode = x.CryptoCode,
                    CurrencyQuotes = x.CurrencyQuotes

                }).ToList();
            return currencies;
        }

        private async Task<CryptoCurrency> GetLastestQuotesByCodeAsync(string code)
        {
            var currencyQuoteList = new List<CurrencyQuote>();
            foreach (var currency in _coinMarketCapConfig.Currencies)
            {
                var apiResponse = await _markeCapCallService.MakeLatestQuoteApiCall(code, currency);
                currencyQuoteList.Add(DeserializeCurrencyQuote(apiResponse, code, currency));
            }

            var result = new CryptoCurrency()
            {
                CryptoCode = code,
                CurrencyQuotes = currencyQuoteList
            };

            return result;
        }

        private CurrencyQuote DeserializeCurrencyQuote(ApiResponse apiResponse, string code, string currency)
        {
            CurrencyQuote currencyQuote;
            if (!apiResponse.HasError)
            {
                var lastestQuote = JsonSerializer.Deserialize<LatestQuoteResponse>(apiResponse.Response);
                currencyQuote = new CurrencyQuote()
                {
                    Currency = lastestQuote.Data[code].Quote.First().Key,
                    Price = lastestQuote.Data[code].Quote.First().Value.Price,
                    ErrorMessage = string.Empty
                };
            }
            else
            {
                currencyQuote = new CurrencyQuote()
                {
                    Currency = currency,
                    Price = 0,
                    ErrorMessage = "SourceApiError"
                };
            }

            return currencyQuote;
        }

        private bool CheckCachedData(string code, DateTime currentCacheTime)
        {
            if (_cache.ContainsKey(code))
            {
                return currentCacheTime.Subtract(_cache[code].CachedTime).TotalMinutes <= _coinMarketCapConfig.TotalMinuteCachedServiceData;
            }

            return false;
        }

        private void AddNewCryptoToCache(CryptoCurrency newCrypto, DateTime currentCacheTime)
        {
            _cache.AddOrUpdate(newCrypto.CryptoCode, new CachedCryptoCurrency(newCrypto, currentCacheTime)
                , (key, oldValue) => new CachedCryptoCurrency(newCrypto, currentCacheTime));
        }
    }
}
