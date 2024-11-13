using FALOFinancialProofing.DTOs.SDGDTOs;
using FALOFinancialProofing.DTOs.UserSDGDTO;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace FALOFinancialProofing.Services.UserSDGServices
{
    public class UserSDGService : IUserSDGService
    {
        private readonly IRepository<UserSDG, int> _userSDGService;
        public UserSDGService(IRepository<UserSDG, int> userSDGService)
        {
            _userSDGService = userSDGService;
        }
        public Task<UserSDG> CreateUserSDGAsync(CreateUserSDG createUserSDG, StringBuilder message)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserSDGAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserSDG>> GetAllUserSDGsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserSDGInformation> GetUserSDGByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserSDGInformation>> GetUserSDGsByUserIdAsync(string userId)
        {
            List<UserSDGInformation> userSDGs = null!;
            try
            {
                // tìm userSDG mà User đã tham gia
                userSDGs = await _userSDGService.GetAll()
                    .Where(x => x.UserId == userId)
                    .Select(x => new UserSDGInformation
                    {
                        Id = x.Id,
                        UserId = x.UserId,
                        sDGInformation = new SDGInformation()
                        {
                            Id = x.SDG.Id,
                            SDGName = x.SDG.SDGName
                        }
                    }).ToListAsync();

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetUserSDGsByUserIdAsync: {ex.Message}");
            }

            return userSDGs;
        }

        public Task<bool> UpdateUserSDGAsync(UserSDG updateUserSDG)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateCreateUserSDGAsync(CreateUserSDG createUserSDG, StringBuilder message)
        {
            throw new NotImplementedException();
        }
    }
}
