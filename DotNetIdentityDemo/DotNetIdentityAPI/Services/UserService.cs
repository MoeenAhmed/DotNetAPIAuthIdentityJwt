using DotNetIdentityShared;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DotNetIdentityAPI.Services
{
    public class UserService : IUserService
    {
        private UserManager<IdentityUser> _userManager;
        private IConfiguration configuration;
        public UserService(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            this.configuration = configuration;
        }

        public async Task<UserManagerResponse> LoginUserAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                return new UserManagerResponse()
                {
                    Message = "User not found with email",
                    IsSuccess = false,
                    Result = null
                };
            }
            var result = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!result)
            {
                return new UserManagerResponse()
                {
                    Message = "Incorrect password",
                    IsSuccess = false,
                    Result = null
                };
            }

            var userClaims = new Claim[]
            {
                new Claim(ClaimTypes.Email, loginDTO.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

           // var jwtKey = GenerateJwtSecretKey(32); 
            
            var token = new JwtSecurityToken(

                issuer: configuration["AuthSettings:Issuer"],
                audience: configuration["AuthSettings:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthSettings:Key"])),
                SecurityAlgorithms.HmacSha256
                )
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenResponse()
            {
                Message = "Token generated successfully",
                IsSuccess = true,
                Result = tokenString,
                ExpiresOn = token.ValidTo
            };
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
                    Message = string.Join("\n", result.Errors),
                    IsSuccess = false,
                    Result = null
                };
            }
        }

        //public static string GenerateJwtSecretKey(int size)
        //{
        //    using (var rng = RandomNumberGenerator.Create())
        //    {
        //        byte[] key = new byte[size];
        //        rng.GetBytes(key); 
        //        return Convert.ToBase64String(key);  
        //    }
        //}
    }
}
