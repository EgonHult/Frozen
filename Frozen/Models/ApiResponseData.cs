using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frozen.Models
{
    public class ApiResponseData
    {
        public HttpResponseMessage Response { get; set; }
        public object Content { get; set; }
    }
}
