using Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface ICryptoService
    {
        Task<LatestQuoteDto> GetLatestCryptoQuotesByCode(string code);
    }
}
