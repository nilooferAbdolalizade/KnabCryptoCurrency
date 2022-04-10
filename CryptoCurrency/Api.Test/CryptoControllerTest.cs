using Application.Dto;
using Application.Interface;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Api.Test
{
    public class CryptoControllerTest
    {
        [Fact]
        public async Task CryptoController_ValidInput_GetValidResponse()
        {
            //arrange
            var currencyQuoteList = new List<CurrencyQuoteDto>() {
                new CurrencyQuoteDto()
                    {
                        Currency = "USD",
                        Price = 34568
                    },
                        new CurrencyQuoteDto()
                    {
                        Currency = "EUR",
                        Price = 25000
                    }
            };
            var dto = new LatestQuoteDto()
            {
                Status = new Status() { ErrorMessage = null },
                Data = new List<Data>() { new Data() {
                    Symbol = "BTC",
                    Quote = currencyQuoteList }
                }
            };

            var serviceUnderTest = new Mock<ICryptoService>();
            serviceUnderTest.Setup(e => e.GetLatestCryptoQuotesByCode(It.IsAny<string>()))
                .ReturnsAsync(dto);

            var controller = new CryptoController(serviceUnderTest.Object);

            //Act
            var input = "BTC";
            var response = await controller.GetLatestCryptoQuotesByCode("BTC");

            //Assert
            response.Status.ErrorMessage.Should().BeNull();
            var data = response.Data.Single(e => e.Symbol == input);
            data.Quote.Should().HaveCount(2);
            data.Quote.Should().ContainSingle(e => e.Currency == "USD");
            data.Quote.Should().ContainSingle(e => e.Price == 34568);
        }
    }
}
