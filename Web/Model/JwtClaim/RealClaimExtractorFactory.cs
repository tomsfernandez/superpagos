using System.Collections.Generic;
using System.Security.Claims;

namespace Web.Model.JwtClaim {
    public class RealClaimExtractorFactory : ClaimExtractorFactory{

        public ClaimExtractor Build(List<Claim> claims) {
            return new ClaimExtractor(claims);
        }
    }
}