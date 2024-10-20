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

        public async Task<List<CampaignMember>> GetAllCampaignMembersAsync()
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

        public async Task<bool> UpdateCampaignMemberAsync(UpdateCampaignMemberDTO updateCampaignMemberDTO)
        {
            try
            {
                var existingCampaignMember = await cmRepository.Get(x => x.UserId == updateCampaignMemberDTO.UserId && x.CampaignId == updateCampaignMemberDTO.CampaignId);

                if (existingCampaignMember == null)
                {
                    return false;
                }

                UpdateCampaignMemberDTOToEntity(existingCampaignMember, updateCampaignMemberDTO);

                return await cmRepository.UpdateAsync(existingCampaignMember);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void UpdateCampaignMemberDTOToEntity(CampaignMember campaignMember, UpdateCampaignMemberDTO updateCampaignMemberDTO)
        {
            campaignMember.Debt = updateCampaignMemberDTO.Debt;
            campaignMember.IsActive = updateCampaignMemberDTO.IsActive;
        }

        public async Task<bool> DeleteCampaignMemberByCampaignIdAndUserIdAsync(int campaignId, string userId)
        {
            try
            {
                var existingCampaignMember = await cmRepository.Get(x => x.UserId == userId && x.CampaignId == campaignId);
                if (existingCampaignMember == null) return false;

                return await cmRepository.DeleteAsync(existingCampaignMember);
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
