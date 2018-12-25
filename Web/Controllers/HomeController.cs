using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers {
    
    [Route("api/")]
    [ApiController]
    public class HomeController : ControllerBase{

        [HttpGet]
        public IActionResult Get() {
            return Ok("Welcome to the Superpagos API");
        }
    }
}