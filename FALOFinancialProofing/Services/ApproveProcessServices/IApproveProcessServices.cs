using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;
using System.Threading.Tasks;

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
        Task<List<PrePayRequestFormViewRequest>?> GetAllPrepayRequestForVolunteerLeader(string userid, string currentRoleLoggedIn);
        Task<bool> ApprovePrePayRequestForLeader(string userid, string currentLoggingRole, int requestid);

        Task<bool> ApprovePrePayRequestForAccounting(string userid, string currentLoggingRole, int requestid);

        Task<bool> ApprovePrePayRequestForProjectManager(string userid, string currentLoggingRole, int requestid);
        Task<bool> RejectPrePayRequestForLeader(string userid, string currentLoggingRole, int requestid);
        Task<bool> RejectPrePayRequestForAccounting(string userid, string currentLoggingRole, int requestid);
        Task<bool> RejectPrePayRequestForProjectManager(string userid, string currentLoggingRole, int requestid);
        Task<ApproveProcess?> GetApproveProcessesByRequestIdAndApproveIdAsync(int requestid, string userid);
        Task<List<PrePayRequestFormViewRequest>?> GetAllPrepayRequestForAccounting(string userid, string currentLoggingRole);
        Task<List<PrePayRequestFormViewRequestWithVoucherForPM>?> GetAllPrepayRequestForProjectManager(string userid, string currentLoggingRole);
        
    }
}
