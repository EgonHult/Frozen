using Frozen.Common;
using Frozen.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Frozen.Services
{
    public class CookieHandler : ICookieHandler
    {
        private readonly IHttpContextAccessor _accessor;

        JwtTokenHandler _tokenHandler;

        public CookieHandler(IHttpContextAccessor accessor)
        {
            this._tokenHandler = new JwtTokenHandler();
            this._accessor = accessor;
        }

        /// <summary>
        /// Creates 3 cookies:
        /// One [Authorization] cookie for authorizing logged in users.
        /// One for storing RefreshToken.
        /// One session cookie for storing token.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task CreateLoginCookiesAsync(LoggedInUser user)
        {
            var token = user.Token;
            var refreshToken = user.RefreshToken;

            await CreateAuthCookieAsync(token);
            CreatePersitentCookie(Cookies.JWT_REFRESH_TOKEN, refreshToken);
            CreateSessionCookie(Cookies.JWT_SESSION_TOKEN, token);
        }

        /// <summary>
        /// Creates an [Authorization] cookie
        /// </summary>
        /// <param name="content"></param>
        /// <param name="isPersistent"></param>
        public async Task CreateAuthCookieAsync(string content, bool isPersistent = false)
        {
            var claims = await _tokenHandler.GetClaimsAsync(content);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties()
            {
                IsPersistent = isPersistent,
                ExpiresUtc = DateTime.UtcNow.AddMonths(2) // Expire in 2 months
            };

            await _accessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);
        }

        public async Task<bool> ValidateJwtTokenAsync()
        {
            var token = ReadSessionCookieContent(Cookies.JWT_SESSION_TOKEN);

            if (token == null)
                return false;

            return await _tokenHandler.ValidateJwtTokenExpirationDateAsync(token);
        }

        //public async Task<string> GetClaimFromAuthTokenAsync(string claimName)
        //{
        //    var token = ReadSessionCookieContent(CommonConfig.SessionCookie.COOKIE_NAME);
        //    var claims = await _tokenHandler.GetClaimsAsync(token);
        //    var userId = claims.FirstOrDefault(x => x.Type == claimName).Value;

        //    return userId;
        //}

        public async Task<string> GetClaimFromIdentityCookieAsync(string claimName)
        {
            var userId = await Task.FromResult(_accessor.HttpContext.User.FindFirstValue(claimName));
            return userId;
        }

        /// <summary>
        /// Renew authentication Jwt-Tokens
        /// </summary>
        /// <param name="model"></param>
        public void RenewAuthTokens(TokenModel model)
        {
            CreatePersitentCookie(Cookies.JWT_REFRESH_TOKEN, model.RefreshToken);
            CreateSessionCookie(Cookies.JWT_SESSION_TOKEN, model.Token);
        }

        /// <summary>
        /// Creates a new persistent cookie
        /// </summary>
        /// <param name="name">Name of cookie</param>
        /// <param name="content">string content</param>
        public void CreatePersitentCookie(string name, string content)
        {
            CookieOptions options = new CookieOptions();

            // Security parameters
            options.HttpOnly = true;
            options.Secure = true;
            options.SameSite = SameSiteMode.Strict;
            options.Expires = DateTime.UtcNow.AddMonths(2);

            _accessor.HttpContext.Response.Cookies.Append(name, content, options);
        }

        /// <summary>
        /// Read contents from a persistent cookie
        /// </summary>
        /// <param name="name">Name of cookie to read from</param>
        /// <returns>string</returns>
        public string ReadPersitentCookie(string name)
        {
            return _accessor.HttpContext.Request.Cookies[name];
        }

        /// <summary>
        /// Creates a session cookie
        /// </summary>
        /// <param name="name">Name of the cookie</param>
        /// <param name="content">String-value to store in the cookie</param>
        public void CreateSessionCookie(string name, string content)
        {
            _accessor.HttpContext.Session.SetString(name, content);
        }

        /// <summary>
        /// Read content from selected cookie
        /// </summary>
        /// <param name="name">Name of the cookie</param>
        /// <returns>string</returns>
        public string ReadSessionCookieContent(string name)
        {
            return _accessor.HttpContext.Session.GetString(name);
        }

        /// <summary>
        /// Destory all regular/session cookies!
        /// </summary>
        public void DestroyAllCookies()
        {
            // Remove JWT-token session cookie
            _accessor.HttpContext.Session.Remove(Cookies.JWT_SESSION_TOKEN);

            // Remove refreshtoken
            _accessor.HttpContext.Response.Headers.Remove(Cookies.JWT_REFRESH_TOKEN);

            // Remove Auth-cookie
            _accessor.HttpContext.SignOutAsync();
        }
    }
}
