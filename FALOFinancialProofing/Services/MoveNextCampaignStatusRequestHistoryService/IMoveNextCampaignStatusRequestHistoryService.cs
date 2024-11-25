using FALOFinancialProofing.DTOs.MoveNextCampaignStatusRequestHistoryDTO;
using FALOFinancialProofing.Models;
using System.Text;

namespace FALOFinancialProofing.Services.MoveNextCampaignStatusRequestHistoryService
{
    public interface IMoveNextCampaignStatusRequestHistoryService
    {
        Task<bool> CreateMoveNextCampaignStatusRequestHistoryAsync(MoveNextCampaignStatusRequestHistoryDTO moveNextCampaignStatusRequestHistoryDTO, StringBuilder message);
        Task<MoveNextCampaignStatusRequestHistory> GetMoveNextCampaignStatusRequestHistoryByIdAsync(int id);
        Task<IEnumerable<MoveNextCampaignStatusRequestHistory>> GetAllMoveNextCampaignStatusRequestHistoriesAsync();
        Task<bool> UpdateMoveNextCampaignStatusRequestHistoryAsync(MoveNextCampaignStatusRequestHistory updateMoveNextCampaignStatusRequestHistory);
        Task<bool> DeleteMoveNextCampaignStatusRequestHistoryAsync(int id);
        Task<bool> CheckValidMoveNextCampaignStatusRequestHistory(MoveNextCampaignStatusRequestHistoryDTO moveNextCampaignStatusRequestHistoryDTO, StringBuilder message);

    }
}
