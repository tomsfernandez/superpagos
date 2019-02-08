using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Web.Dto;
using Web.Model;
using Web.Model.Domain;
using Web.Model.JwtClaim;

namespace Web.Controllers {
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class UsersController : AuthenticatedController {

        private AppDbContext Context { get; }
        private IMapper Mapper { get; }
        private PasswordEncrypter PasswordEncrypter { get; }
        private IConfiguration Config { get; }

        public UsersController(AppDbContext context, IMapper mapper, 
            IConfiguration config, ClaimExtractorFactory extractorFactory) : base(extractorFactory) {
            Context = context;
            Mapper = mapper;
            Config = config;
            PasswordEncrypter = new PasswordEncrypter(Config["EncryptionSalt"]);
        }

        [HttpGet("")]
        public async Task<IActionResult> Get() {
            var users = await Context.Users.ToListAsync();
            var usersWithoutPassword = users.Select(x => Mapper.Map<UserDtoWithoutPassword>(x)).ToList();
            return Ok(usersWithoutPassword);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) {
            var userId = GetIdFromToken();
            if (id != userId) return Unauthorized();
            var user = await Context.Users.FindAsync(id);
            if (user == null) return NotFound();
            var userWithoutPassword = Mapper.Map<UserDtoWithoutPassword>(user);
            return Ok(userWithoutPassword);
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
            var userWithoutPassword = Mapper.Map<UserDtoWithoutPassword>(user);
            return Ok(userWithoutPassword);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id) {
            var userId = GetIdFromToken();
            if (id != userId) return Unauthorized();
            var user = await Context.Users.FindAsync(id);
            if (user == null) return NotFound();
            Context.Users.Remove(user);
            Context.SaveChanges();
            return Ok();
        }

        [HttpDelete("")]
        public async Task<IActionResult> Delete() {
            var userId = GetIdFromToken();
            var user = await Context.Users.FindAsync(userId);
            Context.Users.Remove(user);
            Context.SaveChanges();
            return Ok();
        }

        private bool IsEmailValid(string email) {
            try {
                new MailAddress(email);
                return true;
            }
            catch (FormatException) {
                return false;
            }
        }
    }
}