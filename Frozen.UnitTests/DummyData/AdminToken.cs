using Frozen.Models;
using Frozen.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Users.Models;

namespace Frozen.UnitTests.DummyData
{
    public static class AdminToken
    {
        public static string LoginAdmin(IClientService client)
        {
            var loginModel = new LoginModel() { Username = "admin@frozen.se", Password = "Test123!" };

            var apiLocation = "https://localhost:44350/user/login/";
            var response = client.SendRequestToGatewayAsync(apiLocation, HttpMethod.Post, loginModel).Result;
            var json = response.Content.ReadAsStringAsync().Result;

            var user = JsonConvert.DeserializeObject<LoggedInUser>(json);
            return user.Token;
        }
    }
}
