using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class WebHookController : ControllerBase{
        
        public WebHookController() { }

        [HttpPost("/notified")]
        public async Task<IActionResult> Notified() {
            // todo: check pending transactions
            // todo: if transactions so far are okay, update transacction and do nothing
            // todo: if all transactions are okay, notify with socket
            // todo: if error, start compensation process por all transactions
            // todo: sin importar el resultado, si hay una transacción de error y el proceso de compensado no fue iniciado, iniciarlo
            return Ok();
        }
    }
}