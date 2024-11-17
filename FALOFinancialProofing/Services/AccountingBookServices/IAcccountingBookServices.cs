using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;
using System.Text;

namespace FALOFinancialProofing.Services.AccountingBookServices
{
    public interface IAcccountingBookServices
    {
        Task<AccountingBook?> CreateAccountingRequest(CreateAccountingBookRequest request, System.Text.StringBuilder errorMessages);
        Task<bool> DeleteAccountingBook(string fileName, StringBuilder message);
        Task<(byte[] fileBytes, string contentType, string downloadFileName)> DownloadAccountingBookFileByFileName(string fileName);
        Task<AccountingBook?> GetAccountingBookInCampaign(int campaignId);
        Task<List<AccountingBook>> GetAllAccountingBookInProject(int projectId);
    }
}
