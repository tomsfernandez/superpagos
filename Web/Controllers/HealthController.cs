using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers {
    
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase{

        [HttpGet]
        public IActionResult Get() {
            return Ok();
        }
    }
}