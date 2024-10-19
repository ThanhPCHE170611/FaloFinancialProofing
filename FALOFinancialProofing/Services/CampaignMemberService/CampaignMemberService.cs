using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.DTOs.CampaignDTO;
using FALOFinancialProofing.DTOs.CampaignMemberDTO;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.EntityFrameworkCore;

namespace FALOFinancialProofing.Services.CampaignMemberService
{
    public class CampaignMemberService : ICampaignMemberService
    {
        private readonly IRepository<CampaignMember, int> cmRepository;

        public CampaignMemberService(IRepository<CampaignMember, int> _cmRepository)
        {
            cmRepository = _cmRepository;
        }

        public async Task<CampaignMember?> CreateCampaignMemberAsync(CreateCampaignMemberDTO createCampaignMemberDTO)
        {
            try
            {
                var newCampaignMember= await CreateCampaignMemberDTOToEntity(createCampaignMemberDTO);
                return await cmRepository.InsertAsync(newCampaignMember);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        private async Task<CampaignMember> CreateCampaignMemberDTOToEntity(CreateCampaignMemberDTO createCampaignMemberDTO)
        {
            return new CampaignMember
            {
                CampaignId = createCampaignMemberDTO.CampaignId,
                UserId = createCampaignMemberDTO.UserId
            };
        }

        public async Task<List<CampaignMember>> GetAllCampaignsAsync()
        {
            try
            {
                return await cmRepository.GetAll().ToListAsync();
            }
            catch (Exception e)
            {
                return new List<CampaignMember>();
            }
        }

        public async Task<CampaignMember?> GetCampaignMemberByCampaignIdAndUserIdAsync(int campaignId, string userId)
        {
            try
            {
                return await cmRepository.Get(x => x.UserId == userId && x.CampaignId == campaignId);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //public async Task<bool> UpdateCampaignMemberAsync(UpdateCampaignMemberDTO updateCampaignMemberDTO)
        //{
            
        //}

        //private void UpdateCampaignMemberDTOEntity(CampaignMember campaignMemberModels, UpdateCampaignMemberDTO updateCampaignMemberDTO)
        //{
        //    campaignMemberModels.Debt = updateCampaignMemberDTO.Debt;
        //    campaignMemberModels.IsActive = updateCampaignMemberDTO.IsActive;
        //}
    }
}
