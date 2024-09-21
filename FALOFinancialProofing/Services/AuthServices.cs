using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
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
        private readonly IRepository<User, int> userRepository;
        private readonly IRepository<RefreshToken, Guid > refreshTokenRepository;
        private readonly AppSetting appSetting;

        public AuthServices(IRepository<User, int> _repository,
            IRepository<RefreshToken, Guid> _refreshTokenRepository,
            IOptionsMonitor<AppSetting> optionsMonitor)
        {
            this.userRepository = _repository;
            this.refreshTokenRepository = _refreshTokenRepository;
            this.appSetting = optionsMonitor.CurrentValue;
        }

        public async Task<UserDto?> LoginUser(LoginModel userLogin)
        {
            var user = await userRepository
                .GetAll(u => u.Email.Equals(userLogin.Email) && u.Password.Equals(userLogin.Password))
                .Include(x => x.Roles)
                .FirstOrDefaultAsync();

            if (user == null) return null;

            var roles = user.Roles?.Select(x => x.RoleName).ToList() ?? new List<string>();
            var roleNames = roles.Count > 0 ? string.Join(", ", roles) : "No Roles Assigned";

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password,
                FullName = user.FullName,
                RoleName = roleNames
            };
        }

        public async Task<User?> RegisterUser(SignUpRequest registerRequest)
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
                    FullName = validatedInformationRequest.FullName,
                    Email = validatedInformationRequest.Email,
                    SocialAddress = validatedInformationRequest.SocialAddress,
                    Password = validatedInformationRequest.Password,
                    Status = 1,
                };
                await userRepository.InsertAsync(newUser);
                return newUser;
            }
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
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, User.FullName),
                    new Claim(JwtRegisteredClaimNames.Email, User.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, User.Email),
                    //tokenId
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role,User.RoleName),
                    new Claim("Id", User.Id.ToString()),
                    //new Claim("TokenId", Guid.NewGuid().ToString()),

                }),
                Expires = DateTime.UtcNow.AddDays(appSetting.ExpiryInDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature),
                Issuer = appSetting.Issuer,
                Audience = appSetting.Audience,

            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);
            var refeshToken = GenerateRefeshToken();
            var refeshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = User.Id,
                Token = refeshToken,
                JwtId = token.Id,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                IsUsed = false
            };
             var newRefreshToken = await refreshTokenRepository.InsertAsync(refeshTokenEntity);
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