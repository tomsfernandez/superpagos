using Web.Dto;
using Web.Model.Domain;

namespace Web.Tests.Helpers {
    public class UserFactory {

        public static UserDto GetJuancitoAsDto(string email = "xx@yy.com") {
            return new UserDto {
                Name = "Juancito de la Vega", Email = email,
                Password = "el_zorro_zotudo", Role = Role.USER
            };    
        }

        public static UserDto GetJaimitoAsDto() {
            return new UserDto {
                Name = "Jaimito Ram�n Tercero",
                Email = "jaimito_ramon@superpagos.com",
                Password = "un_password_re_seguro",
                Role = Role.USER
            };
        }

        public static User GetJuancito() {
            return new User {
                Name = "Juancito de la Vega", Email = "xx@yy.com",
                Password = "el_zorro_zotudo", Role = Role.USER
            };
        }

        public static User GetJaimito() {
            return new User {
                Name = "Jaimito Ram�n Tercero",
                Email = "jaimito_ramon@superpagos.com",
                Password = "un_password_re_seguro",
                Role = Role.USER
            };
        }
    }
}