using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Services.OrganizationMemberServices
{
    public interface IOrganizationMemberService
    {
        Task<bool> CreateOrganizationMemberAsync(OrganizationMember createOrganizationMember);
        Task<OrganizationMember> GetOrganizationMemberByIdAsync(int id);
        Task<IEnumerable<OrganizationMember>> GetAllOrganizationMembersAsync();
        Task<bool> UpdateOrganizationMemberAsync(OrganizationMember updateOrganizationMember);
        Task<bool> DeleteOrganizationMemberAsync(int id);
        Task<OrganizationMember> GetOrganizationMemberByUserIdAndOrganizationIdAsync(string userId, int organizationId);
    }
}
