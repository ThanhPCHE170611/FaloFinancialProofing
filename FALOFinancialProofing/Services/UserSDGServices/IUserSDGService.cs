using FALOFinancialProofing.DTOs.UserSDGDTO;
using FALOFinancialProofing.Models;
using System.Text;

namespace FALOFinancialProofing.Services.UserSDGServices
{
    public interface IUserSDGService
    {
        Task<UserSDG> CreateUserSDGAsync(CreateUserSDG createUserSDG, StringBuilder message);
        Task<UserSDGInformation> GetUserSDGByIdAsync(int id);
        Task<IEnumerable<UserSDG>> GetAllUserSDGsAsync();
        Task<List<UserSDGInformation>> GetUserSDGsByUserIdAsync(string userId);
        Task<bool> UpdateUserSDGAsync(UserSDG updateUserSDG);
        Task<bool> DeleteUserSDGAsync(int id);
        Task<bool> ValidateCreateUserSDGAsync(CreateUserSDG createUserSDG, StringBuilder message);
        Task<UserSDG> GetUserSDGByUserIdAndSdgIdAsync(string userId, int sdgId);
        Task<UserSDG> CreateUserSDGAsync(UserSDG userSdg);
        Task<bool> DeleteUserSDGAsync(UserSDG userSDG);
    }
}
