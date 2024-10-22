using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace FALOFinancialProofing.Services.ApproveProcessServices
{
    public class ApproveProcessServices : IApproveProcessServices
    {

        private readonly IRepository<ApproveProcess, int> repository;
        private readonly IRepository<Campaign, int> campaignRepository;
        private readonly IRepository<CampaignMember, int> campaignMemberRepository;
        private readonly IRepository<RequestForm, int> requestFormRepository;

        public ApproveProcessServices(IRepository<ApproveProcess, int> repository, 
            IRepository<Campaign, int> campaignRepository, 
            IRepository<CampaignMember, int> campaignMemberRepository, 
            IRepository<RequestForm, int> requestFormRepository)
        {
            this.repository = repository;
            this.campaignRepository = campaignRepository;
            this.campaignMemberRepository = campaignMemberRepository;
            this.requestFormRepository = requestFormRepository;
        }

        public async Task<ApproveProcess?> CreateApproveProcessAsync(ApproveProcessRequest dto)
        {
            try
            {
                var newApproveProcess = await DTOToEntity(dto);
                return await repository.InsertAsync(newApproveProcess);
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        private async Task<ApproveProcess> DTOToEntity(ApproveProcessRequest dto)
        {
            return  new ApproveProcess
            {
                Id = dto.Id != null ? dto.Id.Value : 0,
                ApproveNumber = dto.ApproveNumber,
                ApproveStatus = dto.ApproveStatus,
                RequestId = dto.RequestId,
                ApproverId = dto.ApproverId,
                Vouchers = dto.Vouchers
            };
        }

        public async Task<bool> DeleteApproveProcessAsync(ApproveProcessRequest dto)
        {
            try
            {
                var deletedApproveProcess = await repository.Get(x => x.Id == dto.Id);
                if (deletedApproveProcess == null) return false;

                return await repository.DeleteAsync(deletedApproveProcess);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteApproveProcessByIdAsync(int id)
        {
            try
            {
                var deletedApproveProcess = await repository.Get(x => x.Id == id);
                if (deletedApproveProcess == null) return false;

                return await repository.DeleteAsync(deletedApproveProcess);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<ApproveProcess>> GetAllApproveProcesssAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ApproveProcess?> GetApproveProcessByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ApproveProcess>?> GetApproveProcessesByRequestIdAsync(int requestId)
        {
            try
            {
                return await repository.GetAll(ap => ap.RequestId == requestId)
                    .Include(ap => ap.RequestForm)
                    .Include(ap => ap.User)
                    .Include(ap => ap.Vouchers)
                    .Select(ap => new ApproveProcess
                    {
                        Id = ap.Id,
                        ApproveNumber = ap.ApproveNumber,
                        ApproveStatus = ap.ApproveStatus,
                        RequestId = ap.RequestForm.Id,
                        ApproverId = ap.User.Id,
                    }).ToListAsync();

            }
            catch (Exception ex)
            {
                return null;

            }
        }

        public async Task<bool> UpdateApproveProcessAsync(ApproveProcessRequest dto)
        {
            try
            {
                var updatedApproveProcess = await DTOToEntity(dto);
                return await repository.UpdateAsync(updatedApproveProcess);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<List<PrePayRequestFormViewRequest>?> GetAllPrepayRequestForVolunteerLeader(string userid, string currentRoleLoggedIn)
        {
            var requests = new List<PrePayRequestFormViewRequest>();
            try
            {
                // check if current logged in user is not Volunteer Leader
                if (currentRoleLoggedIn != "Volunteer Leader")
                {
                    return new List<PrePayRequestFormViewRequest>();
                }
                // get all the campaign that userid is a Volunteer Leader
                var campaigns = await campaignMemberRepository.GetAll(cm => cm.UserId == userid && cm.IdentityRole.Name.Equals("Volunteer Leader"))
                    .Include(cm => cm.Campaign)
                    .Include(cm => cm.IdentityRole)
                    .Select(cm => new Campaign
                    {
                        Id = cm.Campaign.Id,
                    }).ToListAsync();

                foreach(var campaign in campaigns)
                {
                    var requestForms = await requestFormRepository.GetAll(x => x.CampaignId == campaign.Id)
                        .Include(x => x.AttachmentFiles)
                        .Select(rf => new PrePayRequestFormViewRequest
                        {
                            Id = rf.Id,
                            CreateAt = rf.CreateAt,
                            Description = rf.Description,
                            ExpectedMoney = rf.ExpectedMoney,
                            Status = rf.Status,
                            CreatedBy = rf.CreatedBy,
                            CampaignId = rf.CampaignId,
                            AttachmentFiles = rf.AttachmentFiles.Select(af => new AttachmentFileRequest
                            {
                                FilePath = af.FilePath,
                                RequestId = af.RequestId
                            }).ToList()
                        }).ToListAsync();
                    if (requestForms != null && requestForms.Count > 0)
                    {
                        requests.AddRange(requestForms);
                    }
                }
                return requests;
            }
            catch (Exception ex)
            {
                return requests;
            }
        }
    }
}
