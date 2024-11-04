using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Services.AccountingBookServices
{
    public interface IAcccountingBookServices
    {
        Task<AccountingBook?> CreateAccountingRequest(CreateAccountingBookRequest request, System.Text.StringBuilder errorMessages);
        Task<(byte[] fileBytes, string contentType, string downloadFileName)> DownloadAccountingBookFileByFileName(string fileName);
        Task<AccountingBook?> GetAccountingBookInCampaign(int campaignId);
        Task<List<AccountingBook>> GetAllAccountingBookInProject(int projectId);
    }
}
