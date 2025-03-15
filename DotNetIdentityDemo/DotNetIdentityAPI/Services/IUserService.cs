using DotNetIdentityShared;

namespace DotNetIdentityAPI.Services
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterDTO registerDTO);
        Task<UserManagerResponse> LoginUserAsync(LoginDTO loginDTO);
    }
}
