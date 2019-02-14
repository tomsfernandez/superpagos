using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Web.Dto;
using Web.Model.Domain;
using Web.Model.JwtClaim;

namespace Web.Controllers {
    
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class PaymentButtonsController : AuthenticatedController{

        public AppDbContext Context { get; }

        public PaymentButtonsController(ClaimExtractorFactory factory, AppDbContext context) : base(factory) {
            Context = context;
        }

        [HttpGet]
        public async Task<IActionResult> All() {
            var userId = GetIdFromToken();
            var buttons = await Context.PaymentButtons.Where(x => x.Method.User.Id == userId)
                .Select(x => new {
                    x.Id,
                    x.Code,
                    x.Amount,
                    x.CreationDate,
                    x.Method.Provider.Name
                })
                .ToListAsync();
            return Ok(buttons);
        }

        [HttpPost("")]
        public IActionResult Post([FromBody] ButtonCreateInfo dto) {
            var errors = dto.IsValid();
            var paymentMethod = Context.PaymentMethods
                .Include(x => x.User)
                .Include(x => x.Provider)
                .SingleOrDefault(x => x.Id == dto.PaymentMethodId);
            if (paymentMethod == null) errors.Add($"No existe medio de pago con id {dto.PaymentMethodId}");
            if (paymentMethod != null && paymentMethod.User.Id != GetIdFromToken()) 
                errors.Add("El medio de pago no pertenece al usuario logueado");
            if (errors.Any()) return BadRequest(errors);
            var button = new PaymentButton {
                Code = dto.Code,
                CreationDate = DateTime.Now,
                Method = paymentMethod,
                Amount = dto.Amount
            };
            Context.PaymentButtons.Add(button);
            Context.SaveChanges();
            return Ok(new {
                button.Id,
                button.Code,
                button.Amount,
                button.CreationDate,
                button.Method.Provider.Name
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id) {
            var userId = GetIdFromToken();
            var button = Context.PaymentButtons
                .Include(x => x.Method)
                .ThenInclude(x => x.User)
                .SingleOrDefault(x => x.Id == id);
            if (button == null) return NotFound();
            if (button.Method.User.Id != userId)
                return BadRequest("El usuario no puede borar un botón que no es de él");
            Context.PaymentButtons.Remove(button);
            Context.SaveChanges();
            return Ok();

        }
    }
}