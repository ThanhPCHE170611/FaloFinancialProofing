using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Services.VoucherServices
{
    public interface IVoucherServices
    {

        Task<List<Voucher>?> GetVouchersByApproveIdAsync(int approveId);

        Task<bool> CreateManyVoucherAsync(List<VoucherRequest> dtos);

        Task<bool> UpdateVoucherAsync(VoucherRequest dto);

        Task<bool> DeleteVoucherAsync(VoucherRequest dto);

        Task<bool> DeleteVoucherByIdAsync(int id);
    }
}
