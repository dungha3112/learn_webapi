

using api.Dtos.Auth;
using api.Dtos.User;

namespace api.Interfaces
{
    public interface IAuthServices
    {
        Task<UserDto> RegisterAsync(RegisterDto registerDto);
    }
}
