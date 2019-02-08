using Web.Model.Domain;

namespace Web.Dto {
    public class UserDtoWithoutPassword {

        public long Id { get; set; } = 0;
        public string Name { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
    }
}