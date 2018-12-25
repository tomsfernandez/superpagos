using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using static System.String;

namespace Web.Dto {
    public class LoginCredentials {
        public string Email { get; set; }
        public string Password { get; set; }

        public List<string> Validate() {
            var errors = new List<string>();
            if (IsNullOrEmpty(Email)) errors.Add("Email es vacío o nulo");
            if (IsNullOrEmpty(Password)) errors.Add("Password es vacío o nulo");
            return errors;
        }
    }
}