using Application.Dto;
using Application.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api
{
    [Controller]
    public class CryptoController : Controller
    {
        public readonly ICryptoService _cryptoService;
        public CryptoController(ICryptoService cryptoService)
        {
            _cryptoService = cryptoService;
        }

        [HttpPost]
        [Route("/GetLatestCryptoQuoteByCode")]
        public async Task<LatestQuoteDto> GetLatestCryptoQuotesByCode(string code)
        {
            return await _cryptoService.GetLatestCryptoQuotesByCode(code);
        }
    }
}
