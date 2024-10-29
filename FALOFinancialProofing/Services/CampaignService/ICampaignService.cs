using FALOFinancialProofing.DTOs.CampaignDTO;
using FALOFinancialProofing.Models;
using System.Text;

namespace FALOFinancialProofing.Services.CampaignService
{
    public interface ICampaignService
    {
        Task<bool> CreateCampaignRequestAsync(CreateCampaignClientRequest createCampaignClientRequest);
        Task<List<Campaign>> GetAllCampaignsAsync();
        Task<Campaign?> GetCampaignByIdAsync(int id);
        Task<Campaign?> CreateCampaignAsync(CreateCampaignDTO createCampaignDTO);
        Task<bool> UpdateCampaignAsync(UpdateCampaignDTO updateCampaignDTO);
        Task<bool> DeleteCampaignByIdAsync(int id);

        Task<bool> ValidateCampaignCreateAsync(CreateCampaignClientRequest createCampaignClientRequest, StringBuilder message);
        Task<Campaign> ConvertDtoToBaseClass(CreateCampaignClientRequest createCampaignClientRequest);
        Task<Campaign> CreateCampaignReturnEntityAsync(Campaign createCampaign);

        //Task<Campaign> CreateCampaignAsync(Campaign campaign);
        //Task<Campaign> GetCampaignByIdAsync(int campaignId);
        //Task<IEnumerable<Campaign>> GetAllCampaignsAsync();
        //Task UpdateCampaignAsync(Campaign campaign);
        //Task DeleteCampaignAsync(int campaignId);
    }
}
