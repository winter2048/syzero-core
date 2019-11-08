using AutoMapper;
using Example.Core.Authorization.Users;

namespace Example.Application.Users.Dto
{
    public class UserMapProfile : Profile
    {
        public UserMapProfile()
        {
            CreateMap<UserDto, User>();
        }
            
    }
}
