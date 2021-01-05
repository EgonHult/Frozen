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
        private readonly ICookieHandler _cookieHandler;
        private const string TOKEN_SCHEME  = "Bearer";
        private const string MEDIA_TYPE_JSON = "application/json";

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
        public async Task<HttpResponseMessage> SendRequestToGatewayAsync(string api, HttpMethod method, object obj = null, string jwtToken = null)
        {
            await VerifyTokenValidationStatusAsync();

            return await SendHttpRequestAsync(api, method, obj, jwtToken);
        }

        private async Task<HttpResponseMessage> SendHttpRequestAsync(string apiLocation, HttpMethod httpMethod, object obj = null, string jwtToken = null)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(obj);

                var request = new HttpRequestMessage(httpMethod, apiLocation);

                request = AddAuthorizationHeader(request, jwtToken);
                request.Content = new StringContent(json, Encoding.UTF8, MEDIA_TYPE_JSON);

                return await client.SendAsync(request);
            }
        }

        private HttpRequestMessage AddAuthorizationHeader(HttpRequestMessage requestMessage, string jwtToken)
        {
            if (jwtToken == null)
                return requestMessage;

            requestMessage.Headers.Authorization = new AuthenticationHeaderValue(TOKEN_SCHEME, jwtToken);
            return requestMessage;
        }

        /// <summary>
        /// Reads response from API
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseContent"></param>
        public async Task<T> ReadResponseAsync<T>(HttpContent responseContent)
        {
            var content = await responseContent.ReadAsStringAsync();
            var test = JsonConvert.DeserializeObject<T>(content);
            return test;
        }

        /// <summary>
        /// Check if current JWT-token is valid. If not, a new JWT-token request will be sent
        /// </summary>
        public async Task VerifyTokenValidationStatusAsync()
        {
            var isTokenValid = await _cookieHandler.ValidateJwtTokenAsync();
            var refreshToken = _cookieHandler.ReadPersistentCookie(Cookies.JWT_REFRESH_TOKEN);
            var loggedInUserId = await _cookieHandler.GetClaimFromIdentityCookieAsync("UserId");

            if (!isTokenValid && refreshToken != null && loggedInUserId != null)
            {
                await SendRequestForNewTokenAndRefreshToken(Guid.Parse(loggedInUserId));
            }
        }

        private async Task SendRequestForNewTokenAndRefreshToken(Guid userId)
        {
            var renewTokenModel = new RenewTokenModel()
            {
                UserID = userId,
                Token = _cookieHandler.ReadPersistentCookie(Cookies.JWT_REFRESH_TOKEN)
            };

            var result = await SendHttpRequestAsync(ApiLocation.Users.REQUEST_NEW_TOKEN_ENDPOINT, HttpMethod.Post, renewTokenModel);

            if (result.IsSuccessStatusCode)
            {
                var tokenPayload = await ReadResponseAsync<TokenModel>(result.Content);
                _cookieHandler.RenewAuthTokens(tokenPayload);
            }
        }

    }
}
