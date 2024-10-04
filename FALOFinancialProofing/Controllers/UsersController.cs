using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FALOFinancialProofing.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using FALOFinancialProofing.Repository;
using FALOFinancialProofing.DTOs;
using Microsoft.AspNetCore.Identity.Data;
using FALOFinancialProofing.Services;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AuthServices authServices ;

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
            } else
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Authenticate Success",
                    Data = await authServices.GenerateToken(user)
                });
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] SignUpRequest registerRequest)
        {
            var user = await authServices.RegisterUser(registerRequest);
            if (user == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Register Failed" 
                });
            } else
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Register Success",
                });
            }
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
            var user = await authServices.ResetPassword(resetPassword);
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
                    Message = "Password has been changed.",
                });
            }
        }


    }
}
