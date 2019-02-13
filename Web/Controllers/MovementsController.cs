using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Model.JwtClaim;

namespace Web.Controllers {
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class MovementsController : AuthenticatedController {

        private AppDbContext Context { get; }

        public MovementsController(AppDbContext context, ClaimExtractorFactory extractorFactory) 
            : base(extractorFactory) {
            Context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> All() {
            var userId = GetIdFromToken();
            var movements = await Context.Movements
                .Include(x => x.Account.Provider)
                .Where(x => x.Account.User.Id == userId)
                .ToListAsync();
            return Ok(movements);
        }
    }
}