using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers {
   
    public class HomeController : Controller{

        [HttpGet("api/")]
        public IActionResult Get() {
            return Ok("Welcome to the Superpagos API");
        }

        [HttpGet("")]
        public IActionResult Index() {
            return View();
        }
    }
}