using System.Collections.Generic;

namespace Application.Dto
{
    public class LatestQuoteDto
    {
        public Status Status { get; set; }
        public List<Data> Data { get; set; }
    }

    public class Data
    {
        public string Symbol { get; set; }
        public List<CurrencyQuoteDto> Quote { get; set; }
    }

    public class CurrencyQuoteDto
    {
        public string Currency { get; set; }
        public decimal Price { get; set; }
    }

    public class Status
    {
        public int ErrorCode { get; set; }
        public List<string> ErrorMessage { get; set; }
    }
}
