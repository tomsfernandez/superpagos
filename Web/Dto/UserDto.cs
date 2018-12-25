using System.Collections.Generic;
using Web.Model.Domain;
using static System.String;

namespace Web.Dto {
    public class UserDto {

        public long Id { get; set; } = 0;
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

        public List<string> Validate() {
            var errors = new List<string>();
            if (IsNullOrEmpty(Name)) errors.Add("Name es nulo o vacío");
            if (IsNullOrEmpty(Email)) errors.Add("Email es nulo o vacío");
            if (IsNullOrEmpty(Password)) errors.Add("Password es nulo o vacío");
            if (Role.Equals(null)) errors.Add("Role es nulo");
            return errors;
        }
    }
}