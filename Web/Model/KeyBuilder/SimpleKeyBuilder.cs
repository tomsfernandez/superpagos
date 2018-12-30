using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Web.Model.KeyBuilder {
    public class SimpleKeyBuilder : TokenKeyBuilder{

        public string Key { get; }

        public SimpleKeyBuilder(string key) {
            Key = key;
        }

        public SecurityKey BuildKey() {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
        }
    }
}