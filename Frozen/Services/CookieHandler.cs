using Frozen.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <param name="content"></param>
        public async Task CreateLoginCookiesAsync(string token, string refreshToken)
        {
            await CreateLoggInCookieAsync(token);
            CreatePersitentCookie(CommonConfig.PersistantCookie.COOKIE_REFRESHTOKEN_NAME, refreshToken);
            CreateSessionCookie(CommonConfig.SessionCookie.COOKIE_NAME, token);
        }

        /// <summary>
        /// Creates an [Authorization] cookie
        /// </summary>
        /// <param name="content"></param>
        /// <param name="isPersistent"></param>
        public async Task CreateLoggInCookieAsync(string content, bool isPersistent = false)
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
            _accessor.HttpContext.Session.SetString("test", content);
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

    }
}
