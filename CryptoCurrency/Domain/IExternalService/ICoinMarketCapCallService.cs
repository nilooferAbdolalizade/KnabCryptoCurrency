using Domain.Entity;
using System.Threading.Tasks;

namespace Domain.IExternalService
{
    public interface ICoinMarketCapCallService
    {
        Task<ApiResponse> MakeLatestQuoteApiCall(string code, string currency);
    }
}
