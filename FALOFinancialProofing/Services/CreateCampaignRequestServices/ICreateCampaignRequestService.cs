using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Services.CreateCampaignRequestServices
{
    public interface ICreateCampaignRequestService
    {
        Task<bool> CreateCreateCampaignRequestAsync(CreateCampaignRequest createCampaignRequest);
        Task<CreateCampaignRequest> GetCreateCampaignRequestByIdAsync(int id);
        Task<IEnumerable<CreateCampaignRequest>> GetAllCreateCampaignRequestsAsync();
        Task<bool> UpdateCreateCampaignRequestAsync(CreateCampaignRequest updateCampaignRequest);
        Task<bool> DeleteCreateCampaignRequestAsync(int id);
    }
}
