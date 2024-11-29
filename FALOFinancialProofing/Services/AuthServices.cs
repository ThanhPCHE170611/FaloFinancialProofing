using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.DTOs.CampaignMemberDTO;
using FALOFinancialProofing.DTOs.RoleDTOs;
using FALOFinancialProofing.DTOs.SDGDTOs;
using FALOFinancialProofing.DTOs.UserDTOs;
using FALOFinancialProofing.DTOs.UserSDGDTO;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using FALOFinancialProofing.Services.EmailService;
using FALOFinancialProofing.Services.SocialNetworkService;
using FALOFinancialProofing.Services.UserSDGServices;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using Google.Apis.Oauth2.v2.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
//using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;




namespace FALOFinancialProofing.Services
{
    public class AuthServices
    {
        private readonly AppSetting appSetting;
        private readonly IEmailService emailService;
        private readonly RoleManager<Role> roleManager;
        public readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IRepository<CampaignMember, int> campaignMemberRepository;
        private readonly ISocialNetworkService socialNetworkService;
        private readonly RoleService _roleService;
        private readonly IUserSDGService userSDGService;
        //moi
        private readonly LinkGenerator _linkGenerator;


        public AuthServices(UserManager<User> userManager, SignInManager<User> signInManager,
            IOptionsMonitor<AppSetting> optionsMonitor, RoleManager<Role> roleManager,
            IEmailService emailService,
            LinkGenerator linkGenerator, IRepository<CampaignMember, int> campaignMemberRepository, ISocialNetworkService socialNetworkService, RoleService roleService, IUserSDGService userSDGService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.appSetting = optionsMonitor.CurrentValue;
            this.roleManager = roleManager;
            this.emailService = emailService;
            _linkGenerator = linkGenerator;
            this.campaignMemberRepository = campaignMemberRepository;
            this.socialNetworkService = socialNetworkService;
            _roleService = roleService;
            this.userSDGService = userSDGService;
        }
        public async Task<bool> CheckUserExist(string userId, StringBuilder message)
        {
            bool checkValid = false;
            try
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                checkValid = true;
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"CheckUserExist: {ex.Message}");
            }
            return checkValid;
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

        public virtual async Task<bool> CheckRole(string userId, string RoleId, string RoleName, StringBuilder message)
        {
            bool checkValid = false;
            try
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                var role = await roleManager.FindByIdAsync(RoleId);
                if (role == null)
                {
                    throw new Exception("User Role is not permitted");
                }
                if (role.Name.Equals(RoleName))
                {
                    checkValid = true;
                }
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
                if (role == null)
                {
                    throw new Exception("Role not found");
                }
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

        public async Task<bool> CheckIsAccountingRole(string userRoleID, StringBuilder message)
        {
            bool checkValid = false;
            try
            {
                var role = await roleManager.FindByIdAsync(userRoleID);
                if (role != null && role.Name.Equals(AppRole.Accounting))
                    checkValid = true;
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"CheckIsAccountingRole: {ex.Message}");
            }
            return checkValid;
        }

