using FluentAssertions;
using Infrastructure.ExternalService;
using Infrastructure.ExternalService.Entity;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Infrastructure.Test
{
    public class MarketCapCallServiceTest
    {
        [Fact]
        public async Task MarketCallService_ExceptionRaisedWhileCallingApi_ResponseHasError()
        {
            //Arrange
            var config = new CoinMarketCapConfig();
            var serviceUnderTest = new CoinMarketCapCallService(config);

            //Act
            var response = await serviceUnderTest.MakeLatestQuoteApiCall("BTC", "USD");

            //Assert
            response.HasError.Should().BeTrue();
            response.ErrorMessage.Should().NotBeEmpty();
            response.ErrorMessage.Should().Contain("Invalid URI: The hostname could not be parsed.");
        }
    }
}
