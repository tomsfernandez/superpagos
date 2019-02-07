using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Web.Dto;
using Web.Model;
using Web.Model.Domain;
using Web.Model.KeyBuilder;

namespace Web.Controllers {
    [Route("api")]
    [ApiController]
    public class AuthenticationController : ControllerBase{

        private AppDbContext Context { get; }
        private IConfiguration Config { get; }
        private string SecurityKey { get; }
        private long LongLivedTokenTimeInSeconds { get; }
        private long ShortLivedTokenTimeInSeconds { get; }
        private TokenKeyBuilder SecurityKeyBuilder { get; }
        private PasswordEncrypter PasswordEncrypter { get; }
        private JwtTokenStore TokenStore { get; }

        public AuthenticationController(AppDbContext context, IConfiguration config) {
            Context = context;
            Config = config;
            SecurityKey = Config["AuthenticationKey"] ?? "The little brown fox jumps over the lazy dog";
            LongLivedTokenTimeInSeconds = 60 * 60 * 24 * 5;
            ShortLivedTokenTimeInSeconds = 60 * 60 * 1;
            SecurityKeyBuilder = new SimpleKeyBuilder(SecurityKey);
            PasswordEncrypter = new PasswordEncrypter(Config["EncryptionSalt"]);
            TokenStore = new JwtTokenStore();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginCredentials credentials) {
            var errors = credentials.Validate();
            if (errors.Any()) return BadRequest(errors);
            var encryptedPassword = PasswordEncrypter.Encrypt(credentials.Password);
            var user = Context.Users.SingleOrDefault(x => x.Email.Equals(credentials.Email) 
                                                 && x.Password.Equals(encryptedPassword));
            if (user == null) return Unauthorized();
            var longLivedToken = TokenStore.GiveToken(DateTime.Now.AddSeconds(LongLivedTokenTimeInSeconds), SecurityKeyBuilder, GetClaims(user));
            var shortLivedToken = TokenStore.GiveToken(DateTime.Now.AddSeconds(ShortLivedTokenTimeInSeconds), SecurityKeyBuilder, GetClaims(user));
            return Ok(new LoginResponse {
                LongLivedToken = longLivedToken, 
                ShortLivedToken = shortLivedToken,
                IsAdmin = user.Role.Equals(Role.ADMIN)
            });
        }

        [HttpPost("renew")]
        [Authorize]
        public IActionResult RenewToken() {
            var userId = User.Claims.First(x => x.Type.Equals(ClaimTypes.Name)).Value;
            var user = Context.Users.Find(userId);
            if (user == null) return BadRequest("El token no corresponde a ningún usuario");
            var newToken = TokenStore.GiveToken(DateTime.Now.AddSeconds(ShortLivedTokenTimeInSeconds), 
                SecurityKeyBuilder, GetClaims(user));
            return Ok(new {Token = newToken});
        }

        private List<Claim> GetClaims(User user) {
            return new List<Claim> {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
        }
    }
}