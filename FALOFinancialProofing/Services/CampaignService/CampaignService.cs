using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.DTOs.CampaignDTO;
using FALOFinancialProofing.DTOs.CreateCampaignFileDTO;
using FALOFinancialProofing.DTOs.ProjectDTOs;
using FALOFinancialProofing.DTOs.TransactionLogsDTOs;
using FALOFinancialProofing.DTOs.UserDTOs;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using FALOFinancialProofing.Services.BankServices;
using FALOFinancialProofing.Services.CreateProjectRequestServices;
using FALOFinancialProofing.Services.ProjectServices;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace FALOFinancialProofing.Services.CampaignService
{
    public class CampaignService : ICampaignService
    {
        private readonly IRepository<Campaign, int> campaignRepository;
        private readonly AuthServices _authServices;
        private readonly IProjectService _projectService;
        private readonly IBankService bankService;
        private readonly ILogger<CampaignService> _logger;
        public CampaignService(IRepository<Campaign, int> _campaignRepository, AuthServices authServices, IProjectService projectService, ILogger<CampaignService> logger, IBankService bankService)
        {
            campaignRepository = _campaignRepository;
            _authServices = authServices;
            _projectService = projectService;
            _logger = logger;
            this.bankService = bankService;
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
                //ProjectId = createCampaignDTO.ProjectId,
                CreateBy = createCampaignDTO.CreateBy,
                Title = createCampaignDTO.Title,
                Description = createCampaignDTO.Description,
                DateOfCreation = createCampaignDTO.DateOfCreation,
                FundTarget = createCampaignDTO.FundTarget,
                Image = createCampaignDTO.Image,
                EndDate = createCampaignDTO.EndDate,
                Address = createCampaignDTO.Address,
                IsActive = createCampaignDTO.IsActive,
                BankId = createCampaignDTO.BankId,
                Status = createCampaignDTO.Status
            };
        }
        public async Task<List<CampaignInformation>> GetAllCampaignsAsync(HttpRequest request)
        {
            List<CampaignInformation> data = null!;
            try
            {
                data = await campaignRepository.GetAll()
                    .Where(p => p.Status != null && !p.Status.Equals(RequestStatus.Rejected))
                    .Select(p => new CampaignInformation()
                    {
                        FirstName = p.User.FirstName,
                        LastName = p.User.LastName,
                        CampaignId = p.Id,
                        ProjectId = p.ProjectId,
                        ProjectName = p.Project.ProjectName,
                        CreateBy = p.CreateBy,
                        Title = p.Title,
                        Description = p.Description,
                        DateOfCreation = p.DateOfCreation,
                        FundTarget = p.FundTarget,
                        Image = UrlHelper.GetImageUrl(request, p.Image, FolderImage.CampaignImageUpload),
                        EndDate = p.EndDate,
                        Address = p.Address,
                        IsActive = p.IsActive,
                        BankId = p.BankId,
                        Status = p.Status,
                        UpdateLog = p.UpdateLog,
                        TotalMoneyEarned = p.TransactionLogs.Sum(x => (double)x.Amount)
                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllCampaignsAsync: {ex.Message}");
            }

            return data;
        }
        public async Task<List<CampaignInformation>> GetAllCampaignsByProjectIdAsync(int ProjectId, HttpRequest request)
        {
            List<CampaignInformation> data = null!;
            try
            {
                data = await campaignRepository.GetAll().Where(p => p.ProjectId == ProjectId)
                    .Select(p => new CampaignInformation()
                    {
                        FirstName = p.User.FirstName,
                        LastName = p.User.LastName,
                        CampaignId = p.Id,
                        ProjectId = p.ProjectId,
                        ProjectName = p.Project.ProjectName,
                        CreateBy = p.CreateBy,
                        Title = p.Title,
                        Description = p.Description,
                        DateOfCreation = p.DateOfCreation,
                        FundTarget = p.FundTarget,
                        Image = UrlHelper.GetImageUrl(request, p.Image, FolderImage.CampaignImageUpload),
                        EndDate = p.EndDate,
                        Address = p.Address,
                        IsActive = p.IsActive,
                        BankId = p.BankId,
                        Status = p.Status,
                        TotalMoneyEarned = p.TransactionLogs.Sum(x => (double)x.Amount),
                        UpdateLog = p.UpdateLog,
                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllCampaignsByProjectIdAsync: {ex.Message}");
            }

            return data;
        }



        public async Task<CampaignInformation> GetCampaignByCampaignIdAsync(int CampaignId, HttpRequest request)
        {
            CampaignInformation data = null!;
            try
            {
                data = await campaignRepository.GetAll().Where(p => p.Id == CampaignId)
                    .Select(p => new CampaignInformation()
                    {
                        FirstName = p.User.FirstName,
                        LastName = p.User.LastName,
                        CampaignId = p.Id,
                        ProjectId = p.ProjectId,
                        ProjectName = p.Project.ProjectName,
                        CreateBy = p.CreateBy,
                        Title = p.Title,
                        Description = p.Description,
                        DateOfCreation = p.DateOfCreation,
                        FundTarget = p.FundTarget,
                        Image = UrlHelper.GetImageUrl(request, p.Image, FolderImage.CampaignImageUpload),
                        EndDate = p.EndDate,
                        Address = p.Address,
                        IsActive = p.IsActive,
                        AccountNumber = p.Bank.AccountNumber,
                        BankId = p.BankId,
                        Status = p.Status,
                        UpdateLog = p.UpdateLog,
                        TotalMoneyEarned = p.TransactionLogs.Sum(x => (double)x.Amount),
                        CreateCampaignFiles = p.CreateCampaignRequests.SelectMany(ccr => ccr.CreateCampaignFiles).Select(f => new CreateCampaignFileInformation()
                        {
                            Id = f.Id,
                            RequestId = f.RequestId,
                            FilePath = f.FilePath
                        }).ToList()
                    }).SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetCampaignByCampaignIdAsync: {ex.Message}");
            }

            return data;
        }
        public async Task<Campaign> GetCampaignByCampaignIdAsync(int CampaignId)
        {
            Campaign data = null!;
            try
            {
                data = await campaignRepository.GetAll().Where(p => p.Id == CampaignId)
                    .SingleOrDefaultAsync();
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
        public async Task<bool> UpdateCampaignAsync(UpdateCampaignDTO updateCampaignDTO, StringBuilder message)
        {
            bool checkValid = false;
            try
            {
                var campaign = await campaignRepository.Get(updateCampaignDTO.Id);
                campaign.Title = updateCampaignDTO.Title;
                campaign.Description = updateCampaignDTO.Description;
                campaign.Address = updateCampaignDTO.Address;
                campaign.Image = await FileHelper.SaveImageAndReturnShortPathAsync(updateCampaignDTO.LogoFile, FolderImage.CampaignImageUpload, campaign.Image) ?? campaign.Image;
                bool checkAdmin = await _authServices.CheckUserInRole(updateCampaignDTO.UserId, AppRole.Admin, new StringBuilder());
                bool checkPMB = await _authServices.CheckUserInRole(updateCampaignDTO.UserId, AppRole.ProjectManagementBoard, new StringBuilder());
                if (checkAdmin || checkPMB)
                {
                    var GetBank = await bankService.GetBankByIdAsync(updateCampaignDTO.BankId ?? 0);
                    if (GetBank == null)
                        throw new Exception("There no such bank in system!");
                    campaign.FundTarget = updateCampaignDTO.FundTarget ?? campaign.FundTarget;
                    campaign.IsActive = updateCampaignDTO.IsActive ?? campaign.IsActive;
                    campaign.Status = updateCampaignDTO.Status ?? campaign.Status;
                    campaign.BankId = updateCampaignDTO.BankId;
                }
                checkValid = await campaignRepository.UpdateAsync(campaign);
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"UpdateCampaignAsync: {ex.Message}");
            }

            return checkValid;
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
                if (DateTime.Now > createCampaignClientRequest.EndDate)
                {
                    throw new Exception("End Date must be after");
                }
                if (createCampaignClientRequest.LogoFile != null && createCampaignClientRequest.LogoFile.Length > FileHelper.CampaignImageMaxFileSize)
                {
                    throw new Exception("Logo is too large");
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
                    DateOfCreation = DateTime.Now, // sửa ở đây
                    FundTarget = createCampaignClientRequest.FundTarget,
                    Image = await FileHelper.SaveImageAndReturnShortPathAsync(createCampaignClientRequest.LogoFile, FolderImage.CampaignImageUpload, null),
                    EndDate = createCampaignClientRequest.EndDate,
                    Address = createCampaignClientRequest.Address,
                    IsActive = false,
                    Description = createCampaignClientRequest.Description,
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
                    if (campaign.CampaignMembers.Any(cm => cm.UserId == userId
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

        public async Task<Campaign?> UpdateEndDateForProjectManagerAsync(int campaignId, string userId, string currentRole, DateTime newDateTime, StringBuilder message)
        {
            try
            {
                // Check if current logging role is Project Manager
                if (!currentRole.Equals(Resource.ProjectManagerRoleName))
                {
                    message.Append("You are not Project Manager");
                    return null;
                }
                // check if campaign is exist
                var campaignWithMemberAndRole = await campaignRepository.GetAll(x => x.Id == campaignId
                                                    && !x.Status.Equals(Resource.CampaignStatus_Close)
                                                    && x.IsActive)
                    .Include(x => x.CampaignMembers)
                    .ThenInclude(x => x.IdentityRole).FirstOrDefaultAsync();
                if (campaignWithMemberAndRole == null)
                {
                    message.Append("Campaign not found or is close or is not active");
                    return null;
                }

                // check if user is in campaign, is active and is Project Manager
                var campaignMember = campaignWithMemberAndRole.CampaignMembers.FirstOrDefault(x => x.UserId == userId
                                    && x.IsActive
                                    && x.IdentityRole.Name.Equals(Resource.ProjectManagerRoleName));
                if (campaignMember == null)
                {
                    message.Append("You are not in this campaign or is not active or is not Project Manager");
                    return null;
                }

                // check if new date is valid
                if (newDateTime.CompareTo(campaignWithMemberAndRole.EndDate) <= 0)
                {
                    message.Append("New date must be after the current end date");
                    return null;
                }

                campaignWithMemberAndRole.EndDate = newDateTime;
                var newUpdateLog = new StringBuilder(campaignWithMemberAndRole.UpdateLog);
                newUpdateLog.AppendLine($"Project Manager change end date to {newDateTime} at {DateTime.Now}");
                campaignWithMemberAndRole.UpdateLog = newUpdateLog.ToString();
                var canUpdate = await campaignRepository.UpdateAsync(campaignWithMemberAndRole);
                if (!canUpdate)
                {
                    message.Append("Operation is not valid, update failed");
                    return null;
                }
                return campaignWithMemberAndRole;

            }
            catch (Exception ex)
            {
                message.Append("Operation is not valid");
                return null;
            }

        }

        public async Task<Campaign> GetCampaignByUserIdAndCampaignIdAsync(string pmUserId, int campaignId)
        {
            Campaign campaign = null!;
            try
            {
                campaign = await campaignRepository.Get(p => p.CreateBy == pmUserId && p.Id == campaignId);
                if (campaign == null)
                {
                    throw new Exception("This user Has no such Campaign");
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"CheckCampaignByUserIdAndCampaignIdAsync: {ex.Message}");
            }

            return campaign;
        }

        // check user có phải là admin, pmb, pm(người tạo ra project hay không)
        public async Task<bool> ValidateCampaignUpdateAsync(UpdateCampaignDTO updateCampaignDTO, StringBuilder message)
        {
            bool checkValid = false;
            try
            {
                var campaign = await campaignRepository.Get(updateCampaignDTO.Id);
                if (campaign == null)
                {
                    throw new Exception("Campaign not found!");
                }
                var campaignOwner = await GetCampaignByUserIdAndCampaignIdAsync(updateCampaignDTO.UserId, updateCampaignDTO.Id);
                bool checkAdmin = await _authServices.CheckUserInRole(updateCampaignDTO.UserId, AppRole.Admin, new StringBuilder());
                bool checkPMB = await _authServices.CheckUserInRole(updateCampaignDTO.UserId, AppRole.ProjectManagementBoard, new StringBuilder());
                if ((campaignOwner == null && !checkPMB && !checkAdmin) || (campaignOwner != null && !campaignOwner.IsActive))
                {
                    throw new Exception("You don't have permission to update campaign!");
                }
                if (updateCampaignDTO.LogoFile != null && updateCampaignDTO.LogoFile.Length > FileHelper.CampaignImageMaxFileSize)
                {
                    throw new Exception("Logo is too large");
                }

                checkValid = true;
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"ValidateProjectUpdateAsync: {ex.Message}");
            }
            return checkValid;
        }

    }
}
