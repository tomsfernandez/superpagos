using System.Text;
using Microsoft.IdentityModel.Tokens;
using Web.Model.Domain;

namespace Web.Model.KeyBuilder {
    public class PasswordRecoveryKeyBuilder : TokenKeyBuilder {

        private User User { get;}

        public PasswordRecoveryKeyBuilder(User user) {
            User = user;
        }

        public SecurityKey BuildKey() {
            var key = $"{User.Password}";
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }
    }
}