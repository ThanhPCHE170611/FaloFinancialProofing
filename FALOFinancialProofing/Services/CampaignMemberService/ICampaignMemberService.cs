using FALOFinancialProofing.DTOs.CampaignDTO;
using FALOFinancialProofing.DTOs.CampaignMemberDTO;
using FALOFinancialProofing.Models;
using System.Text;

namespace FALOFinancialProofing.Services
{
    public interface ICampaignMemberService
    {
        Task<List<CampaignMember>> GetAllCampaignMembersAsync();
        Task<CampaignMember?> GetCampaignMemberByIdAsync(int id);
        Task<CampaignMember?> GetCampaignMemberByUserIdAsync(string userid);

        Task<CampaignMember?> CreateCampaignMemberAsync(CreateCampaignMemberDTO createCampaignMemberDTO);
        Task<bool> UpdateCampaignMemberAsync(UpdateCampaignMemberDTO updateCampaignMemberDTO);
        Task<bool> DeleteCampaignMemberByIdAsync(int id);
        Task<bool> CreateManyCampaignMembersAsync(List<CreateManyCampaignMemberDTO> createManyCampaignMemberDTOs, int campaignId, StringBuilder message);

        Task<List<CreateManyCampaignMemberDTO>> ValidateCampaignMembersCreateAsync(List<CreateManyCampaignMemberDTO> createManyCampaignMemberDTOs, int campaignId, StringBuilder message);
        Task<List<CreateManyCampaignMemberDTO>> InValidCampaignMembersCreateAsync(List<CreateManyCampaignMemberDTO> createManyCampaignMemberDTOs, List<CreateManyCampaignMemberDTO> ValidCreateManyCampaignMemberDTOs);
        Task<bool> UpdateCampaignMemberStatusAsync(UpdateCampaignMemberStatusDTO updateCampaignMemberStatusDTO);
    }
}
