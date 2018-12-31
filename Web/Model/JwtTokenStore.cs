using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Web.Model.KeyBuilder;

namespace Web.Model {
    public class JwtTokenStore {

        public JwtTokenStore() { }

        public string GiveToken(DateTime expirationDate, TokenKeyBuilder keyBuilder) {
            var securityKey = keyBuilder.BuildKey();
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Expires = expirationDate,
                NotBefore = DateTime.Now,
                IssuedAt = DateTime.Now,
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public List<string> IsTokenValid(string token, TokenKeyBuilder keyBuilder) {
            var key = keyBuilder.BuildKey();
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false
            };
            return TokenDecodeExceptionHandler.HandleJwtDecode(() => {
                handler.ValidateToken(token, validations, out var securityToken);
            });
        }
    }
}