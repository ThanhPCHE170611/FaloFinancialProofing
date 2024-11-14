using Azure;
using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.DTOs.CampaignMemberDTO;
using FALOFinancialProofing.DTOs.ProjectDTOs;
using FALOFinancialProofing.DTOs.RoleDTOs;
using FALOFinancialProofing.DTOs.UserDTOs;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using FALOFinancialProofing.Services.EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
//using System.Security.Policy;
using System.Text;




namespace FALOFinancialProofing.Services
{
    public class AuthServices
    {
        private readonly AppSetting appSetting;
        private readonly IEmailService emailService;
        private readonly RoleManager<IdentityRole> roleManager;
        public readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IRepository<CampaignMember, int> campaignMemberRepository;
        //moi
        private readonly LinkGenerator _linkGenerator;


        public AuthServices(UserManager<User> userManager, SignInManager<User> signInManager,
            IOptionsMonitor<AppSetting> optionsMonitor, RoleManager<IdentityRole> roleManager,
            IEmailService emailService,
            LinkGenerator linkGenerator, IRepository<CampaignMember, int> campaignMemberRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.appSetting = optionsMonitor.CurrentValue;
            this.roleManager = roleManager;
            this.emailService = emailService;
            _linkGenerator = linkGenerator;
            this.campaignMemberRepository = campaignMemberRepository;
        }
        public async Task<bool> CheckUserInRole(string userId, string userRole, StringBuilder message)
        {
            bool checkValid = false;
            try
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                var isInRole = await userManager.IsInRoleAsync(user, userRole);
                if (!isInRole)
                {
                    throw new Exception("User Role is not permitted");
                }
                checkValid = true;
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"CheckUserInRole: {ex.Message}");
            }
            return checkValid;
        }

        public async Task<bool> CheckUserInRoleId(string userId, string userRoleID, StringBuilder message)
        {
            bool checkValid = false;
            try
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                var role = await roleManager.FindByIdAsync(userRoleID);
                var isInRole = await userManager.IsInRoleAsync(user, role.Name);
                if (!isInRole)
                {
                    throw new Exception("User Role is not permitted");
                }
                checkValid = true;
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"CheckUserInRole: {ex.Message}");
            }
            return checkValid;
        }

        public async Task<bool> CheckUserInDonorRole(string userId, string userRoleID, StringBuilder message)
        {
            bool checkValid = false;
            try
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                var role = await roleManager.FindByIdAsync(userRoleID);
                var isInRole = await userManager.IsInRoleAsync(user, role.Name);
                if (!isInRole)
                {
                    throw new Exception("User Role is not permitted");
                }
                checkValid = true;
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"CheckUserInRole: {ex.Message}");
            }
            return checkValid;
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
                    Id = user.Id,
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
        public async Task<UserInformation> UserInformationProcess(User ui)
        {
            if (ui == null)
            {
                throw new ArgumentNullException(nameof(ui));
            }

            var roles = await userManager.GetRolesAsync(ui);
            var roleDetails = new List<RoleInformation>();

            foreach (var roleName in roles)
            {
                var role = await roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    roleDetails.Add(new RoleInformation
                    {
                        RoleId = role.Id,
                        RoleName = role.Name
                    });
                }
            }

            return new UserInformation()
            {
                Id = ui.Id,
                Email = ui.Email ?? string.Empty, // Xử lý null reference
                FirstName = ui.FirstName ?? string.Empty, // Xử lý null reference
                LastName = ui.LastName ?? string.Empty, // Xử lý null reference
                BirthDate = ui.BirthDate,
                Roles = roleDetails
            };
        }

        public async Task<List<UserInformation>> GetUserNotInCampaignById(int CampaignId)
        {

            var Users = await userManager.Users.Where(u => !u.CampaignMembers.Any(cm => cm.CampaignId == CampaignId) && !u.UserRoles.Any(ur => ur.RoleId == AppRole.DonorRoleId)).ToListAsync();

            List<UserInformation> data = new List<UserInformation>();
            try
            {
                foreach (var user in Users)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    //if (roles.Any(r => r == AppRole.Donor))
                    //{
                    //    continue;
                    //}
                    var roleDetails = new List<RoleInformation>();

                    foreach (var roleName in roles)
                    {
                        var role = await roleManager.FindByNameAsync(roleName);
                        if (role != null)
                        {
                            roleDetails.Add(new RoleInformation
                            {
                                RoleId = role.Id,
                                RoleName = role.Name
                            });
                        }
                    }
                    data.Add(new UserInformation()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        BirthDate = user.BirthDate,
                        Roles = roleDetails
                    });
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetUserNotInCampaignById: {ex.Message}");
            }
            return data;
        }

        public async Task<List<SelectedUserInformation>> GetUserInformationList(List<CreateManyCampaignMemberDTO> createManyCampaignMemberDTOs)
        {
            var data = new List<SelectedUserInformation>();
            try
            {
                foreach (var item in createManyCampaignMemberDTOs)
                {
                    var user = await userManager.FindByIdAsync(item.UserId);
                    if (user != null)
                    {
                        var role = await roleManager.FindByIdAsync(item.RoleId);
                        data.Add(new SelectedUserInformation()
                        {
                            Id = user.Id,
                            Email = user.Email,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            BirthDate = user.BirthDate,
                            RoleInformation = new RoleInformation
                            {
                                RoleId = role.Id,
                                RoleName = role.Name
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetUserInformationList: {ex.Message}");
            }
            return data;
        }

        public async Task<List<UserInformation_Admin>> GetAccountList()
        {
            var data = new List<UserInformation_Admin>();
            try
            {
                //var    users = await userManager.Users.ToListAsync();
                data = await userManager.Users.Select(u => new UserInformation_Admin()
                {
                    Id = u.Id,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    BirthDate = u.BirthDate,
                    Roles = u.UserRoles.Select(ur => new RoleInformation
                    {
                        RoleId = ur.RoleId,
                        RoleName = roleManager.Roles.FirstOrDefault(r => r.Id == ur.RoleId).Name
                    }).ToList(),
                    SocialNetworkRequests = u.SocialNetworks.Select(snr => new SocialNetworkRequest
                    {
                        Id = snr.Id,
                        UserId = snr.UserId,
                        SocialNetworksLink = snr.SocialNetworksLink,
                    }).ToList()
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAccountList: {ex.Message}");
            }

            return data;
        }

        public async Task<UserInformation_Admin> GetAccount(string UserId, StringBuilder message)
        {
            UserInformation_Admin data = null!;
            try
            {
                data = await userManager.Users.Where(u => u.Id == UserId).Select(u => new UserInformation_Admin()
                {
                    Id = u.Id,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    BirthDate = u.BirthDate,
                    Roles = u.UserRoles.Select(ur => new RoleInformation
                    {
                        RoleId = ur.RoleId,
                        RoleName = roleManager.Roles.FirstOrDefault(r => r.Id == ur.RoleId).Name
                    }).ToList(),
                    SocialNetworkRequests = u.SocialNetworks.Select(snr => new SocialNetworkRequest
                    {
                        Id = snr.Id,
                        UserId = snr.UserId,
                        SocialNetworksLink = snr.SocialNetworksLink,
                    }).ToList()
                }).FirstOrDefaultAsync();
                if (data == null)
                {
                    throw new Exception("User not found in system!");
                }
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"GetAccount: {ex.Message}");
            }

            return data;
        }
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
                    TwoFactorEnabled = true,
                };
                var result = await userManager.CreateAsync(newUser, registerRequest.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, AppRole.Volunteer);
                    return result;
                }
            }
            return null;
        }

        public async Task<IdentityResult?> RegisterDonor(SignUpRequest registerRequest)
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
                    TwoFactorEnabled = true,
                };
                var result = await userManager.CreateAsync(newUser, registerRequest.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, AppRole.Donor);
                    return result;
                }
            }
            return null;
        }
        public async Task<IdentityResult?> AdminRegisterUser(SignUpAdminRequest registerRequest)
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
                    TwoFactorEnabled = true,
                };
                var result = await userManager.CreateAsync(newUser, registerRequest.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(newUser, registerRequest.Roles);
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

        private async Task<SignUpAdminRequest> ValidatedInformationRequest(SignUpAdminRequest registerRequest)
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
                new Claim(JwtRegisteredClaimNames.NameId, User.Id),
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

        public async Task<string?> ForgotPassword(string email, HttpContext httpContext)
        {

            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var forgotPasswordLink = GenerateForgotPasswordLink(httpContext, token, email);
                var message = new Message(new string[] { user.Email! }, "Forgot Password link", forgotPasswordLink!);
                emailService.SendEmail(message);

                return "Password reset email sent successfully.";
            };
            return null;

        }

        public async Task<IdentityResult> ResetPassword(ResetPassword resetPassword)
        {

            var user = await userManager.FindByEmailAsync(resetPassword.Email);
            if (user != null)
            {
                var resetPassResult = await userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
                if (!resetPassResult.Succeeded)
                {
                    return resetPassResult;
                }
                return resetPassResult;
            }
            return null; // tam thoi


        }
        public string GenerateForgotPasswordLink(HttpContext httpContext, string token, string email)
        {
            // Sử dụng LinkGenerator để tạo URL tương tự như Url.Action
            var forgotPasswordLink = _linkGenerator.GetUriByAction(
                httpContext,
                action: "ResetPassword",
                controller: "Users",
                values: new { token, email },
                scheme: httpContext.Request.Scheme
            );

            return forgotPasswordLink;
        }
        public async Task<bool> UpdateUserRoleAsync(UpdateUserRole updateUserRole, StringBuilder message)
        {
            User user = null!;
            bool result = false;
            try
            {
                user = await userManager.FindByIdAsync(updateUserRole.UserId);
                if (user == null)
                {
                    throw new Exception("User not found in system!");
                }
                var role = await roleManager.FindByIdAsync(updateUserRole.RoleId);
                if (role == null)
                {
                    throw new Exception("Role not found in system!");
                }
                result = await userManager.AddToRoleAsync(user, role.Name) == IdentityResult.Success;
                if (result)
                {
                    message.Append("User role updated successfully");
                }
                else
                {
                    message.Append("User role update failed");
                }
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"UpdateUserRoleAsync: {ex.Message}");
            }

            return result;
        }

        public async Task<double> GetUserDebInCampaign(string userId, int campaignId, StringBuilder message)
        {
            var userinCampaign = await campaignMemberRepository.GetAll(x => x.UserId.Equals(userId)
                                    && x.CampaignId == campaignId).FirstOrDefaultAsync();
            if(userinCampaign == null)
            {
                message.Append("Can't not find debt of this user with specify campaign");
                return Double.MinValue;
            }
            return userinCampaign.Debt;
        }
    }


}