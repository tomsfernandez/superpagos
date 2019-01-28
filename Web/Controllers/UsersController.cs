using System;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Web.Dto;
using Web.Model;
using Web.Model.Domain;

namespace Web.Controllers {
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class UsersController : ControllerBase {

        private AppDbContext Context { get; }
        private IMapper Mapper { get; }
        private PasswordEncrypter PasswordEncrypter { get; }
        private IConfiguration Config { get; }

        public UsersController(AppDbContext context, IMapper mapper, IConfiguration config) {
            Context = context;
            Mapper = mapper;
            Config = config;
            PasswordEncrypter = new PasswordEncrypter(Config["EncryptionSalt"]);
        }

        [HttpGet("")]
        public async Task<IActionResult> Get() {
            var users = await Context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) {
            var user = await Context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // todo: return User without password
        [HttpPost(""), AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] UserDto dto) {
            var errors = dto.Validate();
            if (Context.Users.Any(x => x.Email.Equals(dto.Email))) errors.Add("Email ya está siendo usado");
            if (dto.Email != null && !IsEmailValid(dto.Email)) errors.Add("Email no es válido");
            if (errors.Count > 0) return BadRequest(errors);
            var user = Mapper.Map<User>(dto);
            user.Password = PasswordEncrypter.Encrypt(dto.Password);
            await Context.Users.AddAsync(user);
            await Context.SaveChangesAsync();
            return Ok(user);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            var user = await Context.Users.FindAsync(id);
            if (user == null) return NotFound();
            Context.Users.Remove(user);
            Context.SaveChanges();
            return Ok();
        }

        private bool IsEmailValid(string email) {
            try {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException) {
                return false;
            }
        }
    }
}