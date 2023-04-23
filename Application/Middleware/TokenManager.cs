using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class TokenManager
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;

        public TokenManager(IConfiguration configuration, IMemoryCache memoryCache)
        {
            _configuration = configuration;
            _memoryCache = memoryCache;
        }

        public string GenerateToken()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Santhosh"), // Set the user's name as a claim
                new Claim(ClaimTypes.Role, "Admin"), // Set the user's role as a claim
                // Add any other claims as needed
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, // Pass the list of claims
                "TokenAuth", // Set the authentication type
                ClaimTypes.Name, // Set the name claim type
                ClaimTypes.Role // Set the role claim type
            );

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = signingCredentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Store the token in cache with a sliding expiration of 1 hour
            _memoryCache.Set("JwtToken", tokenString, TimeSpan.FromHours(1));

            return tokenString;

        }

        public bool ValidateToken(string token)
        {
            // Retrieve the token from cache
            if (_memoryCache.TryGetValue("JwtToken", out string cachedToken))
            {
                // Compare the retrieved token with the provided token
                if (string.Equals(token, cachedToken))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

