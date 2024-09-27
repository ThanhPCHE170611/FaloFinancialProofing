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
    }
}
