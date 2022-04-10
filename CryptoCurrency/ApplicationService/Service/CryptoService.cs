using Application.Dto;
using Application.Interface;
using ApplicationService.Helper;
using Domain.Exception;
using Domain.IExternalService;
using Domain.IRepository;
using Domain.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationService.Service
{
    public class CryptoService : ICryptoService
    {
        public readonly ICoinMarketCapService _coinMarketCapService;
        public readonly ICryptoCurrencyRepository _cryptoCurrencyRepository;


        public CryptoService(ICoinMarketCapService coinMarketCapService, ICryptoCurrencyRepository cryptoCurrencyRepository)
        {
            _coinMarketCapService = coinMarketCapService;
            _cryptoCurrencyRepository = cryptoCurrencyRepository;
        }

        public async Task<LatestQuoteDto> GetLatestCryptoQuotesByCode(string code)
        {
            #region Validate Input
            ValidateCryptoCode(code, out List<string> messages);
            if (messages.Any())
            {
                return GenerateInvalidResponse(code, messages);
            }
            #endregion

            var result = new LatestQuoteDto();

            var allCryptoCurrencies = await _cryptoCurrencyRepository.GetAllCryptoCurrenciesAsync();
            var oldCrypto = allCryptoCurrencies.FirstOrDefault(e => e.CryptoCode == code);
            if (oldCrypto == null)
            {
                var cryptoCurrency = await GetLatestCryptoQuotes(code, allCryptoCurrencies);

                //Add to Db
                await AddNewCryptoCurrency(code, cryptoCurrency);

                var dataList = cryptoCurrency.Select(e => e.ToDto()).ToList();
                result = GenerateValidResponse(dataList);
            }
            else
            {
                var errorMessage = new List<string>() { new CryptoCodeIsDuplicateException().GetTranslate() };
                result = GenerateInvalidResponse(code, errorMessage);
            }

            return result;
        }

        private static LatestQuoteDto GenerateInvalidResponse(string code, List<string> messages)
        {
            return new LatestQuoteDto()
            {
                Status = new Status()
                {
                    ErrorCode = 1,
                    ErrorMessage = messages
                },
                Data = new List<Data>() { new Data() { Symbol = code } }
            };
        }

        private static LatestQuoteDto GenerateValidResponse(List<Data> dataList)
        {
            return new LatestQuoteDto()
            {
                Data = dataList,
                Status = new Status()
                {
                    ErrorCode = 0,
                    ErrorMessage = null
                }
            };
        }

        private async Task<List<CryptoCurrency>> GetLatestCryptoQuotes(string code, List<CryptoCurrency> allCryptoCurrencies)
        {
            var newCrypto = new CryptoCurrency() { CryptoCode = code };
            allCryptoCurrencies.Add(newCrypto);

            var cryptoCurrency = await _coinMarketCapService.GetLastestQuoteListsAsync(allCryptoCurrencies);

            return cryptoCurrency;
        }

        private static void ValidateCryptoCode(string code, out List<string> messages)
        {
            messages = new List<string>();
            if (string.IsNullOrEmpty(code) || string.IsNullOrWhiteSpace(code))
            {
                messages.Add(new CryptoCodeLengthException().GetTranslate());
            }
            if (code.Trim().Length != 3)
            {
                messages.Add(new CryptoCodeLengthException().GetTranslate());
            }
            if (!code.All(char.IsUpper))
            {
                messages.Add(new CryptoCodeLetterException().GetTranslate());
            }
        }

        private async Task AddNewCryptoCurrency(string code, List<CryptoCurrency> cryptoCurrency)
        {
            var newCryptoCurrency = cryptoCurrency.Single(e => e.CryptoCode == code);
            await _cryptoCurrencyRepository.AddNewCryptoCurrencyAsync(newCryptoCurrency);
        }
    }
}
