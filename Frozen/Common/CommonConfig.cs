using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frozen.Common
{
    public static class CommonConfig
    {
        public class PersistantCookie
        {
            public const string COOKIE_NAME = "Frozen_LoginCookie";
            public const string COOKIE_REFRESHTOKEN_NAME = "Frozen_RefreshCookie";
        }

        public class SessionCookie
        {
            public const string COOKIE_NAME = "Frozen_SessionCookie";
        }
    }
}
