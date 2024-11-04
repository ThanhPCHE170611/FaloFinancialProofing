using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.EntityFrameworkCore;

namespace FALOFinancialProofing.Services.OrganizationMemberServices
{
    public class OrganizationMemberService : IOrganizationMemberService
    {

        private readonly IRepository<OrganizationMember, int> _organizationMemberRepository;

        public OrganizationMemberService(IRepository<OrganizationMember, int> organizationMemberRepository)
        {
            _organizationMemberRepository = organizationMemberRepository;
        }
        public async Task<bool> CreateOrganizationMemberAsync(OrganizationMember createOrganizationMember)
        {
            try
            {
                if (createOrganizationMember == null)
                {
                    throw new Exception("OrganizationMember is null");
                }
                //var organization = ConvertToBaseEntity(createOrganizationMember);
                await _organizationMemberRepository.InsertAsync(createOrganizationMember);

                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"OrganizationMember: {ex.Message}!");
            }

            return false;
        }

        public async Task<OrganizationMember> GetOrganizationMemberByIdAsync(int id)
        {
            OrganizationMember organization = null!;
            try
            {
                organization = await _organizationMemberRepository.Get(id);
                if (organization == null)
                {
                    throw new Exception("OrganizationMember not found");
                }

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetOrganizationMemberById: {ex.Message}");
            }

            return organization;
        }

        public async Task<IEnumerable<OrganizationMember>> GetAllOrganizationMembersAsync()
        {
            List<OrganizationMember> data = null!;
            try
            {
                data = await _organizationMemberRepository.GetAll().ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllOrganizationMembers: {ex.Message}");
            }

            return data;
        }


        // admin can update transaction logs
        public async Task<bool> UpdateOrganizationMemberAsync(OrganizationMember updateOrganizationMember)
        {
            OrganizationMember organization = null!;
            bool result = false;
            try
            {
                organization = await _organizationMemberRepository.Get(updateOrganizationMember.Id);
                if (organization == null)
                {
                    throw new Exception("OrganizationMember not found!");
                }
                //ConvertToBaseEntity(organization, updateOrganizationMember);
                result = await _organizationMemberRepository.UpdateAsync(updateOrganizationMember);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"UpdateOrganizationMember: {ex.Message}");
            }

            return result;
        }
        // admin can delete transaction logs
        public async Task<bool> DeleteOrganizationMemberAsync(int id)
        {
            OrganizationMember organization = null!;
            bool result = false;
            try
            {
                organization = await _organizationMemberRepository.Get(id);
                if (organization == null)
                {
                    throw new Exception("OrganizationMember not found!");
                }
                result = await _organizationMemberRepository.DeleteAsync(organization);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"DeleteOrganizationMember: {ex.Message}");
            }

            return result;
        }
    }
}
