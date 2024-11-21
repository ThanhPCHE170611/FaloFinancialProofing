using FALOFinancialProofing.DTOs.OrganizationDTO;
using FALOFinancialProofing.Models;
using System.Text;

namespace FALOFinancialProofing.Services.OrganizationServices
{
    public interface IOrganizationService
    {
        Task<Organization> CreateOrganizationAsync(CreateOrganization createOrganization, StringBuilder message);
        Task<OrganizationInformation> GetOrganizationsByIdAsync(int Id, HttpRequest request);
        Task<List<OrganizationInformation>> GetAllOrganizationsAsync(HttpRequest request);
        Task<List<OrganizationInformation>> GetAllOrganizationsByUserIdAsync(string userId, HttpRequest request);
        Task<bool> UpdateOrganizationAsync(UpdateOrganization updateOrganization, StringBuilder message);
        Task<bool> DeleteOrganizationAsync(int id);
        Task<bool> ValidateCreateOrganizationAsync(CreateOrganization createOrganization, StringBuilder message);
        Task<bool> ValidateOrganizationUpdateAsync(UpdateOrganization updateOrganization, StringBuilder message);

    }
}
