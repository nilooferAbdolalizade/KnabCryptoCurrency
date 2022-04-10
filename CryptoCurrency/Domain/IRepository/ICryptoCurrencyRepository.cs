using Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.IRepository
{
    public interface ICryptoCurrencyRepository
    {
        Task<List<CryptoCurrency>> GetAllCryptoCurrenciesAsync();
        //Task<CryptoCurrency> GetCryptoCurrencyByCodeAsync(string code);
        Task<CryptoCurrency> AddNewCryptoCurrencyAsync(CryptoCurrency cryptoCurrency);
        //Task<CryptoCurrency> UpdateCryptoCurrencyAsync(CryptoCurrency cryptoCurrency);
    }
}
