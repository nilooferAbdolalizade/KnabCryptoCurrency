using Domain.Entity;
using Domain.IExternalService;
using Domain.Model;
using FluentAssertions;
using Infrastructure.ExternalService.Entity;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Infrastructure.Test
{
    public class CoinMarketCapServiceTest
    {
        [Fact]
        public async Task CoinMarketService_CacheOperation_CallMarketApiOnce()
        {
            var config = new CoinMarketCapConfig();
            var marketCapService = new Mock<ICoinMarketCapCallService>();

            var jsonData = "";
            using (var fileStream = new StreamReader("Json/" + $"BitcoinQuoteUSD" + ".json"))
            {
                jsonData = fileStream.ReadToEnd();
            }
            var response = new ApiResponse()
            {
                ErrorMessage = string.Empty,
                HasError = false,
                Response = jsonData
            };
            marketCapService.Setup(e => e.MakeLatestQuoteApiCall(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(response);

            config.Currencies = new List<string>() { "USD" };
            config.TotalMinuteCachedServiceData = 1;
            var serviceUnderTest = new CoinMarketCapService(config, marketCapService.Object);

            var cryptoList = new List<CryptoCurrency>() {
                new CryptoCurrency() { CryptoCode = "BTC" }
            };

            // act
            await serviceUnderTest.GetLastestQuoteListsAsync(cryptoList);
            await serviceUnderTest.GetLastestQuoteListsAsync(cryptoList);

            //assert
            marketCapService.Verify(x => x.MakeLatestQuoteApiCall(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CoinMarketService_ApiCall_GetErrorOnCalling()
        {
            var config = new CoinMarketCapConfig();
            var marketCapService = new Mock<ICoinMarketCapCallService>();

            var apiResponse = new ApiResponse()
            {
                ErrorMessage = "Error",
                HasError = true,
                Response = ""
            };
            marketCapService.Setup(e => e.MakeLatestQuoteApiCall(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(apiResponse);

            config.Currencies = new List<string>() { "USD" };
            config.TotalMinuteCachedServiceData = 1;
            var serviceUnderTest = new CoinMarketCapService(config, marketCapService.Object);

            var cryptoList = new List<CryptoCurrency>() {
                new CryptoCurrency() { CryptoCode = "BTC" }
            };

            // act
            var response = await serviceUnderTest.GetLastestQuoteListsAsync(cryptoList);

            //assert
            response[0].CryptoCode.Equals("BTC");
            response[0].CurrencyQuotes.Should().ContainSingle(e => e.ErrorMessage == "SourceApiError");
            response[0].CurrencyQuotes.Should().ContainSingle(e => e.Price == 0);
        }


        [Theory]
        [InlineData("BitcoinQuoteBRL", "BRL", "BTC", 206458.606804587)]
        [InlineData("BitcoinQuoteUSD", "USD", "BTC", 401721.476664939)]
        public async Task CoinMarketService_GetLatestQuoteListFromApi_CorrectData(string fileName, string currency, string code, decimal price)
        {
            var config = new CoinMarketCapConfig();
            var marketCapService = new Mock<ICoinMarketCapCallService>();

            var jsonData = "";
            using (var fileStream = new StreamReader("Json/" + $"{fileName}" + ".json"))
            {
                jsonData = fileStream.ReadToEnd();
            }
            var apiResponse = new ApiResponse()
            {
                ErrorMessage = string.Empty,
                HasError = false,
                Response = jsonData
            };
            marketCapService.Setup(e => e.MakeLatestQuoteApiCall(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(apiResponse);

            config.Currencies = new List<string>() { currency };
            config.TotalMinuteCachedServiceData = 1;
            var serviceUnderTest = new CoinMarketCapService(config, marketCapService.Object);

            var cryptoList = new List<CryptoCurrency>() {
                new CryptoCurrency() { CryptoCode = code }
            };

            // act
            var response = await serviceUnderTest.GetLastestQuoteListsAsync(cryptoList);

            //assert
            response.Should().HaveCount(1);
            response.Should().ContainSingle(e => e.CryptoCode == code);
            response[0].CurrencyQuotes.Should().HaveCount(1);
            response[0].CurrencyQuotes.Should().ContainSingle(e => e.Currency == currency);
            response[0].CurrencyQuotes.Should().ContainSingle(e => e.Price == price);
        }
    }
}
