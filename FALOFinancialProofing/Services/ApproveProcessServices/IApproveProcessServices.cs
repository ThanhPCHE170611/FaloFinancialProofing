using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Services.ApproveProcessServices
{
    public interface IApproveProcessServices
    {
        Task<List<ApproveProcess>> GetAllApproveProcesssAsync();

        Task<ApproveProcess?> GetApproveProcessByIdAsync(int id);

        Task<List<ApproveProcess>?> GetApproveProcessesByRequestIdAsync(int requestId);

        Task<ApproveProcess?> CreateApproveProcessAsync(ApproveProcessRequest dto);

        Task<bool> UpdateApproveProcessAsync(ApproveProcessRequest dto);

        Task<bool> DeleteApproveProcessAsync(ApproveProcessRequest dto);

        Task<bool> DeleteApproveProcessByIdAsync(int id);
        Task<List<CreateFormRequest>?> GetAllPrepayRequestForVolunteerLeader(string userid, string currentRoleLoggedIn);
    }
}
