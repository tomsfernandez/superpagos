using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Model.Domain;

namespace Web.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase {

        private AppDbContext Context { get; }

        public PaymentMethodsController(AppDbContext context) {
            Context = context;
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user) {
            await Context.Users.AddAsync(user);
            await Context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] User user) {
            var userExists = await Context.Users.AnyAsync(x => x.Id.Equals(id));
            if (!userExists) return NotFound();
            Context.Update(user);
            Context.SaveChanges();
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
    }
}