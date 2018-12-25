using AutoMapper;
using Web.Dto;
using Web.Model.Domain;

namespace Web {
    public class DomainProfile : Profile{
        public DomainProfile() {
            CreateMap<User, UserDto>().ForSourceMember(x => x.Id, opt => opt.DoNotValidate());
        }
    }
}