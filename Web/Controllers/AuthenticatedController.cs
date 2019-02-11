using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Web.Model.JwtClaim;

namespace Web.Controllers {
    public class AuthenticatedController : ControllerBase{
        
        public ClaimExtractorFactory Factory { get; }

        public AuthenticatedController(ClaimExtractorFactory factory) {
            Factory = factory;
        }

        public long GetIdFromToken() {
            var claimExtractor = Factory.Build(User?.Claims.ToList());
            return claimExtractor.GetId();
        }
    }
}