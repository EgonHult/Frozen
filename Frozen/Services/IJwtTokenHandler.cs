using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Frozen.Services
{
    interface IJwtTokenHandler
    {
        Task<IEnumerable<Claim>> GetJwtTokenClaimsAsync(string token);
        Task<bool> ValidateJwtTokenExpirationDateAsync(string token);
    }
}
