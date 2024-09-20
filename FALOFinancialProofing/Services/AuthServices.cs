using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace FALOFinancialProofing.Services
{
    public class AuthServices
    {
        private readonly IRepository<User, int> repository;

        public AuthServices(IRepository<User, int> _repository)
        {
            this.repository = _repository;
        }

        public async Task<UserDto?> LoginUser(LoginModel userLogin)
        {
            var user = await repository
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
                await repository.InsertAsync(newUser);
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
    }
}