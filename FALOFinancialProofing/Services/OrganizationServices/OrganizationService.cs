using FALOFinancialProofing.DTOs.CampaignDTO;
using FALOFinancialProofing.DTOs.OrganizationDTO;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace FALOFinancialProofing.Services.OrganizationServices
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IRepository<Organization, int> _organizationRepository;
        private readonly AuthServices _authServices;

        public OrganizationService(IRepository<Organization, int> organizationRepository, AuthServices authServices)
        {
            _organizationRepository = organizationRepository;
            _authServices = authServices;
        }

        #region Comment may use


        //private void ConvertToBaseEntity(Organization SourceOrganization, Organization DesOrganization)
        //{
        //    SourceOrganization.SenderID = DesOrganization.SenderID;
        //    SourceOrganization.Amount = DesOrganization.Amount;
        //    SourceOrganization.BankId = DesOrganization.BankId;
        //    SourceOrganization.Description = DesOrganization.Description;
        //    SourceOrganization.CampaignId = DesOrganization.CampaignId;
        //}
        #endregion
        private async Task<Organization> ConvertToBaseEntity(CreateOrganization createOrganization)
        {
            var organization = new Organization
            {
                Name = createOrganization.Name,
                Main_office = createOrganization.Main_office,
                Representative = createOrganization.Representative,
                PhoneNumber = createOrganization.PhoneNumber,
                Email = createOrganization.Email,
                Logo =
                await FileHelper.SaveImageAndReturnShortPathAsync(createOrganization.LogoFile, FolderImage.OrganizationImageUpload, null),
                Description = createOrganization.Description,
                Vision = createOrganization.Vision,
                Mission = createOrganization.Mission,
                CoreValue = createOrganization.CoreValue,
                MainActivity = createOrganization.MainActivity,
                Interests = createOrganization.Interests,
                VolunteerExperience = createOrganization.VolunteerExperience,
                VolunteerObjectives = createOrganization.VolunteerObjectives,
                //Attachments = createOrganization.Attachments,
                Bio = createOrganization.Bio,
            };
            return organization;
        }
        public async Task<Organization> CreateOrganizationAsync(CreateOrganization createOrganization, StringBuilder message)
        {
            Organization organization = null!;
            try
            {
                if (createOrganization == null)
                {
                    throw new Exception("Organization is null");
                }
                var Organization = await ConvertToBaseEntity(createOrganization);
                organization = await _organizationRepository.InsertAsync(Organization);
                message.Append("Create Organization Successfully!");
            }
            catch (Exception ex)
            {
                message.Append("Create Organization Failed!");
                await Console.Out.WriteLineAsync($"CreateOrganizationAsync: {ex.Message}!");
            }

            return organization;
        }
        public async Task<bool> ValidateCreateOrganizationAsync(CreateOrganization createOrganization, StringBuilder message)
        {
            bool checkValid = false;
            try
            {
                var user = await _authServices.CheckUserExist(createOrganization.UserId, message);
                if (!user)
                {
                    throw new Exception();
                }
                checkValid = true;
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"ValidateCreateOrganizationAsync: {ex.Message}!");
            }

            return checkValid;
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
        //* tạo thêm thông tin tổ chức ở userOrganization
        public async Task<List<Organization>> GetOrganizationsByUserIdAsync(string userId)
        {
            List<Organization> organizations = null!;
            try
            {
                // tìm organizations mà User đã tham gia
                organizations = await _organizationRepository.GetAll()
                    .Include(x => x.OrganizationMembers)
                    .Where(o => o.OrganizationMembers.Any(om => om.UserId.Equals(userId)))
                    .ToListAsync();
                if (organizations == null)
                {
                    throw new Exception("organizations not found");
                }

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetOrganizationByUserIdAsync: {ex.Message}");
            }

            return organizations;
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
