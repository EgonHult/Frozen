using Frozen.Common;
using Frozen.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Frozen.Services
{
    public class ClientService : IClientService
    {
        private const string BEARER_TOKEN_TYPE = "Bearer";
        private const string ACCEPT_VALUE = "application/json";
        private readonly ICookieHandler _cookieHandler;

        public ClientService(ICookieHandler cookieHandler)
        {
            this._cookieHandler = cookieHandler;
        }

        /// <summary>
        /// Send request to Gateway API
        /// </summary>
        /// <param name="api"></param>
        /// <param name="method"></param>
        /// <param name="obj"></param>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> SendRequestToGatewayAsync(string api, HttpMethod method, object obj = null, string jwtToken = null)
        {
            // Before sending any request, check if token needs to be renewed!
            await CheckTokenStatusAsync();

            return await SendRequestAsync(api, method, obj, jwtToken);
        }

        private async Task<HttpResponseMessage> SendRequestAsync(string api, HttpMethod method, object obj = null, string jwtToken = null)
        {
            using (var client = new HttpClient())
            {
                var jsonData = JsonConvert.SerializeObject(obj);

                var request = new HttpRequestMessage(method, api);
                request = AddAuthorizationHeader(request, jwtToken);
                request.Content = AddContentToRequest(jsonData);

                return await client.SendAsync(request);
            }
        }
        
        private StringContent AddContentToRequest(string data)
        {
            return new StringContent(data, Encoding.UTF8, ACCEPT_VALUE);
        }

        private HttpRequestMessage AddAuthorizationHeader(HttpRequestMessage requestMessage, string authToken)
        {
            if (authToken == null)
                return requestMessage;

            requestMessage.Headers.Authorization = new AuthenticationHeaderValue(BEARER_TOKEN_TYPE, authToken);
            return requestMessage;
        }

        /// <summary>
        /// Reads response from API
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseContent"></param>
        /// <returns></returns>
        public async Task<T> ReadResponseAsync<T>(HttpContent responseContent)
        {
            var content = await responseContent.ReadAsStringAsync();
            var test = JsonConvert.DeserializeObject<T>(content);
            return test;
        }

        /// <summary>
        /// Check if current JWT-token is valid. If not, a new JWT-token request will be sent
        /// </summary>
        /// <returns></returns>
        public async Task CheckTokenStatusAsync()
        {
            var isTokenValid = await _cookieHandler.ValidateJwtTokenAsync();
            var refreshToken = _cookieHandler.ReadPersitentCookie(Cookies.JWT_REFRESH_TOKEN);
            var loggedInUserId = await _cookieHandler.GetClaimFromIdentityCookieAsync("UserId");

            if (!isTokenValid && refreshToken != null && loggedInUserId != null)
            {
                await RequestNewTokens(Guid.Parse(loggedInUserId));
            }
        }

        private async Task RequestNewTokens(Guid userId)
        {
            var refreshTokenModel = new RenewTokenModel()
            {
                UserID = userId,
                Token = _cookieHandler.ReadPersitentCookie(Cookies.JWT_REFRESH_TOKEN)
            };

            var result = await SendRequestAsync(ApiLocation.Users.REQUEST_NEW_TOKEN_ENDPOINT, HttpMethod.Post, refreshTokenModel);

            if (result.IsSuccessStatusCode)
            {
                var tokenPayload = await ReadResponseAsync<TokenModel>(result.Content);
                _cookieHandler.RenewAuthTokens(tokenPayload);
            }
        }

    }
}
