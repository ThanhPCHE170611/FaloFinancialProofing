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

        public ApproveProcessServices(IRepository<ApproveProcess, int> approveProcessRepository)
        {
            this.repository = approveProcessRepository;
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

        public async Task<List<CreateFormRequest>?> GetAllPrepayRequestForVolunteerLeader(string userid, string currentRoleLoggedIn)
        {
            var requests = new List<CreateFormRequest>();
            try
            {
                if (currentRoleLoggedIn != "Volunteer Leader")
                {
                    return new List<CreateFormRequest>();
                }

                // get the campaign of request
                var campaign = await repository.GetAll(x => x.ApproverId == userid)
                    .Include(x => x.RequestForm)
                    .ThenInclude(x => x.Campaign)
                    .ThenInclude(c => c.CampaignMembers)
                    .ThenInclude(cm => cm.IdentityRole)
                    .FirstOrDefaultAsync(cm => cm.RequestForm.Campaign.CampaignMembers.FirstOrDefault(x => x.UserId == userid && x.IdentityRole.Name.Equals("Volunteer Leader")) != null);
                if(campaign == null)
                {
                    return new List<CreateFormRequest>();
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
