using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Web.Model.JwtClaim {
    public class ClaimExtractor {
        
        private List<Claim> Claims { get; }

        public ClaimExtractor(List<Claim> claims) {
            Claims = claims;
        }

        public string Get(string claim) {
            return Claims.First(x => x.Type.Equals(claim)).Value;
        }

        public long GetId() {
            var userIdAsString = Claims.First(x => x.Type.Equals(ClaimTypes.Name)).Value;
            return long.Parse(userIdAsString);
        }

        public bool Exists(string claim) {
            return Claims.Any(x => x.Type.Equals(claim));
        }
    }
}