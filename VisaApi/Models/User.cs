using AutoMapper;

namespace VisaApi.Models {
    public class User {
        
        [IgnoreMap]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}