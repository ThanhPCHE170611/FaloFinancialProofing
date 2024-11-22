using FALOFinancialProofing.DTOs.CreateQrCodeDTO;
using FALOFinancialProofing.Models;
using System.Text;

namespace FALOFinancialProofing.Services.CreateQrCodeServices
{
    public interface ICreateQrCodeService
    {
        Task<bool> CreateQrCodeAsync(CreateQrCode createQrCode);
        Task<CreateQrCode> GetQrCodeByIdAsync(int id);
        Task<IEnumerable<CreateQrCode>> GetAllCreateQrCodesAsync();
        Task<bool> UpdateCreateQrCodeAsync(CreateQrCode createQrCode);
        Task<bool> DeleteCreateQrCodeAsync(int id);
        Task<bool> ValidateQrCodeCreate(CreateQrCodeRequest createQrCodeRequest, StringBuilder message);
    }
}
