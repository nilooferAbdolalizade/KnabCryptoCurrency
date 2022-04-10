using Domain.Entity;
using Domain.IExternalService;
using Infrastructure.ExternalService.Entity;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Infrastructure.ExternalService
{
    public class CoinMarketCapCallService : ICoinMarketCapCallService
    {
        private readonly CoinMarketCapConfig _coinMarketCapConfig;

        public CoinMarketCapCallService(CoinMarketCapConfig coinMarketCapConfig)
        {
            _coinMarketCapConfig = coinMarketCapConfig;
        }

        public async Task<ApiResponse> MakeLatestQuoteApiCall(string code, string currency)
        {
            var result = new ApiResponse();
            try
            {
                var url = new UriBuilder(_coinMarketCapConfig.CoinMarketCapBaseUrl + _coinMarketCapConfig.CryptoCurrencyQuotesLatestUrl);

                var queryString = HttpUtility.ParseQueryString(string.Empty);
                queryString["symbol"] = code;
                queryString["convert"] = currency;
                url.Query = queryString.ToString();

                var client = new WebClient();
                client.Headers.Add("X-CMC_PRO_API_KEY", _coinMarketCapConfig.ApiKey);
                client.Headers.Add("Accepts", "application/json");

                var response = await client.DownloadStringTaskAsync(url.Uri);
                result.Response = response;
                result.HasError = false;
                result.ErrorMessage = null;
            }
            catch (Exception ex)
            {
                result.Response = null;
                result.HasError = true;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }
}
