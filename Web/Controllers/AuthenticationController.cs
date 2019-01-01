using System;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Web.Dto;
using Web.Model;
using Web.Model.KeyBuilder;

namespace Web.Controllers {
    [Route("api/[controller]")]
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
        [HttpPost("")]
        public IActionResult Login([FromBody] LoginCredentials credentials) {
            var encryptedPassword = PasswordEncrypter.Encrypt(credentials.Password);
            var user = Context.Users.SingleOrDefault(x => x.Email.Equals(credentials.Email) 
                                                 && x.Password.Equals(encryptedPassword));
            if (user == null) return Unauthorized();
            var longLivedToken = TokenStore.GiveToken(DateTime.Now.AddSeconds(LongLivedTokenTimeInSeconds), SecurityKeyBuilder);
            var shortLivedToken = TokenStore.GiveToken(DateTime.Now.AddSeconds(ShortLivedTokenTimeInSeconds), SecurityKeyBuilder);
            return Ok(new LoginTokens{LongLivedToken = longLivedToken, ShortLivedToken = shortLivedToken});
        }

        [HttpPost("renew")]
        [Authorize]
        public IActionResult RenewToken() {
            var newToken = TokenStore.GiveToken(DateTime.Now.AddSeconds(ShortLivedTokenTimeInSeconds), SecurityKeyBuilder);
            return Ok(new {Token = newToken});
        }
    }
}