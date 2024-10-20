﻿using FALOFinancialProofing.DTOs.CampaignDTO;
using FALOFinancialProofing.DTOs.CampaignMemberDTO;
using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Services
{
    public interface ICampaignMemberService
    {
        Task<List<CampaignMember>> GetAllCampaignMembersAsync();
        Task<CampaignMember?> GetCampaignMemberByCampaignIdAndUserIdAsync(int campaignId, string userId);
        Task<CampaignMember?> CreateCampaignMemberAsync(CreateCampaignMemberDTO createCampaignMemberDTO);
        Task<bool> UpdateCampaignMemberAsync(UpdateCampaignMemberDTO updateCampaignMemberDTO);
        Task<bool> DeleteCampaignMemberByCampaignIdAndUserIdAsync(int campaignId, string userId);
    }
}
