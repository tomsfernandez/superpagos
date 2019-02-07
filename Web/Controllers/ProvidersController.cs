using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Web.Dto;
using Web.Model;
using Web.Model.Domain;

namespace Web.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Admin")]
    public class ProvidersController : ControllerBase{

        private AppDbContext Context { get; }
        private IMapper Mapper { get; }

        public ProvidersController(AppDbContext context, IMapper mapper) {
            Context = context;
            Mapper = mapper;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get() {
            var providers = await Context.Providers.ToListAsync();
            return Ok(providers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) {
            var provider = await Context.Providers.FindAsync(id);
            if (provider == null) return NotFound();
            return Ok(provider);
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody] ProviderDto dto) {
            var errors = dto.Validate();
            if (Context.Providers.Any(x => x.Code.Equals(dto.Code))) errors.Add("Code ya está en uso");
            if (errors.Count > 0) return BadRequest(errors);
            var provider = Mapper.Map<Provider>(dto);
            await Context.Providers.AddAsync(provider);
            await Context.SaveChangesAsync();
            return Ok(provider);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id) {
            var provider = await Context.Providers.FindAsync(id);
            if (provider == null) return NotFound();
            var methodsOfProvider = await Context.PaymentMethods.Where(x => x.Provider.Id == provider.Id).ToListAsync();
            if (methodsOfProvider.Any())
                return BadRequest(new List<string> {"Borrar los métodos de pago de este proveedor antes"});
            Context.Providers.Remove(provider);
            await Context.SaveChangesAsync();
            return Ok("OK");
        }
    }
}