using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Services.OrganizationServices
{
    public interface IOrganizationService
    {
        Task<bool> CreateOrganizationAsync(Organization createOrganization);
        Task<Organization> GetOrganizationByIdAsync(int id);
        Task<IEnumerable<Organization>> GetAllOrganizationsAsync();
        Task<List<Organization>> GetOrganizationsByUserIdAsync(string userId);
        Task<bool> UpdateOrganizationAsync(Organization updateOrganization);
        Task<bool> DeleteOrganizationAsync(int id);

    }
}
