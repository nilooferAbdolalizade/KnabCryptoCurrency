namespace Domain.Model
{
    public class CurrencyQuote
    {
        public string Currency { get; set; }
        public decimal Price { get; set; }
        public string ErrorMessage { get; set; }
    }
}
