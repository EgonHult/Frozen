using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frozen.UnitTests.Sessions
{
    public class HttpContextConfig
    {
        public IHttpContextAccessor HttpContext { get; set; }

        public HttpContextConfig()
        {
            Configuration();
        }

        private void Configuration()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddHttpContextAccessor();
            serviceCollection.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            serviceCollection.AddMvcCore().AddAuthorization();
            serviceCollection.AddSession();
            serviceCollection.AddLogging();

            HttpContext = new HttpContextAccessor();

            HttpContext.HttpContext = new DefaultHttpContext()
            {
                RequestServices = serviceCollection.BuildServiceProvider(),
                Session = new TestSession()
            };
        }
    }
}
