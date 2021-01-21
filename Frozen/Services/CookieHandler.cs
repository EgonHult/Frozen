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

            await CreateAuthenticationCookieAsync(token, rememberUser);
            CreatePersistentCookie(Cookies.JWT_REFRESH_TOKEN, refreshToken);
            CreateSessionCookie(Cookies.JWT_SESSION_TOKEN, token);
        }

        /// <summary>
        /// Creates a cookie for [Authorization] usage
        /// </summary>
        /// <param name="content"></param>
        /// <param name="isPersistent"></param>
        public async Task CreateAuthenticationCookieAsync(string content, bool isPersistent = false)
        {
            var jwtClaims = await _tokenHandler.GetJwtTokenClaimsAsync(content);

            var claimsIdentity = new ClaimsIdentity(jwtClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties()
            {
                IsPersistent = isPersistent,
                ExpiresUtc = DateTime.UtcNow.AddMonths(2)
            };

            await _accessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);
        }

        /// <summary>
        /// Validates JWT-token in session cookie
        /// </summary>
        /// <returns>bool</returns>
        public async Task<bool> ValidateJwtTokenSessionExpirationAsync()
        {
            var token = GetSessionCookieContent(Cookies.JWT_SESSION_TOKEN);

            if (token == null)
                return false;

            return await _tokenHandler.ValidateJwtTokenExpirationDateAsync(token);
        }

        /// <summary>
        /// Get a value from a specific claim in the authentication cookie
        /// </summary>
        /// <param name="claimName"></param>
        /// <returns>string</returns>
        public async Task<string> GetClaimFromAuthenticationCookieAsync(string claimName)
        {
            var userId = await Task.FromResult(_accessor.HttpContext.User.FindFirstValue(claimName));
            return userId;
        }

        /// <summary>
        /// Renew authentication Jwt-Token and Jwt refreshtoken
        /// </summary>
        /// <param name="model"></param>
        public void RenewJwtTokens(TokenModel model)
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

            options.HttpOnly = true;
            options.Secure = true;
            options.SameSite = SameSiteMode.Strict;
            options.Expires = DateTime.UtcNow.AddMonths(2);

            _accessor.HttpContext.Response.Cookies.Append(name, content, options);
        }

        /// <summary>
        /// Get all content from selected persistent cookie
        /// </summary>
        /// <param name="name">Name of cookie</param>
        /// <returns>string</returns>
        public string GetPersistentCookieContent(string name)
        {
            return _accessor.HttpContext.Request.Cookies[name];
        }

        /// <summary>
        /// Create session cookie and store data
        /// </summary>
        /// <param name="name">Name of cookie</param>
        /// <param name="content">String to store</param>
        public void CreateSessionCookie(string name, string content)
        {
            _accessor.HttpContext.Session.SetString(name, content);
        }

        /// <summary>
        /// Read content from selected session cookie
        /// </summary>
        /// <param name="name">Name of cookie</param>
        /// <returns>string</returns>
        public string GetSessionCookieContent(string name)
        {
            return _accessor.HttpContext.Session.GetString(name);
        }

        /// <summary>
        /// Destory cookies and session cookies!
        /// </summary>
        public void DestroyAllCookies()
        {
            _accessor.HttpContext.Session.Remove(Cookies.JWT_SESSION_TOKEN);
            _accessor.HttpContext.Session.Remove(Cookies.CART_SESSION_COOKIE);
            _accessor.HttpContext.Response.Cookies.Delete(Cookies.JWT_REFRESH_TOKEN);
            _accessor.HttpContext.SignOutAsync();
        }
    }
}
