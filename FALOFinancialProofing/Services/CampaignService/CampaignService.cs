using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.DTOs.CampaignDTO;
using FALOFinancialProofing.DTOs.ProjectDTOs;
using FALOFinancialProofing.DTOs.TransactionLogsDTOs;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using FALOFinancialProofing.Services.CreateProjectRequestServices;
using FALOFinancialProofing.Services.ProjectServices;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace FALOFinancialProofing.Services.CampaignService
{
    public class CampaignService : ICampaignService
    {
        private readonly IRepository<Campaign, int> campaignRepository;
        private readonly AuthServices _authServices;
        private readonly IProjectService _projectService;
        public CampaignService(IRepository<Campaign, int> _campaignRepository, AuthServices authServices, IProjectService projectService)
        {
            campaignRepository = _campaignRepository;
            _authServices = authServices;
            _projectService = projectService;
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
        public async Task<List<CampaignInformation>> GetAllCampaignsByProjectIdAsync(int ProjectId)
        {
            List<CampaignInformation> data = null!;
            try
            {
                data = await campaignRepository.GetAll().Where(p => p.ProjectId == ProjectId)
                    .Select(p => new CampaignInformation()
                    {
                        CampaignId = p.Id,
                        ProjectId = p.ProjectId,
                        CreateBy = p.CreateBy,
                        Title = p.Title,
                        Description = p.Description,
                        DateOfCreation = p.DateOfCreation,
                        FundTarget = p.FundTarget,
                        Image = p.Image,
                        EndDate = p.EndDate,
                        Address = p.Address,
                        IsActive = p.IsActive,
                        BankingNumber = p.BankingNumber,
                        Status = p.Status,
                        TotalMoneyEarned = p.TransactionLogs.Sum(x => x.Amount)
                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllCampaignsByProjectIdAsync: {ex.Message}");
            }

            return data;
        }

        public async Task<CampaignInformation> GetCampaignByCampaignIdAsync(int CampaignId)
        {
            CampaignInformation data = null!;
            try
            {
                data = await campaignRepository.GetAll().Where(p => p.Id == CampaignId)
                    .Select(p => new CampaignInformation()
                    {
                        CampaignId = p.Id,
                        ProjectId = p.ProjectId,
                        CreateBy = p.CreateBy,
                        Title = p.Title,
                        Description = p.Description,
                        DateOfCreation = p.DateOfCreation,
                        FundTarget = p.FundTarget,
                        Image = p.Image,
                        EndDate = p.EndDate,
                        Address = p.Address,
                        IsActive = p.IsActive,
                        BankingNumber = p.BankingNumber,
                        Status = p.Status,
                        TotalMoneyEarned = p.TransactionLogs.Sum(x => x.Amount)
                    }).SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetCampaignByCampaignIdAsync: {ex.Message}");
            }

            return data;
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

        public async Task<bool> CreateCampaignRequestAsync(CreateCampaignClientRequest createCampaignClientRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ValidateCampaignCreateAsync(CreateCampaignClientRequest createCampaignClientRequest, StringBuilder message)
        {
            bool IsValid = false;
            try
            {
                bool checkValidUser = await _authServices.CheckUserInRole(createCampaignClientRequest.CreateBy, AppRole.ProjectManager, message);
                if (!checkValidUser)
                {
                    throw new Exception("User is not in role");
                }
                var projectCreatedByUser = await _projectService.CheckProjectByUserIdAndProjectIdAsync(createCampaignClientRequest.CreateBy, createCampaignClientRequest.ProjectId);
                if (!projectCreatedByUser)
                {
                    throw new Exception("This user has no such Project");
                }
                var isProjectActive = await _projectService.CheckProjectIsActiveAsync(createCampaignClientRequest.ProjectId);
                if (!isProjectActive)
                {
                    throw new Exception("Project is not active");
                }
                // số tiền tạo > 0
                if (createCampaignClientRequest.FundTarget < 0)
                {
                    throw new Exception("Fund target must be greater than 0");
                }
                //trạng thái dự án chưa được phép true
                if (createCampaignClientRequest.IsActive)
                {
                    throw new Exception("IsActive must be false");
                }
                // ngày tạo không được lớn hơn ngày hiện tại
                if (createCampaignClientRequest.DateOfCreation > DateTime.Now)
                {
                    throw new Exception("Date of creation cannot be in the future");
                }
                if (createCampaignClientRequest.DateOfCreation < createCampaignClientRequest.EndDate)
                {
                    throw new Exception("End Date must be after");
                }
                IsValid = true;
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"ValidateProjectCreate: {ex.Message}");
            }

            return IsValid;
        }

        public async Task<Campaign> ConvertDtoToBaseClass(CreateCampaignClientRequest createCampaignClientRequest)
        {
            Campaign campaign = null!;
            try
            {
                campaign = new Campaign
                {
                    CreateBy = createCampaignClientRequest.CreateBy,
                    ProjectId = createCampaignClientRequest.ProjectId,
                    Title = createCampaignClientRequest.Title,
                    Description = createCampaignClientRequest.Description,
                    DateOfCreation = createCampaignClientRequest.DateOfCreation,
                    FundTarget = createCampaignClientRequest.FundTarget,
                    Image = createCampaignClientRequest.Image,
                    EndDate = createCampaignClientRequest.EndDate,
                    Address = createCampaignClientRequest.Address,
                    IsActive = createCampaignClientRequest.IsActive,
                    BankingNumber = createCampaignClientRequest.BankingNumber,
                    Status = createCampaignClientRequest.Status,
                };
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"ConvertDtoToBaseClass: {ex.Message}");
            }

            return campaign;
        }

        public async Task<Campaign> CreateCampaignReturnEntityAsync(Campaign createCampaign)
        {
            try
            {
                if (createCampaign == null)
                {
                    throw new Exception("Campaign is null");
                }
                await campaignRepository.InsertAsync(createCampaign);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"Campaign: {ex.Message}!");
            }

            return createCampaign;
        }

        public async Task<List<Campaign>> GetAllCampaignByUserIdAndRoleAsync(string userId, string currentRole)
        {
            var campaigns = new List<Campaign>();
            try
            {
                var allCampaigns = await campaignRepository.GetAll().Include(c => c.CampaignMembers).ThenInclude(cm => cm.IdentityRole).ToListAsync();

                foreach (var campaign in allCampaigns)
                {
                    if(campaign.CampaignMembers.Any(cm => cm.UserId == userId 
                        && cm.IdentityRole.Name.Equals(currentRole)
                        && cm.IsActive))
                    {
                        campaigns.Add(campaign);
                    }
                }
                return campaigns;
            }
            catch (Exception ex)
            {
                return campaigns;
            }
        }
    }
}
