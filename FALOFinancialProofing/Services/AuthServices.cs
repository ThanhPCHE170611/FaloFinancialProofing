using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.AspNetCore.Identity.Data;

namespace FALOFinancialProofing.Services
{
    public class AuthServices
    {
        private readonly IRepository<User> repository;
        public AuthServices(IRepository<User> _repository)
        {
            this.repository = _repository;
        }

        public async Task<UserDto?> LoginUser(LoginModel userLogin)
        {
            var user = await repository.Get(u => u.Email.Equals(userLogin.Email) && u.Password.Equals(userLogin.Password));
            return user == null ? null : new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password,
                FullName = user.FullName,
                RoleName = null
            };
            //Todo: đang để RoleName là string? tại vì bị null, sửa đi nhé
        }

        public async Task<User?> RegisterUser(SignUpRequest registerRequest)
        {

            var validatedInformationRequest = await ValidatedInformationRequest(registerRequest);
            if(validatedInformationRequest == null)
            {
                return null;
            } else
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
