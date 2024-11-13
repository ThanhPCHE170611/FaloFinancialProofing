using FALOFinancialProofing.DTOs.CampaignDTO;
using FALOFinancialProofing.DTOs.CampaignMemberDTO;
using FALOFinancialProofing.Models;
using System.Text;

namespace FALOFinancialProofing.Services
{
    public interface ICampaignMemberService
    {
        Task<List<CampaignMemberInformation>> GetAllCampaignMembersAsync();
        Task<CampaignMemberInformation?> GetCampaignMemberByIdAsync(int id);
        Task<CampaignMember?> GetCampaignMemberByUserIdAsync(string userid);

        Task<CampaignMember?> CreateCampaignMemberAsync(CreateCampaignMemberDTO createCampaignMemberDTO);
        Task<CampaignMember?> CreateCampaignMemberAsync(CampaignMember campaignMember);
        Task<bool> UpdateCampaignMemberAsync(UpdateCampaignMemberDTO updateCampaignMemberDTO);
        //Task<bool> UpdateCampaignMemberAsync(CampaignMember campaignMember);
        Task<bool> DeleteCampaignMemberByIdAsync(int id);
        Task<bool> CreateManyCampaignMembersAsync(List<CreateManyCampaignMemberDTO> createManyCampaignMemberDTOs, int campaignId, StringBuilder message);

        Task<List<CreateManyCampaignMemberDTO>> ValidateCampaignMembersCreateAsync(List<CreateManyCampaignMemberDTO> createManyCampaignMemberDTOs, int campaignId, StringBuilder message);
        Task<List<CreateManyCampaignMemberDTO>> InValidCampaignMembersCreateAsync(List<CreateManyCampaignMemberDTO> createManyCampaignMemberDTOs, List<CreateManyCampaignMemberDTO> ValidCreateManyCampaignMemberDTOs);
        Task<bool> UpdateCampaignMemberStatusAsync(UpdateCampaignMemberStatusDTO updateCampaignMemberStatusDTO, StringBuilder message);
        Task<List<CampaignMemberInformation>> GetAllCampaignMemberByUserIdAndRoleIdAsync(string userId, string roleId);
        Task<List<CampaignMemberInformation>> GetAllCampaignMemberByCampaignIdAsync(int campaignId);
        Task<List<CampaignMemberInformation>> GetAllCampaignMemberByUserIdAsync(string userId);
    }
}
