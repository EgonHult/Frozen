using System.Net.Http;
using System.Threading.Tasks;

namespace Frozen.Services
{
    public interface IClientService
    {
        Task<HttpResponseMessage> SendRequestToGatewayAsync(string api, HttpMethod method, object obj = null);
        Task<T> ReadResponseAsync<T>(HttpContent responseContent);
        Task ValidateJwtTokenStatusAsync();
    }
}
