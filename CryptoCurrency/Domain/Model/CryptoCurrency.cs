using System.Collections.Generic;

namespace Domain.Model
{
    public class CryptoCurrency
    {
        public int Id { get; set; }
        public string CryptoCode { get; set; }

        public List<CurrencyQuote> CurrencyQuotes { get; set; }
    }
}
