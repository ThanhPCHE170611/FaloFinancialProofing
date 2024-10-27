using FALOFinancialProofing.DTOs.CampaignRequestApproveHistoryDTO;
using FALOFinancialProofing.Models;
using System.Text;

namespace FALOFinancialProofing.Services.CampaignRequestApproveHistoryServices
{
    public interface ICampaignRequestApproveHistoryService
    {
        Task<bool> CreateCampaignRequestApproveHistoryAsync(CampaignRequestApproveHistoryClientRequest campaignRequestApproveHistoryClientRequest, StringBuilder message);
        Task<CampaignRequestApproveHistory> GetCampaignRequestApproveHistoryByIdAsync(int id);
        Task<IEnumerable<CampaignRequestApproveHistory>> GetAllCampaignRequestApproveHistoriesAsync();
        Task<bool> UpdateCampaignRequestApproveHistoryAsync(CampaignRequestApproveHistory updateCampaignRequestApproveHistory);
        Task<bool> DeleteCampaignRequestApproveHistoryAsync(int id);
        Task<bool> CheckValidCampaignRequestApproveHistory(CampaignRequestApproveHistoryClientRequest campaignRequestApproveHistoryClientRequest, StringBuilder message);
    }
}
