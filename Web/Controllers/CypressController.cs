using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using static System.String;

namespace Web.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class CypressController : Controller {
        private AppDbContext Context { get; }
        private IConfiguration Configuration { get; }

        public CypressController(AppDbContext context, IConfiguration configuration) {
            Context = context;
            Configuration = configuration;
        }

        public override void OnActionExecuting(ActionExecutingContext context) {
            base.OnActionExecuting(context);
            
            var cypressToken = context.HttpContext.Request.Headers["CYPRESS_TOKEN"];
            var isForTesting = Configuration["IsForTesting"] != null && bool.Parse(Configuration["IsForTesting"]);
            if (!isForTesting || !TokenIsValid(cypressToken)) {
                context.Result = Unauthorized();
            }

            bool TokenIsValid(string token) {
                if (IsNullOrEmpty(token)) return false;
                var expectedToken = Configuration["CYPRESS_TOKEN"];
                return token.Equals(expectedToken);
            }
        }

        [HttpGet("health")]
        public IActionResult Health() {
            return Ok();
        }

        [HttpGet("deleteUser/{email}")]
        public IActionResult DeleteUser(string email) {
            var user = Context.Users.SingleOrDefault(x => x.Email.Equals(email));
            if (user != null) {
                Context.Users.Remove(user);
                Context.SaveChanges();
            }
            return Ok();
        }
    }
}