using FALOFinancialProofing.DTOs.CampaignDTO;
using FALOFinancialProofing.DTOs.ProjectDTOs;
using FALOFinancialProofing.Models;
using System.Text;

namespace FALOFinancialProofing.Services.CampaignService
{
    public interface ICampaignService
    {
        Task<bool> CreateCampaignRequestAsync(CreateCampaignClientRequest createCampaignClientRequest);
        Task<List<CampaignInformation>> GetAllCampaignsAsync(HttpRequest request);
        Task<Campaign?> GetCampaignByIdAsync(int id);
        Task<Campaign?> CreateCampaignAsync(CreateCampaignDTO createCampaignDTO);
        Task<bool> DeleteCampaignByIdAsync(int id);
        Task<bool> ValidateCampaignCreateAsync(CreateCampaignClientRequest createCampaignClientRequest, StringBuilder message);
        Task<Campaign> ConvertDtoToBaseClass(CreateCampaignClientRequest createCampaignClientRequest);
        Task<Campaign> CreateCampaignReturnEntityAsync(Campaign createCampaign);
        Task<List<Campaign>> GetAllCampaignByUserIdAndRoleAsync(string userId, string currentRole);
        Task<List<CampaignInformation>> GetAllCampaignsByProjectIdAsync(int ProjectId, HttpRequest request);
        Task<List<Campaign>> GetAllCampaignsByProjectIdAsync(int ProjectId);
        Task<CampaignInformation> GetCampaignByCampaignIdAsync(int CampaignId, HttpRequest request);
        Task<Campaign?> UpdateEndDateForProjectManagerAsync(int campaignId, string userId, string currentRole, DateTime newDateTime, StringBuilder message);
        Task<Campaign> GetCampaignByCampaignIdAsync(int CampaignId);
        Task<bool> ValidateCampaignUpdateAsync(UpdateCampaignDTO updateCampaignDTO, StringBuilder message);
        Task<bool> UpdateCampaignAsync(UpdateCampaignDTO updateCampaignDTO, StringBuilder message);
        Task<Campaign> GetCampaignByUserIdAndCampaignIdAsync(string pmUserId, int campaignId);
        Task<bool> UpdateManyCampaignAsync(List<Campaign> campaigns);
        //Task<Campaign> CreateCampaignAsync(Campaign campaign);
        //Task<Campaign> GetCampaignByIdAsync(int campaignId);
        //Task<IEnumerable<Campaign>> GetAllCampaignsAsync();
        //Task UpdateCampaignAsync(Campaign campaign);
        //Task DeleteCampaignAsync(int campaignId);
    }
}
