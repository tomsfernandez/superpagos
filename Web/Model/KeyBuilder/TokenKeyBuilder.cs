using Microsoft.IdentityModel.Tokens;

namespace Web.Model.KeyBuilder {
    public interface TokenKeyBuilder {
        SecurityKey BuildKey();
    }
}