
using api.Constants;
using api.Dtos.Auth;
using api.Dtos.User;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace api.Services
{
    public class AuthService : IAuthServices
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthService(
            UserManager<AppUser> userManager, IMapper mapper,
            ITokenService tokenService, SignInManager<AppUser> signInManager
        )
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        /*
            Login
        **/
        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginDto.UserName);
            if (user == null) throw new UnauthorizedAccessException("Invalid username.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) throw new UnauthorizedAccessException("Username not found and/or password incorrect");

            var token = _tokenService.CreateToken(user);

            var userDto = _mapper.Map<UserDto>(user, opt =>
            {
                opt.Items["Token"] = token;
            });

            return userDto;
        }

        /*
            RegisterAsync
        **/
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

            var token = _tokenService.CreateToken(appUser);

            var userDto = _mapper.Map<UserDto>(appUser, opt =>
            {
                opt.Items["Token"] = token;
            });

            return userDto;
        }
    }
}
