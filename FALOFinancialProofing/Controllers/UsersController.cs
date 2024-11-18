using FALOFinancialProofing.Attributes.RoleAttributes;
using FALOFinancialProofing.Constant;
using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.DTOs.UserDTOs;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services;
using FALOFinancialProofing.Utilities;
using Google.Apis.Oauth2.v2.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AuthServices authServices;

        public UsersController(AuthServices _authServices)
        {
            authServices = _authServices;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Post([FromBody] SignInModel userLogin)
        {
            var user = await authServices.LoginUser(userLogin);
            if (user == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Invalid Username/Password"
                });
            }
            else
            {
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Authenticate Success",
                    Data = await authServices.GenerateToken(user)
                });
            }
        }

        [HttpPost("Login-Google/{roleName}")]
        public async Task<IActionResult> LoginGoogle([FromHeader(Name = "Authorization")] string accessToken, string roleName)
        {
            accessToken = accessToken.Replace("AccessToken ", "");
            StringBuilder message = new StringBuilder();
            var checkValid = await authServices.CheckValidExternalRegister(roleName, message);
            if (!checkValid)
            {
                return Ok(new ApiResponse()
                {
                    Success = checkValid,
                    Message = message.ToString()
                });
            }
            Userinfo userInfo = await authServices.GetUserInfoAsync(accessToken);
            if (userInfo == null)
            {
                return Ok(new ApiResponse()
                {
                    Success = false,
                    Message = "Login with Google Fails, No access to account!"
                });
            }
            var checkAccountExist = await authServices.CheckGoogleExistAccount(userInfo.Email);
            if (!checkAccountExist)
            {
                var checkSuccessCreate = await authServices.ExternalRegisterUser(userInfo, roleName);
                if (checkSuccessCreate == null || !checkSuccessCreate.Succeeded)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = "Login with Google Fails, No access to account!"
                    });
                }
            }
            UserDto userDto = await authServices.GetUserDto(userInfo);
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Google Authentication Success",
                Data = await authServices.GenerateToken(userDto)
            });
        }
        [RoleAttribute(AppRole.Admin)]
        [HttpPut("Update-User-Role")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRole updateUserRole)
        {

            StringBuilder message = new StringBuilder();
            var isValid = await authServices.UpdateUserRoleAsync(updateUserRole, message);
            return Ok(new
            {
                Success = isValid,
                Message = message.ToString()
            });


        }


        [Authorize]
        // hiển thị thông tin danh sách người dùng không ở trong một chiến dịch cụ thể
        [HttpGet("GetUserNotInCampaignById/{CampaignId}")]
        public async Task<IActionResult> GetUserNotInCampaignById(string? searchInput, int CampaignId)
        {
            var users = await authServices.GetUserNotInCampaignById(CampaignId);
            if (!string.IsNullOrEmpty(searchInput))
            {
                searchInput = searchInput.Trim();
                users = users.FindAll(x => ($"{x.FirstName} {x.LastName}").Contains(searchInput, StringComparison.OrdinalIgnoreCase) || ($"{x.Email}").Contains(searchInput, StringComparison.OrdinalIgnoreCase));
            }
            return Ok(new ApiResponse()
            {
                Message = "Get Users Successfully!",
                Data = users,
                Success = true
            });
        }


        [Authorize]
        [HttpGet("SetRoleLoginAfterLogin/{userRole}")]
        public IActionResult SetRoleLogin(string userRole)
        {
            HttpContext.Session.SetString("userRole", userRole);
            return Ok(new ApiResponse()
            {
                Success = true,
                Message = "Role Set Success!"
            }); ;
        }

        [Authorize]
        [HttpGet("GetRoleLoginAfterLogin")]
        public async Task<IActionResult> GetRoleLogin()
        {
            bool status = true;
            string message = "Role Set Success";
            try
            {
                var userRole = HttpContext.Session.GetString("userRole");
                if (string.IsNullOrEmpty(userRole))
                {
                    message = "Must Login To Get User Role!";
                    status = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetRoleLogin: {ex.Message}");
            }
            return Ok(new ApiResponse()
            {
                Success = status,
                Message = message
            });
        }
        #region Xóa sau
        [HttpGet("loginGG")]
        public IActionResult Login()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        [HttpGet("signin-google")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return BadRequest();

            var claims = result.Principal.Identities
                .FirstOrDefault()?.Claims.Select(claim => new
                {
                    claim.Type,
                    claim.Value
                });

            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = await authServices.userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new User { UserName = email, Email = email };
                var createResult = await authServices.userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                    return BadRequest(createResult.Errors);

                var info = new UserLoginInfo(GoogleDefaults.AuthenticationScheme, result.Principal.FindFirstValue(ClaimTypes.NameIdentifier), "Google");
                var addLoginResult = await authServices.userManager.AddLoginAsync(user, info);
                if (!addLoginResult.Succeeded)
                    return BadRequest(addLoginResult.Errors);
            }
            //var token = GenerateJwtToken(user);

            //return Ok(new { token });
            return Ok();
        }
        #endregion
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] SignUpRequest registerRequest)
        {
            var user = await authServices.RegisterUser(registerRequest);
            if (user == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Register Failed: Email or Username is exist"
                });
            }
            else
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Register Success",
                });
            }
        }

        [HttpPost("Donor-Register")]
        public async Task<IActionResult> DonorRegister([FromBody] SignUpRequest registerRequest)
        {
            var user = await authServices.RegisterDonor(registerRequest);
            if (user == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Register Failed"
                });
            }
            else
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Register Success",
                });
            }
        }
        [RoleAttribute(AppRole.Admin)]
        [HttpPost("Admin-Register")]
        public async Task<IActionResult> AdminRegister([FromBody] SignUpAdminRequest registerRequest)
        {
            var user = await authServices.AdminRegisterUser(registerRequest);
            if (user == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Register Failed"
                });
            }
            else
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Register Success",
                });
            }
        }
        [RoleAttribute(AppRole.Admin)]
        [HttpGet("GetAccountList")]
        public async Task<IActionResult> GetAllAccountInSystem(string? searchInput, int currentPage = IntConstant.PageNumberDefault)
        {
            List<UserInformation_Admin> data = null;
            FilterPagingData filterPagingData = new FilterPagingData();
            filterPagingData.CurrentPage = currentPage;
            try
            {
                data = await authServices.GetAccountList();
                if (data == null || data.Count == 0)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = "Get All Account Failed!",
                        Data = filterPagingData
                    });
                }
                if (!string.IsNullOrEmpty(searchInput))
                {
                    searchInput = searchInput.Trim();
                    data = data.FindAll(x => ($"{x.FirstName} {x.LastName}").Contains(searchInput, StringComparison.OrdinalIgnoreCase) || ($"{x.Email}").Contains(searchInput, StringComparison.OrdinalIgnoreCase) || x.Roles.Any(rl => rl.RoleName.Contains(searchInput, StringComparison.OrdinalIgnoreCase)));
                }

                filterPagingData.DataCount = data.Count;
                data = PaginationHelper.Paginate<UserInformation_Admin>(data.AsQueryable(), currentPage, IntConstant.PageSize).ToList();
                filterPagingData.Data = data;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllAccountInSystem: {ex.Message}");
            }
            return Ok(new ApiResponse()
            {
                Success = true,
                Message = "Get All Accounts In System Successfully!",
                Data = filterPagingData
            });
        }

        [HttpGet("GetPMBAccountList")]
        public async Task<IActionResult> GetAllPMBAccountInSystem(string? searchInput, int currentPage = IntConstant.PageNumberDefault)
        {
            List<UserInformation_Admin> data = null;
            FilterPagingData filterPagingData = new FilterPagingData();
            filterPagingData.CurrentPage = currentPage;
            try
            {
                data = await authServices.GetPMBAccountList();
                if (data == null || data.Count == 0)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = "Get All PMB Account Failed!",
                        Data = filterPagingData
                    });
                }
                if (!string.IsNullOrEmpty(searchInput))
                {
                    searchInput = searchInput.Trim();
                    data = data.FindAll(x => ($"{x.FirstName} {x.LastName}").Contains(searchInput, StringComparison.OrdinalIgnoreCase) || ($"{x.Email}").Contains(searchInput, StringComparison.OrdinalIgnoreCase));
                }

                filterPagingData.DataCount = data.Count;
                data = PaginationHelper.Paginate<UserInformation_Admin>(data.AsQueryable(), currentPage, IntConstant.PageSize).ToList();
                filterPagingData.Data = data;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"Get All PMB Account In System: {ex.Message}");
            }
            return Ok(new ApiResponse()
            {
                Success = true,
                Message = "Get All PMB Accounts In System Successfully!",
                Data = filterPagingData
            });
        }
        [RoleAttribute(AppRole.Admin)]
        [HttpGet("GetAccount/{UserId}")]
        public async Task<IActionResult> GetAccount(string UserId)
        {
            StringBuilder message = new StringBuilder();
            UserInformation_Admin data = null;
            try
            {
                data = await authServices.GetAccount(UserId, message);
                if (data == null)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = message.ToString(),
                        Data = data
                    });
                }
                message.Append("Get Account Successfully!");

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllAccountInSystem: {ex.Message}");
            }
            return Ok(new ApiResponse()
            {
                Success = true,
                Message = message.ToString(),
                Data = data
            });
        }
        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([Required] string email)
        {
            var user = await authServices.ForgotPassword(email, HttpContext);
            if (user == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Could not send link to email. Email is not registered, or you have entered the wrong email address"
                });
            }
            else
            {
                return Ok(new
                {
                    Success = true,
                    Message = " Password Changed request is sent on Email . Please Open your email & click the link.",
                });
            }
        }
        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var model = new ResetPassword { Token = token, Email = email };

            return Ok(new
            {
                model
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([Required] ResetPassword resetPassword)
        {
            StringBuilder stringBuilderMessage = new StringBuilder();
            bool checkValidateProject = await authServices.ValidateResetPasswordAsync(resetPassword, stringBuilderMessage);
            if (!checkValidateProject)
            {
                return Ok(new
                {
                    Success = false,
                    Message = stringBuilderMessage.ToString()
                });
            }

            //var user = await authServices.ResetPassword(resetPassword);
            //if (user == null)
            //{
            //    return Ok(new
            //    {
            //        Success = false,
            //        Message = "Could not send link to email. Email is not registered, or you have entered the wrong email address"
            //    });
            //}
            //else

            var user = await authServices.ResetPassword(resetPassword);
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Password has been changed.",
                });
            }
        }

        [HttpGet("getuserdebincampaign")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserDebInCampaign(string userId, int campaignId)
        {
            var message = new StringBuilder();
            var userDeb = await authServices.GetUserDebInCampaign(userId, campaignId, message);
            if (userDeb == Double.MinValue)
            {
                return Ok(new
                {
                    Success = false,
                    Message = message.ToString()
                });
            }
            return Ok(new
            {
                Message = "Get Debt Successfully!",
                Data = userDeb,
                Success = true
            });
        }
        // ấn vào profile rồi hiển thị chi tiết
        [HttpGet("GetUserProfile/{UserId}")]
        public async Task<IActionResult> GetUserProfile(string UserId)
        {
            StringBuilder message = new StringBuilder();
            UserProfileDetail data = null;
            try
            {
                data = await authServices.GetUserProfile(UserId, message, Request);
                if (data == null)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = message.ToString(),
                        Data = data
                    });
                }
                message.Append("Get User Profile Successfully!");

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"Get User Profile: {ex.Message}");
            }
            return Ok(new ApiResponse()
            {
                Success = true,
                Message = message.ToString(),
                Data = data
            });
        }

        [HttpPost("UpdateUserProfile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateUserProfileRequest updateUserProfileRequest)
        {

            if (updateUserProfileRequest == null)
                return Ok(new ApiResponse()
                {
                    Success = false,
                    Message = "Update Profile Failed!"
                });
            StringBuilder message = new StringBuilder();
            bool checkValid = await authServices.CheckValidUser(updateUserProfileRequest, message);
            if (!checkValid)
            {
                return Ok(new ApiResponse()
                {
                    Success = checkValid,
                    Message = message.ToString()
                });
            }
            var UserUpdated = await authServices.UpdateUserProfile(updateUserProfileRequest, message);
            return Ok(new ApiResponse()
            {
                Success = true,
                Message = "Update Profile successfully"
            });

        }

    }
}
