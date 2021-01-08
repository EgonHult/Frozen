﻿using Frozen.Models;
using System.Threading.Tasks;

namespace Frozen.Services
{
    public interface ICookieHandler
    {
        Task CreateLoginCookiesAsync(LoggedInUser user, bool rememberUser);
        Task CreateAuthCookieAsync(string content, bool isPersistent = false);
        void CreatePersistentCookie(string name, string content);
        string ReadPersistentCookie(string name);
        void CreateSessionCookie(string name, string content);
        string ReadSessionCookieContent(string name);
        void DestroyAllCookies();
        Task<bool> ValidateJwtTokenAsync();
        Task<string> GetClaimFromIdentityCookieAsync(string claimName);
        void RenewAuthTokens(TokenModel model);
    }
}
