using FALOFinancialProofing.DTOs.CampaignDTO;
using FALOFinancialProofing.Models;
using System.Text;

namespace FALOFinancialProofing.Services.CampaignService
{
    public interface ICampaignService
    {
        Task<bool> CreateCampaignRequestAsync(CreateCampaignClientRequest createCampaignClientRequest);
        Task<List<CampaignInformation>> GetAllCampaignsAsync();
        Task<Campaign?> GetCampaignByIdAsync(int id);
        Task<Campaign?> CreateCampaignAsync(CreateCampaignDTO createCampaignDTO);
        Task<bool> UpdateCampaignAsync(UpdateCampaignDTO updateCampaignDTO);
        Task<bool> DeleteCampaignByIdAsync(int id);

        Task<bool> ValidateCampaignCreateAsync(CreateCampaignClientRequest createCampaignClientRequest, StringBuilder message);
        Task<Campaign> ConvertDtoToBaseClass(CreateCampaignClientRequest createCampaignClientRequest);
        Task<Campaign> CreateCampaignReturnEntityAsync(Campaign createCampaign);
        Task<List<Campaign>> GetAllCampaignByUserIdAndRoleAsync(string userId, string currentRole);

        Task<List<CampaignInformation>> GetAllCampaignsByProjectIdAsync(int ProjectId);
        Task<CampaignInformation> GetCampaignByCampaignIdAsync(int CampaignId);
        Task<Campaign?> UpdateEndDateForProjectManagerAsync(int campaignId, string userId, string currentRole, DateTime newDateTime, StringBuilder message);
        //Task<Campaign> CreateCampaignAsync(Campaign campaign);
        //Task<Campaign> GetCampaignByIdAsync(int campaignId);
        //Task<IEnumerable<Campaign>> GetAllCampaignsAsync();
        //Task UpdateCampaignAsync(Campaign campaign);
        //Task DeleteCampaignAsync(int campaignId);
    }
}
