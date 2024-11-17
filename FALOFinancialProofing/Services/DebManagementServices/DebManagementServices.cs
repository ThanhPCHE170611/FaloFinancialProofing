using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace FALOFinancialProofing.Services.DebManagementServices
{
    public class DebManagementServices : IDebManagementServices
    {

        private readonly IRepository<CampaignMember, int> repository;
        private readonly AuthServices authServices;

        public DebManagementServices(IRepository<CampaignMember, int> repository, 
            AuthServices authServices)
        {
            this.repository = repository;
            this.authServices = authServices;
        }

        public async Task<List<UserWithDeb>> GetDebManagementForAccounting(string userId, string currentLoggingRole,
            StringBuilder message,  int campaignId)
        {
            var userWithDeb = new List<UserWithDeb>();
            try
            {
                var isLoggingInAccounting = await authServices.CheckUserInRole(userId, currentLoggingRole, message);
                if (!isLoggingInAccounting)
                {
                    return userWithDeb;
                }
                var isValidateRequest = CheckValidateRequestForAccounting(userId, campaignId, message);
                userWithDeb = await repository.GetAll(
                    cm => cm.CampaignId == campaignId
                    )
                    .Include(cm => cm.User)
                    .Include(cm => cm.IdentityRole)
                        .Select(cm => new UserWithDeb
                        {
                            UserId = cm.UserId,
                            UserRole = cm.IdentityRole.Name,
                            UserEmail = cm.User.Email,
                            Debt = cm.Debt,
                            CampaignId = cm.CampaignId,
                            CampaignName = cm.Campaign.Description,
                            IsActive = cm.IsActive
                        }).ToListAsync();

                return userWithDeb;
            }
            catch (Exception ex)
            {
                message.Append("Get User with Deb for accounting fail");
                return userWithDeb;
            }
        }

        private bool CheckValidateRequestForAccounting(string userId, int campaignId, StringBuilder message)
        {
            var isValidateRequest = false;
            try
            {
                var accountingInCampaign = repository.GetAll()
                    .Include(cm => cm.User)
                    .Include(cm => cm.IdentityRole)
                    .Where(cm => cm.UserId == userId 
                        && cm.CampaignId == campaignId
                        && cm.IdentityRole.Name == AppRole.Accounting
                        && cm.IsActive)
                    .FirstOrDefault();
                if (accountingInCampaign == null)
                {
                    message.Append("User is not in the campaign or not in accounting role or accounting is not longer active");
                    return isValidateRequest;
                }
                return true;
            }
            catch (Exception ex)
            {
                message.Append("Validate Accounting Failed!");
                return isValidateRequest;
            }
        }

        public async Task<List<UserWithDeb>> GetDebManagementForPMB(string userId, 
                                                            string currentLoggingRole,
                                                            StringBuilder message)
        {
            var userWithDeb = new List<UserWithDeb>();
            try
            {
                var isRequestValidate = await authServices.CheckUserInRole(userId, currentLoggingRole, message);
                if (!isRequestValidate)
                {
                    return userWithDeb;
                }
                userWithDeb = await repository.GetAll()
                    .Include(cm => cm.User)
                    .Include(cm => cm.IdentityRole)
                        .Select(cm => new UserWithDeb
                        {
                            UserId = cm.UserId,
                            UserRole = cm.IdentityRole.Name,
                            UserEmail = cm.User.Email,
                            Debt = cm.Debt,
                            CampaignId = cm.CampaignId,
                            CampaignName = cm.Campaign.Description,
                            IsActive = cm.IsActive
                        }).ToListAsync();

                return userWithDeb;
            }
            catch (Exception ex)
            {
                message.Append("Get User with Deb for PMB fail");
                return userWithDeb;
            }
        }
    }
}
