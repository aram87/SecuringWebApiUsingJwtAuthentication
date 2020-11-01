using Microsoft.IdentityModel.Tokens;
using SecuringWebApiUsingJwtAuthentication.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace SecuringWebApiUsingJwtAuthentication.Helpers
{
    public class TokenHelper
    {
        public const string Issuer = "http://codingsonata.com";
        public const string Audience = "http://codingsonata.com";
      
        public const string Secret = "OFRC1j9aaR2BvADxNWlG2pmuD392UfQBZZLM1fuzDEzDlEpSsn+btrpJKd3FfY855OMA9oK4Mc8y48eYUrVUSw==";
      
        //Important note***************
        //The secret is a base64-encoded string, always make sure to use a secure long string so no one can guess it. ever!.
        //a very recommended approach to use is through the HMACSHA256() class, to generate such a secure secret, you can refer to the below function
        // you can run a small test by calling the GenerateSecureSecret() function to generate a random secure secret once, grab it, and use it as the secret above 
        // or you can save it into appsettings.json file and then load it from them, the choice is yours

        public static string GenerateSecureSecret()
        {
            var hmac = new HMACSHA256();
            return Convert.ToBase64String(hmac.Key);
        }

        public static string GenerateToken(Customer customer)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key =  Convert.FromBase64String(Secret);

            var claimsIdentity = new ClaimsIdentity(new[] { 
                new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()),
                new Claim("IsBlocked", customer.Blocked.ToString())
            });
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Issuer = Issuer,
                Audience = Audience,
                Expires = DateTime.Now.AddMinutes(15),
                SigningCredentials = signingCredentials,
                
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
