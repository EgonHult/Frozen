using Frozen.Models;
using System.Threading.Tasks;

namespace Frozen.Services
{
    public interface ICookieHandler
    {
        Task CreateLoginCookiesAsync(LoggedInUser user, bool rememberUser);
        Task CreateAuthenticationCookieAsync(string content, bool isPersistent = false);
        void CreatePersistentCookie(string name, string content);
        string GetPersistentCookieContent(string name);
        void CreateSessionCookie(string name, string content);
        string GetSessionCookieContent(string name);
        void DestroyAllCookies();
        Task<bool> ValidateJwtTokenSessionCookieAsync();
        Task<string> GetClaimFromAuthenticationCookieAsync(string claimName);
        void RenewJwtTokens(TokenModel model);
    }
}
