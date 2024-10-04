using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FALOFinancialProofing.Services
{
    public class AuthServices
    {
        private readonly AppSetting appSetting;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AuthServices(UserManager<User> userManager, SignInManager<User> signInManager,
            IOptionsMonitor<AppSetting> optionsMonitor, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.appSetting = optionsMonitor.CurrentValue;
            this.roleManager = roleManager;
        }

        public async Task<UserDto?> LoginUser(SignInModel userLogin)
        {
            var user = await userManager.FindByNameAsync(userLogin.UserName);
            var checkPassword = await userManager.CheckPasswordAsync(user, userLogin.Password);
            if (user == null || !checkPassword)
            {
                return null;
            }
            var result = await signInManager.PasswordSignInAsync(userLogin.UserName, userLogin.Password, true, false);
            if (result.Succeeded)
            {
                var userDTO = new UserDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.UserName,
                    BirthDate = user.BirthDate,
                    RoleNames = userManager.GetRolesAsync(user).Result.ToList()
                };
                return userDTO;
            }
            return null;
        }

        //public async Task<User?> RegisterUser(SignUpRequest registerRequest)
        //{
        //    var validatedInformationRequest = await ValidatedInformationRequest(registerRequest);
        //    if (validatedInformationRequest == null)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        var newUser = new User
        //        {
        //            FirstName = validatedInformationRequest.FirstName,
        //            LastName = validatedInformationRequest.LastName,
        //            Email = validatedInformationRequest.Email,
        //            UserName = validatedInformationRequest.UserName,
        //        };
        //        await userManager.CreateAsync(newUser, registerRequest.Password);
        //        return newUser;
        //    }
        //}


        public async Task<IdentityResult?> RegisterUser(SignUpRequest registerRequest)
        {
            var validatedInformationRequest = await ValidatedInformationRequest(registerRequest);
            if (validatedInformationRequest == null)
            {
                return null;
            }
            else
            {
                var newUser = new User
                {
                    FirstName = validatedInformationRequest.FirstName,
                    LastName = validatedInformationRequest.LastName,
                    Email = validatedInformationRequest.Email,
                    UserName = validatedInformationRequest.UserName,
                };
                var result = await userManager.CreateAsync(newUser, registerRequest.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, AppRole.User);
                    return result;
                }
            }
            return null;
        }
        private async Task<SignUpRequest> ValidatedInformationRequest(SignUpRequest registerRequest)
        {
            //Validate information

            if (true)
            {
                return registerRequest;
            }
            return null;
        }

        public async Task<TokenModel> GenerateToken(UserDto User)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(appSetting.SecretKey);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, User.FirstName),
                new Claim(ClaimTypes.Name,User.LastName),
                new Claim(ClaimTypes.DateOfBirth,User.BirthDate.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, User.Email),
                new Claim(JwtRegisteredClaimNames.Sub, User.Email),
                //tokenId
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //new Claim("Id", User.Id.ToString()),
                //new Claim("TokenId", Guid.NewGuid().ToString()),

            };
            foreach (var roleName in User.RoleNames)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, roleName));
            }
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authClaims),
                Expires = DateTime.UtcNow.AddDays(appSetting.ExpiryInDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature),
                Issuer = appSetting.Issuer,
                Audience = appSetting.Audience,

            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);
            //var refeshToken = GenerateRefeshToken();
            //var refeshTokenEntity = new RefreshToken
            //{
            //    Id = Guid.NewGuid(),
            //    UserId = User.Id,
            //    Token = refeshToken,
            //    JwtId = token.Id,
            //    IssuedAt = DateTime.UtcNow,
            //    ExpiredAt = DateTime.UtcNow.AddDays(7),
            //    IsRevoked = false,
            //    IsUsed = false
            //};
            //var newRefreshToken = await refreshTokenRepository.InsertAsync(refeshTokenEntity);
            return new TokenModel
            {
                AccessToken = accessToken,
                RefeshToken = GenerateRefeshToken()
            };
        }

        private string GenerateRefeshToken()
        {
            var randomBytes = new byte[32];
            using (var rngCsp = RandomNumberGenerator.Create())
            {
                rngCsp.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }
    }
}