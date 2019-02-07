using System.Collections.Generic;
using System.Security.Claims;

namespace Web.Model.JwtClaim {
    public class SameClaimExtractorFactory : ClaimExtractorFactory{

        public List<Claim> Claims { get; }

        public SameClaimExtractorFactory(List<Claim> claims) {
            Claims = claims;
        }

        public ClaimExtractor Build(List<Claim> claims) {
            return new ClaimExtractor(Claims);
        }
    }
}