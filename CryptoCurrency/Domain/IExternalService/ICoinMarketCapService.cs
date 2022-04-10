using Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.IExternalService
{
    public interface ICoinMarketCapService
    {
        Task<List<CryptoCurrency>> GetLastestQuoteListsAsync(List<CryptoCurrency> cryptoCurrencies);
    }
}