        public async Task<bool> CheckIsDonorAdminPmbPmRole(string userRoleID, StringBuilder message)
        {
            bool checkValid = false;
            try
            {
                var role = await roleManager.FindByIdAsync(userRoleID);
                if (role != null && (role.Name.Equals(AppRole.ProjectManagementBoard) || role.Name.Equals(AppRole.Admin) || role.Name.Equals(AppRole.ProjectManager)))
                    checkValid = true;
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"CheckIsAccountingRole: {ex.Message}");
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
                if (role == null)
                {
                    throw new Exception("Role not found");
                }
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
        public bool checkLockoutAccount(User
             user, StringBuilder message)
        {
            // Check if the account is locked out
            if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.UtcNow)
            {
                // Account is locked out
                message.Append("Account is locked out");
                return true;
            }

            return false;
        }
        public async Task<UserDto?> LoginUser(SignInModel userLogin, StringBuilder message)
        {
            var user = await userManager.FindByNameAsync(userLogin.UserName);
            var checkPassword = await userManager.CheckPasswordAsync(user, userLogin.Password);
            if (user == null || !checkPassword)
            {
                message.Append("Invalid Username/Password");
                return null;
            }
            if (checkLockoutAccount(user, message))
            {

                return null;
            }

            var result = await signInManager.PasswordSignInAsync(userLogin.UserName, userLogin.Password, false, false);
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
                    RoleInformations = await _roleService.GetRoleInformationsByUserId(user.Id)
                    //RoleNames = (await userManager.GetRolesAsync(user)).ToList()
                };
                return userDTO;
            }
            message.Append("Invalid Username/Password");
            return null;
        }
        public async Task<UserDto> GetUserDto(Userinfo userinfo)
        {
            var user = await userManager.FindByEmailAsync(userinfo.Email);
            var userDTO = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                BirthDate = user.BirthDate,
                RoleInformations = await _roleService.GetRoleInformationsByUserId(user.Id)
            };
            return userDTO;
        }

        public async Task<User> GetUserById(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            return user;
        }
        public async Task<bool> CheckGoogleExistAccount(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }
            return true;
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
            //var Users = await userManager.Users.Where(u => !u.CampaignMembers.Any(cm => cm.CampaignId == CampaignId) && !u.UserRoles.Any(ur => ur.RoleId == AppRole.DonorRoleId)).ToListAsync();
            var Users = await userManager.Users.Where(u => !u.CampaignMembers.Any(cm => cm.CampaignId == CampaignId)).ToListAsync();
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
                        if (roleName.Equals(AppRole.Donor) || roleName.Equals(AppRole.ProjectManager) || roleName.Equals(AppRole.ProjectManagementBoard) || roleName.Equals(AppRole.Admin))
                            continue;
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
                    if (roleDetails.Count != 0)
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

        public async Task<List<UserInformation_Admin>> GetAccountList(HttpRequest request)
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
                    Image = UrlHelper.GetImageUrl(request, u.Image, FolderImage.UserImageUpload),
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

        public async Task<List<UserInformation_Admin>> GetPMBAccountList(HttpRequest request)
        {
            var data = new List<UserInformation_Admin>();
            try
            {
                data = await userManager.Users.Where(u => u.UserRoles.Any(ur => ur.Role.Name.Equals(AppRole.ProjectManagementBoard)))
                    .Select(u => new UserInformation_Admin()
                    {
                        Id = u.Id,
                        Email = u.Email,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        BirthDate = u.BirthDate,
                        Image = UrlHelper.GetImageUrl(request, u.Image, FolderImage.UserImageUpload),
                        Roles = u.UserRoles.Where(usr => usr.Role.Name.Equals(AppRole.ProjectManagementBoard)).Select(ur => new RoleInformation
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
        public async Task<IdentityResult?> AdminRegisterUser(SignUpAdminRequest registerRequest, StringBuilder message)
        {
            try
            {
                var validatedInformationRequest = await ValidatedInformationRequest(registerRequest);
                if (validatedInformationRequest == null)
                {
                    return null;
                }
                else
                {
                    var user = await userManager.FindByEmailAsync(validatedInformationRequest.Email);
                    if (user != null)
                        throw new Exception("Email is already registered in the system!");
                    var newUser = new User
                    {
                        FirstName = validatedInformationRequest.FirstName,
                        LastName = validatedInformationRequest.LastName,
                        Email = validatedInformationRequest.Email,
                        UserName = validatedInformationRequest.UserName,
                        BirthDate = validatedInformationRequest.BirthDate,
                        Gender = validatedInformationRequest.Gender,
                        Address = validatedInformationRequest.Address,
                        PhoneNumber = validatedInformationRequest.PhoneNumber,
                        TwoFactorEnabled = true,
                    };
                    var result = await userManager.CreateAsync(newUser, registerRequest.Password);
                    if (result.Succeeded)
                    {
                        result = await userManager.AddToRolesAsync(newUser, registerRequest.Roles);
                        return result;
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            message.AppendLine(error.Description);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                Console.WriteLine($"AdminRegisterUser: {ex.Message}");
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
                new Claim(JwtRegisteredClaimNames.Sub,  User.FirstName +" "+ User.LastName),
                //tokenId
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, User.Id),
                new Claim("UserName", User.UserName ),
                //new Claim("RoleId", User.RoleNames),
                //new Claim("TokenId", Guid.NewGuid().ToString()),

            };
            foreach (var roleName in User.RoleInformations)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, roleName.RoleName));
                authClaims.Add(new Claim("RoleId", roleName.RoleId));
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
                //var token = await userManager.GeneratePasswordResetTokenAsync(user);
                //var forgotPasswordLink = GenerateForgotPasswordLink(httpContext, token, email);
                //var message = new Message(new string[] { user.Email! }, "Forgot Password link", forgotPasswordLink!);
                //emailService.SendEmail(message);

                var token = await userManager.GeneratePasswordResetTokenAsync(user);

                // Tạo nội dung email
                string resetPasswordLink = "https://localhost:7109"; // Đường dẫn cố định
                string emailContent = $@"
Mã token của bạn: {token}
Mã token này sẽ bị vô hiệu hóa sau 10 phút.

Click vào link này để đặt lại mật khẩu: {resetPasswordLink}";

                // Create the email message
                var message = new Message(
                    new string[] { user.Email! },
                    "Forgot Password",
                    emailContent
                );

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
        public async Task<bool> ValidateResetPasswordAsync(ResetPassword resetPassword, StringBuilder message)
        {
            bool IsValid = false;
            try
            {
                var user = await userManager.FindByEmailAsync(resetPassword.Email);
                if (user == null)
                {
                    throw new Exception($"Email: {resetPassword.Email} is not registered in the system!");
                }
                else
                {
                    var isValidToken = await userManager.VerifyUserTokenAsync(user, userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetPassword.Token);
                    if (!isValidToken)
                    {
                        throw new Exception("Incorrect Token!");
                    }

                    if (string.IsNullOrEmpty(resetPassword.Password))
                    {
                        throw new Exception("Password cannot be null");
                    }
                    else
                    {
                        if (resetPassword.Password.Length < 8
                        || !Regex.IsMatch(resetPassword.Password, "[a-z]")
                        || !Regex.IsMatch(resetPassword.Password, "[A-Z]")
                        || !Regex.IsMatch(resetPassword.Password, "[0-9]")
                        || !Regex.IsMatch(resetPassword.Password, @"[@$!%*?&]"))
                        {
                            throw new Exception("Password must be at least 8 characters, including letters, uppercase letters, numbers, and special characters.");
                        }
                        else
                        {
                            if (!resetPassword.Password.Equals(resetPassword.ConfirmPassword))
                            {
                                throw new Exception("The password and confirmation password do not match.");
                            }
                        }
                    }
                }
                IsValid = true;
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"ValidateResetPassword: {ex.Message}");
            }

            return IsValid;
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
            if (userinCampaign == null)
            {
                message.Append("Can't not find debt of this user with specify campaign");
                return Double.MinValue;
            }
            return userinCampaign.Debt;
        }

        public async Task<bool> CheckValidUser(UpdateUserProfileRequest updateUserProfileRequest, StringBuilder message)
        {
            bool checkValid = false;
            try
            {
                var user = await userManager.FindByIdAsync(updateUserProfileRequest.Id);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                //long MaxFileSize = 5 * 1024 * 1024;
                if (updateUserProfileRequest.LogoFile != null && updateUserProfileRequest.LogoFile.Length > FileHelper.UserImageMaxFileSize)
                {
                    throw new Exception("Logo is too large");
                }

                var data = JsonConvert.DeserializeObject<List<SocialNetworkRequest>>(updateUserProfileRequest.SocialNetworkRequestJsons);

                var SdgData = JsonConvert.DeserializeObject<List<SDGUserRequest>>(updateUserProfileRequest.SDGUserRequestJsons);

                checkValid = true;
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"CheckUserInRole: {ex.Message}");
            }
            return checkValid;
        }

        public async Task<UserProfileDetail?> GetUserProfile(string userId, StringBuilder message, HttpRequest request)
        {
            UserProfileDetail data = null!;
            try
            {
                data = await userManager.Users.Where(u => u.Id == userId)
                    .Select(u => new UserProfileDetail()
                    {
                        Id = u.Id,
                        Email = u.Email,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        BirthDate = u.BirthDate,
                        Gender = u.Gender,
                        Address = u.Address,
                        WorkPlace = u.WorkPlace,
                        Bio = u.Bio,
                        Image = UrlHelper.GetImageUrl(request, u.Image, FolderImage.UserImageUpload),
                        Education = u.Education,
                        Skill = u.Skill,
                        Hobby = u.Hobby,
                        Strength = u.Strength,
                        PhoneNumber = u.PhoneNumber,
                        VolunteerExperience = u.VolunteerExperience,
                        VolunteerGoal = u.VolunteerGoal,
                        SocialNetworkRequests = u.SocialNetworks.Select(snr => new SocialNetworkRequest
                        {
                            Id = snr.Id,
                            UserId = snr.UserId,
                            SocialNetworksLink = snr.SocialNetworksLink,
                        }).ToList(),
                        userSDGInformations = u.UserSDGs.Select(usdg => new UserSDGInformation
                        {
                            Id = usdg.Id,
                            UserId = usdg.UserId,
                            sDGInformation = new SDGInformation()
                            {
                                Id = usdg.SDG.Id,
                                SDGName = usdg.SDG.SDGName
                            }
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
        #region May use later
        //FileHelper
        //public async Task<bool> UpdateUserProfile(UpdateUserProfileRequest updateUserProfileRequest, StringBuilder message)
        //{
        //    User user = null!;
        //    bool result = false;
        //    try
        //    {
        //        user = await userManager.FindByIdAsync(updateUserProfileRequest.Id);
        //        user.FirstName = updateUserProfileRequest.FirstName;
        //        user.LastName = updateUserProfileRequest.LastName;
        //        user.Email = updateUserProfileRequest.Email;
        //        user.BirthDate = updateUserProfileRequest.BirthDate;
        //        user.Image = await FileHelper.ConvertIFormFileToStringAsync(updateUserProfileRequest.LogoFile) != null ? await FileHelper.ConvertIFormFileToStringAsync(updateUserProfileRequest.LogoFile) : user.Image;
        //        foreach (var item in updateUserProfileRequest.SocialNetworkRequests)
        //        {
        //            var socialNetwork = await socialNetworkService.GetSocialNetworkByIdAsync(item.Id.Value);
        //            socialNetwork.SocialNetworksLink = item.SocialNetworksLink;
        //            await socialNetworkService.UpdateSocialNetworkAsync(socialNetwork);
        //        }
        //        var UpdateResult = await userManager.UpdateAsync(user);
        //        if (UpdateResult.Succeeded)
        //        {
        //            result = true;
        //            message.Append("User profile updated successfully");
        //        }
        //        else
        //        {
        //            throw new Exception("User profile update failed");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        message.Append(ex.Message);
        //        await Console.Out.WriteLineAsync($"UpdateUserProfile: {ex.Message}");
        //    }

        //    return result;
        //} 
        #endregion

        public async Task<User?> UpdateUserProfile(UpdateUserProfileRequest updateUserProfileRequest, StringBuilder message)
        {
            User user = null!;
            try
            {
                user = await userManager.FindByIdAsync(updateUserProfileRequest.Id);
                user.FirstName = updateUserProfileRequest.FirstName;
                user.LastName = updateUserProfileRequest.LastName;
                user.BirthDate = updateUserProfileRequest.BirthDate;
                user.Image = await FileHelper.SaveImageAndReturnShortPathAsync(updateUserProfileRequest.LogoFile, FolderImage.UserImageUpload, user.Image) ?? user.Image;
                user.Gender = updateUserProfileRequest.Gender;
                user.Address = updateUserProfileRequest.Address;
                user.WorkPlace = updateUserProfileRequest.WorkPlace;
                user.Bio = updateUserProfileRequest.Bio;
                user.Education = updateUserProfileRequest.Education;
                user.Skill = updateUserProfileRequest.Skill;
                user.Hobby = updateUserProfileRequest.Hobby;
                user.Strength = updateUserProfileRequest.Strength;
                user.VolunteerExperience = updateUserProfileRequest.VolunteerExperience;
                user.VolunteerGoal = updateUserProfileRequest.VolunteerGoal;
                user.PhoneNumber = updateUserProfileRequest.PhoneNumber;
                updateUserProfileRequest.SocialNetworkRequests = JsonConvert.DeserializeObject<List<SocialNetworkRequest>>(updateUserProfileRequest.SocialNetworkRequestJsons);
                updateUserProfileRequest.sDGUserRequests = JsonConvert.DeserializeObject<List<SDGUserRequest>>(updateUserProfileRequest.SDGUserRequestJsons);
                foreach (var item in updateUserProfileRequest.SocialNetworkRequests)
                {

                    var socialNetwork = await socialNetworkService.GetSocialNetworkByIdAsync(item.Id.Value);

                    if (socialNetwork != null)
                    {
                        socialNetwork.SocialNetworksLink = item.SocialNetworksLink;
                        await socialNetworkService.UpdateSocialNetworkAsync(socialNetwork);
                    }
                    else
                    {
                        socialNetwork = new SocialNetwork()
                        {
                            UserId = updateUserProfileRequest.Id,
                            SocialNetworksLink = item.SocialNetworksLink
                        };
                        await socialNetworkService.CreateSocialNetworkAsync(socialNetwork);
                    }
                }
                foreach (var item in updateUserProfileRequest.sDGUserRequests)
                {
                    var userSdg = await userSDGService.GetUserSDGByUserIdAndSdgIdAsync(item.UserId, item.SDGId);
                    if (item.IsActive)// trạng thái add
                    {
                        if (userSdg == null)
                        {
                            userSdg = new UserSDG()
                            {
                                UserId = item.UserId,
                                SDGId = item.SDGId
                            };
                            await userSDGService.CreateUserSDGAsync(userSdg);
                        }
                    }
                    else// trạng thái xóa
                    {
                        if (userSdg != null)
                        {
                            await userSDGService.DeleteUserSDGAsync(userSdg);
                        }
                    }
                }
                var UpdateResult = await userManager.UpdateAsync(user);
                if (UpdateResult.Succeeded)
                {
                    message.Append("User profile updated successfully");
                }
                else
                {
                    throw new Exception("User profile update failed");
                }
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"UpdateUserProfile: {ex.Message}");
            }

            return user;
        }

        public async Task<bool> CheckValidExternalRegister(string roleName, StringBuilder message)
        {
            bool checkValid = false;
            try
            {
                userManager.AddLoginAsync(null, null);
                if (roleName == null)
                {
                    throw new Exception("Role Name is null");
                }
                if (!roleName.Equals(AppRole.Volunteer) && !roleName.Equals(AppRole.Donor))
                {
                    throw new Exception("Not Allowed To Register Other Roles Except Donor And Volunteer!");
                }
                checkValid = true;
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"CheckValidExternalRegister: {ex.Message}");
            }
            return checkValid;
        }
        public async Task<IdentityResult?> ExternalRegisterUser(Userinfo userInfo, string roleName)
        {
            try
            {
                var newUser = new User
                {
                    FirstName = userInfo.GivenName ?? string.Empty,
                    LastName = userInfo.FamilyName ?? string.Empty,
                    Email = userInfo.Email,
                    UserName = userInfo.Email,
                    TwoFactorEnabled = true,
                    Gender = userInfo.Gender != null ? false : true,
                    Image = userInfo.Picture,
                };
                var result = await userManager.CreateAsync(newUser);
                if (result.Succeeded)
                {
                    result = await userManager.AddToRoleAsync(newUser, roleName);
                    if (result.Succeeded)
                    {
                        var info = new UserLoginInfo(GoogleDefaults.AuthenticationScheme, userInfo.Id, userInfo.Name);
                        result = await userManager.AddLoginAsync(newUser, info);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ExternalRegisterUser: {ex.Message}");
            }


            return null;
        }
        public async Task<Userinfo> GetUserInfoAsync(string accessToken)
        {
            // Tạo credential từ access token
            var credential = GoogleCredential.FromAccessToken(accessToken);

            // Tạo dịch vụ OAuth2
            var oauth2Service = new Oauth2Service(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                //ApplicationName = "FALO"
            });
            // Lấy thông tin người dùng
            Userinfo userInfo = await oauth2Service.Userinfo.Get().ExecuteAsync();
            return userInfo;
        }
        public async Task<IdentityResult> LockUserAccountAsync(string userId, DateTime? lockoutEnd)
        {
            IdentityResult result = null!;
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.LockoutEnd = lockoutEnd;
                result = await userManager.UpdateAsync(user);
            }

            return result;
        }

        //public async Task AssignRoleToUserAsync(AddUserRole addUserRole)
        //{
        //    var user = await userManager.FindByIdAsync(addUserRole.UserId);
        //    if (user != null)
        //    {
        //        var roleExists = await roleManager.RoleExistsAsync(addUserRole.RoleName);
        //        if (roleExists)
        //        {
        //            await userManager.AddToRoleAsync(user, addUserRole.RoleName);
        //        }
        //    }
        //}

        public async Task<bool> AssignRoleToUserAsync(AddUserRole addUserRole, StringBuilder message)
        {
            User user = null!;
            bool result = false;
            try
            {
                user = await userManager.FindByIdAsync(addUserRole.UserId);
                if (user == null)
                {
                    throw new Exception("User not found in system!");
                }
                result = await userManager.AddToRolesAsync(user, addUserRole.RoleNames) == IdentityResult.Success;
                if (result)
                {
                    message.Append("Assign Role(s) successfully");
                }
                else
                {
                    message.Append("Assign Role(s) failed");
                }
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"UpdateUserRoleAsync: {ex.Message}");
            }

            return result;
        }

    }


}