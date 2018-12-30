using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Web.Model {
    public class PasswordEncrypter {

        private string Salt { get; }

        public PasswordEncrypter(string salt) {
            Salt = salt;
        }
        
        public string Encrypt(string password) {
            var salt = Encoding.UTF8.GetBytes(Salt);
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }
    }
}