using Application.Dto;
using Domain.Model;
using System.Linq;

namespace ApplicationService.Helper
{
    public static class DtoExtention
    {
        public static Data ToDto(this CryptoCurrency e)
        {
            return new Data
            {
                Symbol = e.CryptoCode,
                Quote = e.CurrencyQuotes.Select(x => new CurrencyQuoteDto
                {
                    Currency = x.Currency,
                    Price = x.Price
                }).ToList()
            };
        }
    }
}
