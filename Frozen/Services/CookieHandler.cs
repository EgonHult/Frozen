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
        /// - Cookie for authorizing logged in users
        /// - Cookie for storing RefreshToken.
        /// - Session cookie for storing token.
        /// </summary>
        /// <param name="user"></param>
        public async Task CreateLoginCookiesAsync(LoggedInUser user, bool rememberUser)
        {
            var token = user.Token;
            var refreshToken = user.RefreshToken;

            await CreateAuthCookieAsync(token, rememberUser);
            CreatePersistentCookie(Cookies.JWT_REFRESH_TOKEN, refreshToken);
            CreateSessionCookie(Cookies.JWT_SESSION_TOKEN, token);
        }

        /// <summary>
        /// Creates an [Authorization] cookie
        /// </summary>
        /// <param name="content"></param>
        /// <param name="isPersistent"></param>
        public async Task CreateAuthCookieAsync(string content, bool isPersistent = false)
        {
            var claims = await _tokenHandler.GetJwtClaimsAsync(content);

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
            CreatePersistentCookie(Cookies.JWT_REFRESH_TOKEN, model.RefreshToken);
            CreateSessionCookie(Cookies.JWT_SESSION_TOKEN, model.Token);
        }

        /// <summary>
        /// Creates a new persistent cookie
        /// </summary>
        /// <param name="name">Name of cookie</param>
        /// <param name="content">string content</param>
        public void CreatePersistentCookie(string name, string content)
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
        public string ReadPersistentCookie(string name)
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
        public string ReadSessionCookieContent(string name)
        {
            return _accessor.HttpContext.Session.GetString(name);
        }

        /// <summary>
        /// Destory cookies and session cookies!
        /// </summary>
        public void DestroyAllCookies()
        {
            _accessor.HttpContext.Session.Remove(Cookies.JWT_SESSION_TOKEN);
            _accessor.HttpContext.Response.Cookies.Delete(Cookies.JWT_REFRESH_TOKEN);
            _accessor.HttpContext.SignOutAsync();
        }
    }
}
