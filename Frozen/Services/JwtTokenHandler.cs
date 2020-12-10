using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Frozen.Services
{
    public class JwtTokenHandler : IJwtTokenHandler
    {
        public async Task<IEnumerable<Claim>> GetClaimsAsync(string token)
        {
            var extractedClaims = await ExtractClaimsFromJwtTokenAsync(token);
            return extractedClaims.Claims;
        }

        private async Task<JwtSecurityToken> ExtractClaimsFromJwtTokenAsync(string token)
        {
            if (String.IsNullOrEmpty(token) || String.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException("Token kan inte vara null eller tom!");

            var handler = new JwtSecurityTokenHandler();
            var tokenContent = handler.ReadJwtToken(token);

            return await Task.FromResult(tokenContent);
        }
    }
}
