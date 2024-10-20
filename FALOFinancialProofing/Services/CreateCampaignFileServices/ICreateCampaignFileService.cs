using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Services.CreateCampaignFileServices
{
    public interface ICreateCampaignFileService
    {
        Task<bool> CreateCreateCampaignFileAsync(CreateCampaignFile createCampaignFile);
        Task<CreateCampaignFile> GetCreateCampaignFileByIdAsync(int id);
        Task<IEnumerable<CreateCampaignFile>> GetAllCreateCampaignFilesAsync();
        Task<bool> UpdateCreateCampaignFileAsync(CreateCampaignFile updateCampaignFile);
        Task<bool> DeleteCreateCampaignFileAsync(int id);
    }
}
