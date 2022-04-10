using System.Collections.Generic;

namespace Infrastructure.ExternalService.Entity
{
    public class CoinMarketCapConfig
    {
        public string ApiKey { get; set; }
        public List<string> Currencies { get; set; }
        public string CoinMarketCapBaseUrl { get; set; }
        public string CryptoCurrencyQuotesLatestUrl { get; set; }
        public int TotalMinuteCachedServiceData { get; set; }

    }
}
