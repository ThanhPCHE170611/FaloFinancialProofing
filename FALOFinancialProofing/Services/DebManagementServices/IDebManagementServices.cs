
using FALOFinancialProofing.DTOs;
using System.Text;

namespace FALOFinancialProofing.Services.DebManagementServices
{
    public interface IDebManagementServices
    {
        Task<List<UserWithDeb>> GetDebManagementForAccounting(string userId, string currentLoggingRole, StringBuilder message, int campaignId);
        Task<List<UserWithDeb>> GetDebManagementForPMB(string userId, string currentLoggingRole, System.Text.StringBuilder message);
    }
}
