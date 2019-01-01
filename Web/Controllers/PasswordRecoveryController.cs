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
        private JwtTokenStore TokenStore { get; }

        public PasswordRecoveryController(AppDbContext context, IConfiguration config, EmailSender sender) {
            Context = context;
            Config = config;
            PasswordEncrypter = new PasswordEncrypter(Config["EncryptionSalt"]);
            MinutesToRecoverPassword = 5;
            EmailSender = sender;
            TokenStore = new JwtTokenStore();
        }

        [HttpPost("")]
        public async Task<IActionResult> SendResetToken([FromBody] ResetTokenDto dto) {
            var user = Context.Users.SingleOrDefault(x => x.Email.Equals(dto.Email));
            if (user == null) return Ok();
            var keyBuilder = new PasswordRecoveryKeyBuilder(user);
            var token = TokenStore.GiveToken(DateTime.Now.AddMinutes(MinutesToRecoverPassword), keyBuilder);
            var urlCallback = $"/{user.Id}/{token}";
            await EmailSender.Send(dto.Email,"tomas.martinez@ing.austral.edu.ar",
                "Superpagos - Recuperación de contraseña", urlCallback);
            return Ok(urlCallback);
        }

        // todo: manejar mejor excepciones de jwt al validar el token
        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword([FromBody] RecoveryCredential credential) {
            var errors = credential.Validate();
            var user = await Context.Users.FindAsync(credential.Id);
            if (user == null) errors.Add("El usuario en el token no existe");
            if (errors.Any()) return BadRequest(errors);
            var keyBuilder = new PasswordRecoveryKeyBuilder(user);
            var jwtDecodeErrors = TokenStore.IsTokenValid(credential.Token, keyBuilder);
            if (jwtDecodeErrors.Count > 0) return BadRequest(jwtDecodeErrors);
            user.Password = PasswordEncrypter.Encrypt(credential.Password);
            Context.Users.Update(user);
            await Context.SaveChangesAsync();
            return Ok();
        }
    }
}