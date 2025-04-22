
using api.Dtos.Auth;
using api.Dtos.User;
using api.Models;
using AutoMapper;

namespace api.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<AppUser, UserDto>();
            CreateMap<RegisterDto, AppUser>();
        }
    }
}
