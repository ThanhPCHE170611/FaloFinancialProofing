using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.DTOs.CampaignDTO;
using FALOFinancialProofing.DTOs.TransactionLogsDTOs;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.EntityFrameworkCore;

namespace FALOFinancialProofing.Services.CampaignService
{
    public class CampaignService : ICampaignService
    {
        private readonly IRepository<Campaign, int> campaignRepository;
        public CampaignService(IRepository<Campaign, int> _campaignRepository)
        {
            campaignRepository = _campaignRepository;
        }

        public async Task<Campaign?> CreateCampaignAsync(CreateCampaignDTO createcampaignDTO)
        {
            try
            {
                var newCampaign = await CreateCampaignDTOToEntity(createcampaignDTO);

                return await campaignRepository.InsertAsync(newCampaign);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        private async Task<Campaign> CreateCampaignDTOToEntity(CreateCampaignDTO createCampaignDTO)
        {
            return new Campaign
            {
                //Id = createCampaignDTO.Id != null ? createCampaignDTO.Id.Value : 0,
                CreateBy = createCampaignDTO.CreateBy,
                Title = createCampaignDTO.Title,
                Description = createCampaignDTO.Description,
                DateOfCreation = createCampaignDTO.DateOfCreation,
                FundTarget = createCampaignDTO.FundTarget,
                Image = createCampaignDTO.Image,
                EndDate = createCampaignDTO.EndDate,
                Address = createCampaignDTO.Address,
                IsActive = createCampaignDTO.IsActive,
                BankingNumber = createCampaignDTO.BankingNumber,
                Status = createCampaignDTO.Status
            };
        }
        public async Task<List<Campaign>> GetAllCampaignsAsync()
        {
            try
            {
                return await campaignRepository.GetAll().ToListAsync();
            }
            catch (Exception e)
            {
                return new List<Campaign>();
            }
        }

        public async Task<Campaign?> GetCampaignByIdAsync(int id)
        {
            try
            {
                return await campaignRepository.Get(x => x.Id == id);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> UpdateCampaignAsync(UpdateCampaignDTO updateCampaignDTO)
        {
            Campaign campaign = null!;
            bool result = false;
            try
            {
                campaign = await campaignRepository.Get(updateCampaignDTO.Id);
                if (campaign == null)
                {
                    throw new Exception("Campaign not found!");
                }
                UpdateCampaignDTOEntity(campaign, updateCampaignDTO);
                result = await campaignRepository.UpdateAsync(campaign);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"Update Campaign: {ex.Message}");
            }

            return result;
        }

        private void UpdateCampaignDTOEntity(Campaign campaignModels, UpdateCampaignDTO updateCampaignDTO)
        {
            campaignModels.Title = updateCampaignDTO.Title;
            campaignModels.Description = updateCampaignDTO.Description;
            campaignModels.FundTarget = updateCampaignDTO.FundTarget;
            campaignModels.Image = updateCampaignDTO.Image;
            campaignModels.EndDate = updateCampaignDTO.EndDate;
            campaignModels.Address = updateCampaignDTO.Address;
            campaignModels.IsActive = updateCampaignDTO.IsActive;
            campaignModels.BankingNumber = updateCampaignDTO.BankingNumber;
            campaignModels.Status = updateCampaignDTO.Status;
        }

        public async Task<bool> DeleteCampaignByIdAsync(int id)
        {
            try
            {
                var campaign = await campaignRepository.Get(x => x.Id == id);
                if (campaign == null) return false;

                return await campaignRepository.DeleteAsync(campaign);
            }
            catch (Exception e)
            {
                return false;
            }
        }


    }
}
