using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Services.MoveNextCampaignStatusRequestServices
{
    public interface IMoveNextCampaignStatusRequestService
    {
        Task<bool> CreateMoveNextCampaignStatusRequestAsync(MoveNextCampaignStatusRequest createMoveNextCampaignStatusRequest);
        Task<MoveNextCampaignStatusRequest> GetMoveNextCampaignStatusRequestByIdAsync(int id);
        Task<IEnumerable<MoveNextCampaignStatusRequest>> GetAllMoveNextCampaignStatusRequestsAsync();
        Task<bool> UpdateMoveNextCampaignStatusRequestAsync(MoveNextCampaignStatusRequest updateMoveNextCampaignStatusRequest);
        Task<bool> DeleteMoveNextCampaignStatusRequestAsync(int id);
    }
}
