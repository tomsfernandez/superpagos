using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Web.Dto;
using Web.Model;
using Web.Model.KeyBuilder;
using Web.Service.Email;

namespace Web.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordRecoveryController : ControllerBase{
        
        private AppDbContext Context { get; }
        private PasswordEncrypter PasswordEncrypter { get; }
        private IConfiguration Config { get; }
        private int MinutesToRecoverPassword { get; }
        private EmailSender EmailSender { get; }

        public PasswordRecoveryController(AppDbContext context, IConfiguration config, EmailSender sender) {
            Context = context;
            Config = config;
            PasswordEncrypter = new PasswordEncrypter(Config["EncryptionSalt"]);
            MinutesToRecoverPassword = 5;
            EmailSender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> SendResetToken([FromBody] string email) {
            var user = await Context.Users.SingleAsync(x => x.Email.Equals(email));
            if (user == null) return Ok();
            var keyBuilder = new PasswordRecoveryKeyBuilder(user);
            var token = BuildToken(DateTime.Now.AddMinutes(MinutesToRecoverPassword), keyBuilder);
            var urlCallback = $"/{user.Id}/{token}";
            await EmailSender.Send(email,"tomas.martinez@ing.austral.edu.ar","Superpagos - Recuperación de contraseña", urlCallback);
            return Ok(urlCallback);
        }

        // todo: manejar mejor excepciones de jwt al validar el token
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] RecoveryCredential credential) {
            var errors = credential.Validate();
            var user = await Context.Users.FindAsync(credential.Id);
            if (user == null) errors.Add("El usuario en el token no existe");
            if (errors.Any()) return BadRequest(errors);
            var keyBuilder = new PasswordRecoveryKeyBuilder(user);
            var jwtDecodeErrors = IsTokenValid(credential.Token, keyBuilder);
            if (jwtDecodeErrors.Count > 0) return BadRequest(jwtDecodeErrors);
            user.Password = PasswordEncrypter.Encrypt(credential.Password);
            Context.Users.Update(user);
            await Context.SaveChangesAsync();
            return Ok();
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

        public string BuildToken(DateTime expirationDate, TokenKeyBuilder builder) {
            var securityKey = builder.BuildKey();
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
    }
}