using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Repository
{
    public interface IUserRepository : IRepository<User,int>
    {
        Task<UserDto> GetUserByEmailAndPassword(string email, string password);
    }
}
