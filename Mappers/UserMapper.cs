
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
            CreateMap<AppUser, UserDto>().ConstructUsing((src, context) => new UserDto
            {
                UserName = src.UserName!,
                Email = src.Email!,
                Token = context.Items["Token"]?.ToString() ?? ""
            });

            CreateMap<RegisterDto, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        }
    }
}
