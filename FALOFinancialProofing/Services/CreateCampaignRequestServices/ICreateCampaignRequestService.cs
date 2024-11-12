using FALOFinancialProofing.DTOs.CreateCampaignRequestDTO;
using FALOFinancialProofing.DTOs.CreateProjectRequestDTO;
using FALOFinancialProofing.Models;
using System.Text;

namespace FALOFinancialProofing.Services.CreateCampaignRequestServices
{
    public interface ICreateCampaignRequestService
    {
        Task<bool> CreateCreateCampaignRequestAsync(CreateCampaignRequest createCampaignRequest);
        Task<CreateCampaignRequestInformation> GetCreateCampaignRequestByIdAsync(int id);
        Task<IEnumerable<CreateCampaignRequest>> GetAllCreateCampaignRequestsAsync();
        Task<bool> UpdateCreateCampaignRequestAsync(CreateCampaignRequest updateCampaignRequest);
        Task<bool> DeleteCreateCampaignRequestAsync(int id);
        Task<CreateCampaignRequest> CreateCreateCampaignRequestReturnEntityAsync(CreateCampaignRequest createCampaignRequest);
        Task<IEnumerable<CreateCampaignRequestInformation>> GetAllCreateCampaignRequestsByPMBAsync(StringBuilder message);
        Task<IEnumerable<CreateCampaignRequestInformation>> GetAllCreateCampaignRequestsByUserIdAsync(string userId, StringBuilder message);
        Task<bool> ValidateCreateCampaignRequestByUserIdAndRequestIdAsync(string userId, int CreateCampaignRequestId, StringBuilder message);
        Task<bool> CancelCreateCampaignRequestAsync(string userId, int CreateCampaignRequestId);
    }
}
