using ApplicationService.Service;
using Domain.IExternalService;
using Domain.IRepository;
using Domain.Model;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ApplicationService.Test
{
    public class CryptoServiceTest
    {
        [Theory]
        [InlineData("BT")]
        [InlineData("")]
        [InlineData("bitcoin")]
        [InlineData("BTCC")]
        [InlineData("1BitCoin")]
        public async Task CryptoService_InputLengthOrNullValidation_NotValid(string code)
        {
            //arrange 
            var coinMarketCapService = new Mock<ICoinMarketCapService>();
            var cryptoCurrencyRepository = new Mock<ICryptoCurrencyRepository>();
            var serviceUnderTest = new CryptoService(coinMarketCapService.Object, cryptoCurrencyRepository.Object);

            //act
            var response = await serviceUnderTest.GetLatestCryptoQuotesByCode(code);

            //Assert
            response.Status.ErrorMessage.Should().NotBeEmpty();
            response.Status.ErrorMessage[0].Equals("The 'code' must be three letters long");
        }

        [Theory]
        [InlineData("BtC")]
        [InlineData("BTc")]
        [InlineData("bTC")]
        [InlineData("btc")]
        [InlineData("BT1")]
        [InlineData("123")]
        public async Task CryptoService_InputLetterValidation_NotValid(string code)
        {
            //arrange 
            var coinMarketCapService = new Mock<ICoinMarketCapService>();
            var cryptoCurrencyRepository = new Mock<ICryptoCurrencyRepository>();
            var serviceUnderTest = new CryptoService(coinMarketCapService.Object, cryptoCurrencyRepository.Object);

            //act
            var response = await serviceUnderTest.GetLatestCryptoQuotesByCode(code);

            //Assert
            response.Status.ErrorMessage.Should().NotBeEmpty();
            response.Status.ErrorMessage[0].Equals("All letters of the 'code' must be uppercase");
        }

        [Theory]
        [InlineData("BTC")]
        [InlineData("ETH")]
        [InlineData("ADG")]
        public async Task CryptoService_InputDuplicateValidation_NotValid(string code)
        {
            //Arrange 
            var coinMarketCapService = new Mock<ICoinMarketCapService>();
            var cryptoCurrencyRepository = new Mock<ICryptoCurrencyRepository>();
            var cryptoList = new List<CryptoCurrency>() {
                new CryptoCurrency()
                {
                    CryptoCode = code
                }
            };

            cryptoCurrencyRepository.Setup(e => e.GetAllCryptoCurrenciesAsync())
               .ReturnsAsync(cryptoList);

            var serviceUnderTest = new CryptoService(coinMarketCapService.Object, cryptoCurrencyRepository.Object);

            //Act
            var response = await serviceUnderTest.GetLatestCryptoQuotesByCode(code);

            //Assert
            response.Status.ErrorMessage.Should().NotBeEmpty();
            response.Status.ErrorMessage.Should().HaveCount(1);
            response.Status.ErrorMessage[0].Equals("The 'code' exists");
        }


        [Theory]
        [InlineData("BTC")]
        [InlineData("ETH")]
        [InlineData("ADG")]
        public async Task CryptoService_InputValidation_IsValid(string code)
        {
            //Arrange 
            var coinMarketCapService = new Mock<ICoinMarketCapService>();
            var cryptoCurrencyRepository = new Mock<ICryptoCurrencyRepository>();

            var newCryptoList = new List<CryptoCurrency>() {
                new CryptoCurrency()
                {
                    CryptoCode = code,
                    CurrencyQuotes = new List<CurrencyQuote>() { new CurrencyQuote()
                    {
                        Currency = "USD",
                        Price = 446458
                    } }
                }
            };
            cryptoCurrencyRepository.Setup(e => e.GetAllCryptoCurrenciesAsync())
               .ReturnsAsync(new List<CryptoCurrency>());
            cryptoCurrencyRepository.Setup(e => e.AddNewCryptoCurrencyAsync(It.IsAny<CryptoCurrency>()))
                .ReturnsAsync(new CryptoCurrency());
            coinMarketCapService
                .Setup(e => e.GetLastestQuoteListsAsync(It.IsAny<List<CryptoCurrency>>()))
                .ReturnsAsync(newCryptoList);

            var serviceUnderTest = new CryptoService(coinMarketCapService.Object, cryptoCurrencyRepository.Object);

            //Act
            var response = await serviceUnderTest.GetLatestCryptoQuotesByCode(code);

            //Assert
            response.Data.Should().HaveCount(1);
            response.Status.ErrorCode.Equals(0);
        }

        [Fact]
        public async Task CryptoService_OneCodeInDb_GetTwoCryptoQuoteList()
        {
            //Arrange 
            var coinMarketCapService = new Mock<ICoinMarketCapService>();
            var cryptoCurrencyRepository = new Mock<ICryptoCurrencyRepository>();
            var oldCryptoList = new List<CryptoCurrency>() {
                new CryptoCurrency()
                {
                    CryptoCode = "BTC",
                    CurrencyQuotes = new List<CurrencyQuote>() { new CurrencyQuote()
                    {
                        Currency = "USD",
                        Price = 206458
                    } }
                }
            };
            var newCryptoList = new List<CryptoCurrency>() {
                new CryptoCurrency()
                {
                    CryptoCode = "ETH",
                    CurrencyQuotes = new List<CurrencyQuote>() { new CurrencyQuote()
                    {
                        Currency = "USD",
                        Price = 446458
                    } }
                }, oldCryptoList[0]
            };
            cryptoCurrencyRepository.Setup(e => e.GetAllCryptoCurrenciesAsync())
               .ReturnsAsync(oldCryptoList);
            cryptoCurrencyRepository.Setup(e => e.AddNewCryptoCurrencyAsync(It.IsAny<CryptoCurrency>()))
                .ReturnsAsync(new CryptoCurrency());
            coinMarketCapService
                .Setup(e => e.GetLastestQuoteListsAsync(It.IsAny<List<CryptoCurrency>>()))
                .ReturnsAsync(newCryptoList);

            var serviceUnderTest = new CryptoService(coinMarketCapService.Object, cryptoCurrencyRepository.Object);

            //Act
            var response = await serviceUnderTest.GetLatestCryptoQuotesByCode("ETH");

            //Assert
            response.Data.Should().HaveCount(2);
            response.Status.ErrorCode.Equals(0);
        }
    }
}
