using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Web.Model.Domain;
using static System.String;

namespace Web.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class CypressController : Controller {
        private AppDbContext Context { get; }
        private IConfiguration Configuration { get; }
        private string DemoEndpoint { get; }

        public CypressController(AppDbContext context, IConfiguration configuration) {
            Context = context;
            Configuration = configuration;
            DemoEndpoint = Configuration["FakeProviderEndpoint"];
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

        [HttpGet("addProvider")]
        public IActionResult AddTestProvider() {
            if (Context.Providers.Any(x => x.Code.Equals("DEMO"))) return Ok();
            var provider = new Provider {
                Name = "DemoProvider",
                Company = "DemoCompany",
                Code = "DEMO",
                PaymentEndpoint = DemoEndpoint,
                RollbackEndpoint = $"{DemoEndpoint}/rollback",
                LinkEndpoint = $"{DemoEndpoint}/link"                
            };
            Context.Providers.Add(provider);
            Context.SaveChanges();
            return Ok();
        }
        
        [HttpGet("deleteAllFromUser/{email}")]
        public IActionResult DeleteAllFromUser(string email) {
            var user = Context.Users.SingleOrDefault(x => x.Email.Equals(email));
            if (user != null)
            {
                var methods = Context.PaymentMethods.Where(x => x.User.Id == user.Id).ToList();
                Context.PaymentMethods.RemoveRange(methods);
                Context.SaveChanges();
            }
            return Ok();
        }

        [HttpGet("deleteProvider")]
        public IActionResult DeleteTestProvider() {
            var provider = Context.Providers.SingleOrDefault(x => x.Code.Equals("DEMO"));
            if (provider != null) {
                Context.PaymentMethods.Where(x => x.Provider.Id == provider.Id).DeleteFromQuery();
                Context.Providers.Remove(provider);
                Context.SaveChanges();
            }
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

        [HttpGet("addMovements/{email}")]
        public IActionResult AddTestMovements(string email)
        {
            var user = Context.Users.SingleOrDefault(aUser => aUser.Email.Equals(email));
            var provider = Context.Providers.SingleOrDefault(aProvider => aProvider.Code.Equals("DEMO"));
            if (user != null && provider != null)
            {
                var testPaymentMethod = new PaymentMethod {Token = "aToken", User = user, Provider = provider};
                var testMovements = new List<Movement>
                {
                    new Movement {Account = testPaymentMethod, Amount = 149.99, OperationType = Operation.DEPOSIT},
                    new Movement {Account = testPaymentMethod, Amount = 15.99, OperationType = Operation.WITHDRAWAL},
                    new Movement {Account = testPaymentMethod, Amount = 19.99, OperationType = Operation.DEPOSIT}
                };
                Context.Movements.AddRange(testMovements);
                Context.SaveChanges();
            }
            return Ok();
        }

        [HttpGet("removeMovements/{email}")]
        public IActionResult RemoveTestMovements(string email)
        {
            var user = Context.Users.SingleOrDefault(aUser => aUser.Email.Equals(email));
            if (user != null)
            {
                var movements = Context.Movements.Where(x => x.Account.User.Id == user.Id).ToList();
                Context.Movements.RemoveRange(movements);
                Context.SaveChanges();
            }
            return Ok();
        }
    }
}