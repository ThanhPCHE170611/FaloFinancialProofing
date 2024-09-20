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
        private readonly AppSetting _appSetting;
        //private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly AuthServices authServices ;

        public UsersController(IOptionsMonitor<AppSetting> optionsMonitor, AuthServices _authServices)
        {
            _appSetting = optionsMonitor.CurrentValue;
            //_refreshTokenRepository = refreshTokenRepository;
            authServices = _authServices;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Post([FromBody] LoginModel userLogin)
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
                    Data = GenerateToken(user)
                });
            }
            ////var user = _db.NguoiDungs.SingleOrDefault(n => n == User.Email && nguoiDung.PassWord == n.PassWord);
            //var user = await _userRepository.GetUserByEmailAndPassword(userLogin.Email, userLogin.Password);
            //if (user == null)
            //{
            //    return Ok(new
            //    {
            //        Success = false,
            //        Message = "Invalid Username/Password"
            //    });
            //}
            //return Ok(new
            //{
            //    Success = true,
            //    Message = "Authenticate Success",
            //    Data = GenerateToken(user)
            //});
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


        //// GET: api/Users
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        //{
        //    return await _context.Users.ToListAsync();
        //}

        //// GET: api/Users/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<User>> GetUser(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return user;
        //}

        //// PUT: api/Users/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUser(int id, User user)
        //{
        //    if (id != user.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Users
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<User>> PostUser(User user)
        //{
        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetUser", new { id = user.Id }, user);
        //}

        //// DELETE: api/Users/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Users.Remove(user);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool UserExists(int id)
        //{
        //    return _context.Users.Any(e => e.Id == id);
        //}

        private async Task<TokenModel> GenerateToken(UserDto User)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSetting.SecretKey);
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
                Expires = DateTime.UtcNow.AddDays(_appSetting.ExpiryInDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature),
                Issuer = _appSetting.Issuer,
                Audience = _appSetting.Audience,

            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);
            var refeshToken = GenerateRefeshToken();
            var refeshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = refeshToken,
                JwtId = token.Id,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                IsUsed = false
            };
            //await _refreshTokenRepository.InsertAsync(refeshTokenEntity);
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
