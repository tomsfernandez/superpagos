using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

        [HttpGet("state/{id}")]
        public IActionResult GetTransactionState(long id) {
            var userId = GetIdFromToken();
            var transaction = Context.Transactions
                .Include(x => x.Movements)
                    .ThenInclude(x => x.Account)
                        .ThenInclude(x => x.User)
                .SingleOrDefault(x => x.Id == id);
            if (transaction == null) return BadRequest($"Transacción con id {id} no existe");
            if (transaction.Movements.Any(x => x.Account.User.Id == userId)) {
                return Ok(new {
                    Finished = !transaction.InProcess() && transaction.Started(),
                    Success = transaction.IsSuccesfull(),
                    Failed = transaction.IsRollback()
                });
            }
            return BadRequest("Solo alguna de las partes puede ver el estado de la transacción");
        }
    }
}