using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.EntityFrameworkCore;

namespace FALOFinancialProofing.Services.OrganizationServices
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IRepository<Organization, int> _organizationRepository;

        public OrganizationService(IRepository<Organization, int> transactionLogRepository)
        {
            _organizationRepository = transactionLogRepository;
        }

        #region Comment may use
        //private Organization ConvertToBaseEntity(Organization createOrganization)
        //{
        //    var organization = new Organization
        //    {
        //        SenderID = createOrganization.SenderID,
        //        CampaignId = createOrganization.CampaignId,
        //        TransactionDate = createOrganization.TransactionDate,
        //        Amount = createOrganization.Amount,
        //        Description = createOrganization.Description,
        //        BankId = createOrganization.BankId
        //    };
        //    return organization;
        //}

        //private void ConvertToBaseEntity(Organization SourceOrganization, Organization DesOrganization)
        //{
        //    SourceOrganization.SenderID = DesOrganization.SenderID;
        //    SourceOrganization.Amount = DesOrganization.Amount;
        //    SourceOrganization.BankId = DesOrganization.BankId;
        //    SourceOrganization.Description = DesOrganization.Description;
        //    SourceOrganization.CampaignId = DesOrganization.CampaignId;
        //}
        #endregion
        public async Task<bool> CreateOrganizationAsync(Organization createOrganization)
        {
            try
            {
                if (createOrganization == null)
                {
                    throw new Exception("Organization is null");
                }
                //var organization = ConvertToBaseEntity(createOrganization);
                await _organizationRepository.InsertAsync(createOrganization);

                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"Organization: {ex.Message}!");
            }

            return false;
        }

        public async Task<Organization> GetOrganizationByIdAsync(int id)
        {
            Organization organization = null!;
            try
            {
                organization = await _organizationRepository.Get(id);
                if (organization == null)
                {
                    throw new Exception("Organization not found");
                }

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetOrganizationById: {ex.Message}");
            }

            return organization;
        }

        public async Task<IEnumerable<Organization>> GetAllOrganizationsAsync()
        {
            List<Organization> data = null!;
            try
            {
                data = await _organizationRepository.GetAll().ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllOrganizations: {ex.Message}");
            }

            return data;
        }


        // admin can update transaction logs
        public async Task<bool> UpdateOrganizationAsync(Organization updateOrganization)
        {
            Organization organization = null!;
            bool result = false;
            try
            {
                organization = await _organizationRepository.Get(updateOrganization.Id);
                if (organization == null)
                {
                    throw new Exception("Organization not found!");
                }
                //ConvertToBaseEntity(organization, updateOrganization);
                result = await _organizationRepository.UpdateAsync(updateOrganization);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"UpdateOrganization: {ex.Message}");
            }

            return result;
        }
        // admin can delete transaction logs
        public async Task<bool> DeleteOrganizationAsync(int id)
        {
            Organization organization = null!;
            bool result = false;
            try
            {
                organization = await _organizationRepository.Get(id);
                if (organization == null)
                {
                    throw new Exception("Organization not found!");
                }
                result = await _organizationRepository.DeleteAsync(organization);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"DeleteOrganization: {ex.Message}");
            }

            return result;
        }
    }
}
