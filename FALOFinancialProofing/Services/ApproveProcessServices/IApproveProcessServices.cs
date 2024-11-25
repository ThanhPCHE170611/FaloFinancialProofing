using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;
using System.Text;

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
        Task<bool> ApproveRequestForLeader(string userid, string currentLoggingRole, int requestid, StringBuilder message);

        Task<bool> ApproveRequestForAccounting(string userid, string currentLoggingRole, int requestid, StringBuilder message);

        Task<bool> ApproveRequestForProjectManager(string userid, string currentLoggingRole, int requestid, StringBuilder message);
        Task<bool> RejectPrePayRequestForLeader(string userid, string currentLoggingRole, int requestid, StringBuilder message, string feedback, StringBuilder feedBackStringBuilder);
        Task<bool> RejectPrePayRequestForAccounting(string userid, string currentLoggingRole, int requestid, StringBuilder message, string feedback, StringBuilder feedBackStringBuilder);
        Task<bool> RejectPrePayRequestForProjectManager(string userid, string currentLoggingRole, int requestid, StringBuilder msg, string feedback, StringBuilder feedBackStringBuilder);

        Task<List<PrePayRequestFormViewRequest>?> GetAllPrepayRequestForVolunteerLeader(string userid, string currentRoleLoggedIn);
        Task<ApproveProcess?> GetApproveProcessesByRequestIdAndApproveIdAsync(int requestid, string userid);
        Task<List<PrePayRequestFormViewRequest>?> GetAllPrepayRequestForAccounting(string userid, string currentLoggingRole);
        Task<List<PrePayRequestFormViewRequest>?> GetAllPaymentRequestForVolunteerLeader(string userid, string currentLoggingRole);
        Task<List<PrePayRequestFormViewRequest>?> GetAllPaymentRequestForAccounting(string userid, string currentLoggingRole);
        Task<List<RequestFormViewRequestWithVoucherForPM>?> GetAllPrepayRequestForProjectManager(string userid, string currentLoggingRole);
        Task<List<RequestFormViewRequestWithVoucherForPM>?> GetAllPaymentRequestForProjectManager(string userid, string currentLoggingRole);
    }
}
