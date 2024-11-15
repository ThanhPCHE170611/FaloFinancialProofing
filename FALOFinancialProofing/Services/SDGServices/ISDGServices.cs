using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.DTOs.SDGDTOs;
using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Services.SDGServices
{
    public interface ISDGServices
    {
        Task<List<SDGInformation>> GetAllSDGsAsync();

        Task<SDG?> GetSDGByIdAsync(int id);

        Task<List<SDG>?> GetSDGByUserIdAsync(string userId);

        Task<SDG?> CreateSDGAsync(SDGRequest sdg);

        Task<bool> UpdateSDGAsync(SDGRequest sdg);

        Task<bool> DeleteSDGAsync(SDGRequest sdg);

        Task<bool> DeleteSDGByIdAsync(int id);
    }
}
