using Domain.IRepository;
using Domain.Model;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class CryptoCurrencyRepository : ICryptoCurrencyRepository
    {
        private readonly CryptoCurrencyContext _context;

        public CryptoCurrencyRepository(CryptoCurrencyContext context)
        {
            _context = context;
        }

        public async Task<CryptoCurrency> AddNewCryptoCurrencyAsync(CryptoCurrency cryptoCurrency)
        {
            await _context.CryptoCurrencies.AddAsync(cryptoCurrency);
            await _context.SaveChangesAsync();
            return cryptoCurrency;
        }

        public async Task<List<CryptoCurrency>> GetAllCryptoCurrenciesAsync()
        {
            return await _context.CryptoCurrencies.ToListAsync();
        }

        //public async Task<CryptoCurrency> GetCryptoCurrencyByCodeAsync(string code)
        //{
        //    return await _context.CryptoCurrencies.FirstOrDefaultAsync(x => x.CryptoCode == code);
        //}
    }
}
