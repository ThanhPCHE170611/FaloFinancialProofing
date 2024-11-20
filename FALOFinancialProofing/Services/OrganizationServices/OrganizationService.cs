using FALOFinancialProofing.DTOs.OrganizationDTO;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using FALOFinancialProofing.Services.OrganizationMemberServices;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace FALOFinancialProofing.Services.OrganizationServices
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IRepository<Organization, int> _organizationRepository;
        private readonly AuthServices _authServices;
        private readonly IOrganizationMemberService organizationMemberService;


        public OrganizationService(IRepository<Organization, int> organizationRepository, AuthServices authServices, IOrganizationMemberService organizationMemberService)
        {
            _organizationRepository = organizationRepository;
            _authServices = authServices;
            this.organizationMemberService = organizationMemberService;
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


        public async Task<List<OrganizationInformation>> GetAllOrganizationsAsync(HttpRequest request)
        {
            List<OrganizationInformation> data = null!;
            try
            {
                data = await _organizationRepository.GetAll().Select(o => new OrganizationInformation()
                {
                    Id = o.Id,
                    Name = o.Name,
                    Main_office = o.Main_office,
                    Representative = o.Representative,
                    PhoneNumber = o.PhoneNumber,
                    Email = o.Email,
                    Logo = UrlHelper.GetImageUrl(request, o.Logo, FolderImage.CampaignImageUpload), //*
                    Description = o.Description,
                    Vision = o.Vision,
                    Mission = o.Mission,
                    CoreValue = o.CoreValue,
                    MainActivity = o.MainActivity,
                    Interests = o.Interests,
                    VolunteerExperience = o.VolunteerExperience,
                    VolunteerObjectives = o.VolunteerObjectives,
                    Bio = o.Bio,
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllOrganizationsAsync: {ex.Message}");
            }

            return data;
        }

        public async Task<List<OrganizationInformation>> GetAllOrganizationsByUserIdAsync(string userId, HttpRequest request)
        {
            List<OrganizationInformation> data = null!;
            try
            {
                data = await _organizationRepository.GetAll(x => x.OrganizationMembers.Any(om => om.UserId.Equals(userId)))
                    .Select(o => new OrganizationInformation()
                    {
                        Id = o.Id,
                        Name = o.Name,
                        Main_office = o.Main_office,
                        Representative = o.Representative,
                        PhoneNumber = o.PhoneNumber,
                        Email = o.Email,
                        Logo = UrlHelper.GetImageUrl(request, o.Logo, FolderImage.OrganizationImageUpload), //*
                        Description = o.Description,
                        Vision = o.Vision,
                        Mission = o.Mission,
                        CoreValue = o.CoreValue,
                        MainActivity = o.MainActivity,
                        Interests = o.Interests,
                        VolunteerExperience = o.VolunteerExperience,
                        VolunteerObjectives = o.VolunteerObjectives,
                        Bio = o.Bio,
                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllOrganizationsByUserIdAsync: {ex.Message}");
            }

            return data;
        }
        public async Task<OrganizationInformation> GetOrganizationsByIdAsync(int Id, HttpRequest request)
        {
            OrganizationInformation data = null!;
            try
            {
                data = await _organizationRepository.GetAll(x => x.Id == Id)
                    .Select(o => new OrganizationInformation()
                    {
                        Id = o.Id,
                        Name = o.Name,
                        Main_office = o.Main_office,
                        Representative = o.Representative,
                        PhoneNumber = o.PhoneNumber,
                        Email = o.Email,
                        Logo = UrlHelper.GetImageUrl(request, o.Logo, FolderImage.OrganizationImageUpload), //*
                        Description = o.Description,
                        Vision = o.Vision,
                        Mission = o.Mission,
                        CoreValue = o.CoreValue,
                        MainActivity = o.MainActivity,
                        Interests = o.Interests,
                        VolunteerExperience = o.VolunteerExperience,
                        VolunteerObjectives = o.VolunteerObjectives,
                        Bio = o.Bio,
                    }).SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllOrganizationsByIdAsync: {ex.Message}");
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

        public async Task<bool> ValidateOrganizationUpdateAsync(UpdateOrganization updateOrganization, StringBuilder message)
        {
            bool checkValid = false;
            try
            {
                var organization = await _organizationRepository.Get(updateOrganization.Id);
                if (organization == null)
                {
                    throw new Exception("Organization not found!");
                }
                var organizationOwner = await organizationMemberService.GetOrganizationMemberByUserIdAndOrganizationIdAsync(updateOrganization.UserId, updateOrganization.Id);
                bool checkAdmin = await _authServices.CheckUserInRole(updateOrganization.UserId, AppRole.Admin, new StringBuilder());
                bool checkPMB = await _authServices.CheckUserInRole(updateOrganization.UserId, AppRole.ProjectManagementBoard, new StringBuilder());
                if (organizationOwner == null && !checkPMB && !checkAdmin)
                {
                    throw new Exception("You don't have permission to update organization!");
                }
                if (updateOrganization.LogoFile != null && updateOrganization.LogoFile.Length > FileHelper.OrganizationImageMaxFileSize)
                {
                    throw new Exception("Logo is too large");
                }

                checkValid = true;
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"ValidateOrganizationUpdateAsync: {ex.Message}");
            }
            return checkValid;
        }
        public async Task<bool> UpdateOrganizationAsync(UpdateOrganization updateOrganization, StringBuilder message)
        {
            bool checkValid = false;
            try
            {
                var organization = await _organizationRepository.Get(updateOrganization.Id);
                organization.Name = updateOrganization.Name;
                organization.Main_office = updateOrganization.Main_office;
                organization.Representative = updateOrganization.Representative;
                organization.PhoneNumber = updateOrganization.PhoneNumber;
                organization.Email = updateOrganization.Email;
                organization.Logo = await FileHelper.SaveImageAndReturnShortPathAsync(updateOrganization.LogoFile, FolderImage.OrganizationImageUpload, organization.Logo) ?? organization.Logo;
                organization.Description = updateOrganization.Description;
                organization.Vision = updateOrganization.Vision;
                organization.Mission = updateOrganization.Mission;
                organization.CoreValue = updateOrganization.CoreValue;
                organization.MainActivity = updateOrganization.MainActivity;
                organization.Interests = updateOrganization.Interests;
                organization.VolunteerExperience = updateOrganization.VolunteerExperience;
                organization.VolunteerObjectives = updateOrganization.VolunteerObjectives;
                organization.Bio = updateOrganization.Bio;
                checkValid = await _organizationRepository.UpdateAsync(organization);
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"UpdateOrganizationAsync: {ex.Message}");
            }

            return checkValid;
        }
    }
}
