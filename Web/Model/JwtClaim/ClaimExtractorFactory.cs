using System.Collections.Generic;
using System.Security.Claims;

namespace Web.Model.JwtClaim {
    public interface ClaimExtractorFactory {

        ClaimExtractor Build(List<Claim> claims);
    }
}