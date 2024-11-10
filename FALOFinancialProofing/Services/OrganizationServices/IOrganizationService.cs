using FALOFinancialProofing.DTOs.OrganizationDTO;
using FALOFinancialProofing.Models;
using System.Text;

namespace FALOFinancialProofing.Services.OrganizationServices
{
    public interface IOrganizationService
    {
        Task<Organization> CreateOrganizationAsync(CreateOrganization createOrganization, StringBuilder message);
        Task<Organization> GetOrganizationByIdAsync(int id);
        Task<IEnumerable<Organization>> GetAllOrganizationsAsync();
        Task<List<Organization>> GetOrganizationsByUserIdAsync(string userId);
        Task<bool> UpdateOrganizationAsync(Organization updateOrganization);
        Task<bool> DeleteOrganizationAsync(int id);
        Task<bool> ValidateCreateOrganizationAsync(CreateOrganization createOrganization, StringBuilder message);

    }
}
