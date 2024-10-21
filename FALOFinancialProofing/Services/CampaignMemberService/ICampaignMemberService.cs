using FALOFinancialProofing.DTOs.CampaignDTO;
using FALOFinancialProofing.DTOs.CampaignMemberDTO;
using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Services
{
    public interface ICampaignMemberService
    {
        Task<List<CampaignMember>> GetAllCampaignMembersAsync();
        Task<CampaignMember?> GetCampaignMemberByIdAsync(int id);
        Task<CampaignMember?> CreateCampaignMemberAsync(CreateCampaignMemberDTO createCampaignMemberDTO);
        Task<bool> UpdateCampaignMemberAsync(UpdateCampaignMemberDTO updateCampaignMemberDTO);
        Task<bool> DeleteCampaignMemberByIdAsync(int id);
    }
}
