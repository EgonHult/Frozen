using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frozen.Services
{
    public interface ICookieHandler
    {
        Task CreateLoginCookiesAsync(string token, string refreshToken);
        Task CreateLoggInCookieAsync(string content, bool isPersistent = false);
        void CreatePersitentCookie(string name, string content);
        string ReadPersitentCookie(string name);
        void CreateSessionCookie(string name, string content);
        string ReadSessionCookieContent(string name);
    }
}
