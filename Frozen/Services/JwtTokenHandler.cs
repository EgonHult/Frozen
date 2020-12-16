using Microsoft.IdentityModel.Tokens;
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
        /// <summary>
        /// Extract a specific Claim from JWT token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Claim>> GetClaimsAsync(string token)
        {
            var extractedClaims = await ExtractClaimsFromJwtTokenAsync(token);
            return extractedClaims.Claims;
        }

        /// <summary>
        /// Validate JWT token expiration date
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> ValidateJwtTokenExpirationDateAsync(string token)
        {
            var claims = await GetClaimsAsync(token);
            var expireDate = claims.FirstOrDefault(x => x.Type == ClaimTypes.Expiration).Value;

            var now = DateTime.UtcNow;
            var expire = DateTime.Parse(expireDate).AddMinutes(-1);

            return (now < expire);
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
