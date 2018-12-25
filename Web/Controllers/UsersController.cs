using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Dto;
using Web.Model.Domain;

namespace Web.Controllers {
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class UsersController : ControllerBase {

        private AppDbContext Context { get; }
        private IMapper Mapper { get; }

        public UsersController(AppDbContext context, IMapper mapper) {
            Context = context;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get() {
            var users = await Context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) {
            var user = await Context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return Ok();
        }

        // todo: add password encryption
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] UserDto dto) {
            var errors = dto.Validate();
            if (Context.Users.Any(x => x.Email.Equals(dto.Email))) errors.Add("Email ya está siendo usado");
            if (!IsEmailValid(dto.Email)) errors.Add("Email no es válido");
            if (errors.Count > 0) return BadRequest(errors);
            var user = Mapper.Map<User>(dto);
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
            /*var regex = new Regex(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$",
                RegexOptions.Compiled);
            return regex.IsMatch(email);*/
            return true;
        }
    }
}