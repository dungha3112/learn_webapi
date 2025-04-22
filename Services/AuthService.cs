
using api.Constants;
using api.Dtos.Auth;
using api.Dtos.User;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace api.Services
{
    public class AuthService : IAuthServices
    {
        private UserManager<AppUser> _userManager;
        private IMapper _mapper;

        public AuthService(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email!);
            if (existingUser != null)
            {
                throw new ApplicationException("Email already in use.");
            }

            var appUser = _mapper.Map<AppUser>(registerDto);
            // Console.WriteLine(JsonConvert.SerializeObject(appUser, Formatting.Indented));

            var createUser = await _userManager.CreateAsync(appUser, registerDto.Password!);
            if (!createUser.Succeeded)
            {
                var errors = string.Join("; ", createUser.Errors.Select(e => e.Description));
                throw new ApplicationException($"User creation failed: {errors}");
            }

            // role
            var roleResult = await _userManager.AddToRoleAsync(appUser, RoleUserConstants.USER);
            if (!roleResult.Succeeded)
            {
                var errors = string.Join("; ", roleResult.Errors.Select(e => e.Description));
                throw new ApplicationException($"User creation failed: {errors}");
            }



            var userDto = _mapper.Map<UserDto>(appUser);
            return userDto;
        }
    }
}
