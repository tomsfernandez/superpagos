using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.VisualBasic;
using static System.String;

namespace Web.Dto {
    public class RecoveryCredential {
        public long Id { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }

        public List<string> Validate() {
            var errors = new List<string>();
            if (Id == 0) errors.Add("Id is 0");
            if (IsNullOrEmpty(Token)) errors.Add("Token field is empty");
            if (IsNullOrEmpty(Password)) errors.Add("Password field is empty");
            if (IsNullOrEmpty(ConfirmedPassword)) errors.Add("ConfirmedPassword field is empty");
            if (Password != ConfirmedPassword) errors.Add("Password and ConfirmedPassword dont match");
            return errors;
        }
    }
}