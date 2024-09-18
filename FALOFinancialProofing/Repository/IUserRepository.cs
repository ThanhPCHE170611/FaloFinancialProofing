using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<UserDto> GetUserByEmailAndPassword(string email, string password);
    }
}
