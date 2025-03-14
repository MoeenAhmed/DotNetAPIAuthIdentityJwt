using DotNetIdentityShared;
using Microsoft.AspNetCore.Identity;

namespace DotNetIdentityAPI.Services
{
    public class UserService : IUserService
    {
        private UserManager<IdentityUser> _userManager;
        public UserService(UserManager<IdentityUser> userManager)
        {
                _userManager = userManager;
        }
        public async Task<UserManagerResponse> RegisterUserAsync(RegisterDTO registerDTO)
        {
            if (registerDTO == null)
            {
                throw new NullReferenceException("Register DTO is null");
            }

            if (registerDTO.Password != registerDTO.ConfirmPassword)
            {
                return new UserManagerResponse()
                {
                    Message = "Password and confirm password doesn't match",
                    IsSuccess = true,
                    Result = null
                };
            }

            var identityUser = new IdentityUser() { UserName = registerDTO.Email, Email = registerDTO.Email };

            var result = await _userManager.CreateAsync(identityUser, registerDTO.Password);

            if (result.Succeeded)
            {
                return new UserManagerResponse()
                {
                    Message = "User created successfully",
                    IsSuccess = false,
                    Result = _userManager.GetUserIdAsync(identityUser)
                };
            }
            else
            {
                return new UserManagerResponse()
                {
                    Message = string.Join("\n",result.Errors),
                    IsSuccess = false,
                    Result = null
                };
            }


        }
    }
}
