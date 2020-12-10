using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Frozen.Services
{
    interface IJwtTokenHandler
    {
        Task<IEnumerable<Claim>> GetClaimsAsync(string token);
    }
}
