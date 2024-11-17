using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.DTOs.CampaignDTO;
using FALOFinancialProofing.DTOs.CampaignMemberDTO;
using FALOFinancialProofing.DTOs.RoleDTOs;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;

namespace FALOFinancialProofing.Services.CampaignMemberService
{
    public class CampaignMemberService : ICampaignMemberService
    {
        private readonly IRepository<CampaignMember, int> cmRepository;
        private readonly IRepository<Campaign, int> campaignRepository;
        private readonly AuthServices authServices;
        public CampaignMemberService(IRepository<CampaignMember, int> _cmRepository, IRepository<Campaign, int> campaignRepository, AuthServices authServices)
        {
            cmRepository = _cmRepository;
            this.campaignRepository = campaignRepository;
            this.authServices = authServices;
        }

        public async Task<CampaignMember?> CreateCampaignMemberAsync(CreateCampaignMemberDTO createCampaignMemberDTO)
        {
            try
            {
                var existingCampaignMember = await cmRepository.Get(x => x.CampaignId == createCampaignMemberDTO.CampaignId && x.UserId == createCampaignMemberDTO.UserId);

                if (existingCampaignMember != null)
                {
                    return null;
                }

                var newCampaignMember = await CreateCampaignMemberDTOToEntity(createCampaignMemberDTO);
                return await cmRepository.InsertAsync(newCampaignMember);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"CreateCampaignMemberAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<CampaignMember?> CreateCampaignMemberAsync(CampaignMember campaignMember)
        {
            try
            {
                var existingCampaignMember = await cmRepository.Get(x => x.CampaignId == campaignMember.CampaignId && x.UserId == campaignMember.UserId);

                if (existingCampaignMember != null)
                {
                    return null;
                }

                return await cmRepository.InsertAsync(campaignMember);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        private async Task<CampaignMember> CreateCampaignMemberDTOToEntity(CreateCampaignMemberDTO createCampaignMemberDTO)
        {
            return new CampaignMember
            {
                CampaignId = createCampaignMemberDTO.CampaignId,
                UserId = createCampaignMemberDTO.UserId
            };
        }

        public async Task<List<CampaignMemberInformation>> GetAllCampaignMembersAsync()
        {
            var campaignMembers = new List<CampaignMemberInformation>();
            try
            {
                campaignMembers = await cmRepository.GetAll()
                    .Select(cm => new CampaignMemberInformation()
                    {
                        id = cm.Id,
                        UserId = cm.UserId,
                        UserName = cm.User.UserName,
                        FirstName = cm.User.FirstName,
                        LastName = cm.User.LastName,
                        CampaignId = cm.CampaignId,
                        CampaignTitle = cm.Campaign.Title,
                        Debt = cm.Debt,
                        IsActive = cm.IsActive,
                        roleInformation = new RoleInformation()
                        {
                            RoleId = cm.RoleId,
                            RoleName = cm.IdentityRole.Name
                        }
                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllCampaignMembersAsync: {ex.Message}");
            }
            return campaignMembers;
        }

        public async Task<List<CampaignMemberInformation>> GetAllCampaignMemberByUserIdAndRoleIdAsync(string userId, string roleId)
        {
            var campaignMembers = new List<CampaignMemberInformation>();
            try
            {
                campaignMembers = await cmRepository.GetAll().Where(cm => cm.UserId.Equals(userId) && roleId.Equals(roleId)).Select(cm => new CampaignMemberInformation()
                {
                    id = cm.Id,
                    UserId = cm.UserId,
                    UserName = cm.User.UserName,
                    FirstName = cm.User.FirstName,
                    LastName = cm.User.LastName,
                    CampaignId = cm.CampaignId,
                    CampaignTitle = cm.Campaign.Title,
                    Debt = cm.Debt,
                    ProjectName = cm.Campaign.Project.ProjectName,
                    FundTarget = cm.Campaign.FundTarget,
                    Status = cm.Campaign.Status,
                    IsActive = cm.IsActive,
                    roleInformation = new RoleInformation()
                    {
                        RoleId = cm.RoleId,
                        RoleName = cm.IdentityRole.Name
                    }
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllCampaignMemberByUserIdAndRoleIdAsync: {ex.Message}");
            }
            return campaignMembers;
        }


        public async Task<List<CampaignMemberInformation>> GetAllCampaignMemberByUserIdAsync(string userId)
        {
            var campaignMembers = new List<CampaignMemberInformation>();
            try
            {
                campaignMembers = await cmRepository.GetAll()
                    .Where(cm => cm.UserId.Equals(userId) && !string.IsNullOrEmpty(cm.Campaign.Status) && !cm.Campaign.Status.Equals(RequestStatus.Rejected))
                    .Select(cm => new CampaignMemberInformation()
                    {
                        id = cm.Id,
                        UserId = cm.UserId,
                        UserName = cm.User.UserName,
                        FirstName = cm.User.FirstName,
                        LastName = cm.User.LastName,
                        CampaignId = cm.CampaignId,
                        CampaignTitle = cm.Campaign.Title,
                        Debt = cm.Debt,
                        IsActive = cm.IsActive,
                        FundTarget = cm.Campaign.FundTarget,
                        ProjectName = cm.Campaign.Project.ProjectName,
                        roleInformation = new RoleInformation()
                        {
                            RoleId = cm.RoleId,
                            RoleName = cm.IdentityRole.Name
                        }
                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllCampaignMemberByUserIdAndRoleIdAsync: {ex.Message}");
            }
            return campaignMembers;
        }
        public async Task<CampaignMemberInformation?> GetCampaignMemberByIdAsync(int id)
        {
            try
            {
                var campaignMember = new CampaignMemberInformation();
                try
                {
                    campaignMember = await cmRepository.GetAll().Where(cm => cm.Id == id).Select(cm => new CampaignMemberInformation()
                    {
                        id = cm.Id,
                        UserId = cm.UserId,
                        UserName = cm.User.UserName,
                        FirstName = cm.User.FirstName,
                        LastName = cm.User.LastName,
                        CampaignId = cm.CampaignId,
                        CampaignTitle = cm.Campaign.Title,
                        Debt = cm.Debt,
                        IsActive = cm.IsActive,
                        roleInformation = new RoleInformation()
                        {
                            RoleId = cm.RoleId,
                            RoleName = cm.IdentityRole.Name
                        }
                    }).SingleOrDefaultAsync();
                }
                catch (Exception ex)
                {
                    await Console.Out.WriteLineAsync($"GetAllCampaignMemberByUserIdAndRoleIdAsync: {ex.Message}");
                }
                return campaignMember;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<List<CampaignMemberInformation>> GetAllCampaignMemberByCampaignIdAsync(int campaignId)
        {
            var campaignMembers = new List<CampaignMemberInformation>();
            try
            {
                campaignMembers = await cmRepository.GetAll()
                    .Where(cm => cm.CampaignId == campaignId)
                    .Select(cm => new CampaignMemberInformation()
                    {
                        id = cm.Id,
                        UserId = cm.UserId,
                        UserName = cm.User.UserName,
                        FirstName = cm.User.FirstName,
                        LastName = cm.User.LastName,
                        CampaignId = cm.CampaignId,
                        CampaignTitle = cm.Campaign.Title,
                        Debt = cm.Debt,
                        IsActive = cm.IsActive,
                        roleInformation = new RoleInformation()
                        {
                            RoleId = cm.RoleId,
                            RoleName = cm.IdentityRole.Name
                        }
                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllCampaignMemberByCampaignIdAsync: {ex.Message}");
            }
            return campaignMembers;
        }
        //?
        public async Task<CampaignMember?> GetCampaignMemberByUserIdAsync(string userid)
        {
            try
            {
                return await cmRepository.Get(x => x.UserId.Equals(userid));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> UpdateCampaignMemberStatusAsync(UpdateCampaignMemberStatusDTO updateCampaignMemberStatusDTO, StringBuilder message)
        {
            try
            {
                var existingCampaignMember = await cmRepository.Get(updateCampaignMemberStatusDTO.Id);

                if (existingCampaignMember == null)
                {
                    throw new Exception("CampaignMember not found.");
                }
                if (existingCampaignMember.Debt != 0)
                {
                    throw new Exception("Cannot deactivate CampaignMember with debt greater than 0.");
                }
                UpdateCampaignMemberStatusDTOToEntity(existingCampaignMember, updateCampaignMemberStatusDTO);

                return await cmRepository.UpdateAsync(existingCampaignMember);
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"UpdateCampaignMemberStatusAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateCampaignMemberAsync(UpdateCampaignMemberDTO updateCampaignMemberDTO)
        {
            try
            {
                var existingCampaignMember = await cmRepository.Get(updateCampaignMemberDTO.Id);

                if (existingCampaignMember == null)
                {
                    return false;
                }

                UpdateCampaignMemberDTOToEntity(existingCampaignMember, updateCampaignMemberDTO);

                return await cmRepository.UpdateAsync(existingCampaignMember);
            }
            catch (Exception e)
            {
                return false;
            }
        }
        //public async Task<bool> UpdateCampaignMemberAsync(CampaignMember campaignMember)
        //{
        //    try
        //    {
        //        var existingCampaignMember = await cmRepository.Get(campaignMember.Id);

        //        if (existingCampaignMember == null)
        //        {
        //            return false;
        //        }



        //        return await cmRepository.UpdateAsync(existingCampaignMember);
        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }
        //}

        private void UpdateCampaignMemberDTOToEntity(CampaignMember campaignMember, UpdateCampaignMemberDTO updateCampaignMemberDTO)
        {
            campaignMember.Debt = updateCampaignMemberDTO.Debt;
            campaignMember.IsActive = updateCampaignMemberDTO.IsActive;
        }
        private void UpdateCampaignMemberStatusDTOToEntity(CampaignMember campaignMember, UpdateCampaignMemberStatusDTO updateCampaignMemberStatusDTO)
        {
            campaignMember.IsActive = updateCampaignMemberStatusDTO.IsActive;
        }
        public async Task<bool> DeleteCampaignMemberByIdAsync(int id)
        {
            try
            {
                var existingCampaignMember = await cmRepository.Get(x => x.Id == id);
                if (existingCampaignMember == null) return false;

                return await cmRepository.DeleteAsync(existingCampaignMember);
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public CampaignMember ConvertToBaseClass(CreateManyCampaignMemberDTO createManyCampaignMemberDTO, int CampaignId)
        {
            return new CampaignMember()
            {
                CampaignId = CampaignId,
                UserId = createManyCampaignMemberDTO.UserId,
                RoleId = createManyCampaignMemberDTO.RoleId,
                Debt = 0,
                IsActive = true
            };
        }

        public async Task<List<CreateManyCampaignMemberDTO>> ValidateCampaignMembersCreateAsync(List<CreateManyCampaignMemberDTO> createManyCampaignMemberDTOs, int campaignId, StringBuilder message)
        {
            List<CreateManyCampaignMemberDTO> successDatas = new List<CreateManyCampaignMemberDTO>();
            try
            {
                // check campaign Exist
                var campaign = await campaignRepository.Get(campaignId);
                if (campaign == null)
                {
                    throw new Exception($"Campaign not found with id = {campaignId}.");
                }
                var DbData = await cmRepository.GetAll()
                    .Where(x => x.CampaignId == campaignId)
                    .ToListAsync();
                //var isExist = DbData.Any(cm => createManyCampaignMemberDTOs.Any(cmd => cmd.UserId == cm.UserId));
                var checkExistFailData = false;
                foreach (var item in createManyCampaignMemberDTOs)
                {
                    var checkUserInRole = await authServices.CheckUserInRoleId(item.UserId, item.RoleId, message);
                    var isExist = DbData.Any(cm => cm.UserId == item.UserId);
                    if (isExist || !checkUserInRole)
                    {
                        checkExistFailData = true;
                    }
                    else
                    {
                        successDatas.Add(item);
                    }

                }
                if (checkExistFailData)
                {
                    message.Append($"Users already exist in Campaign with Id = {campaignId}.");
                }
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"ValidateCampaignMembersCreateAsync: {ex.Message}");
            }

            return successDatas;
        }

        public async Task<List<CreateManyCampaignMemberDTO>> InValidCampaignMembersCreateAsync(List<CreateManyCampaignMemberDTO> createManyCampaignMemberDTOs, List<CreateManyCampaignMemberDTO> ValidCreateManyCampaignMemberDTOs)
        {
            List<CreateManyCampaignMemberDTO> InValidDatas = new List<CreateManyCampaignMemberDTO>();
            try
            {
                InValidDatas = createManyCampaignMemberDTOs.Except(ValidCreateManyCampaignMemberDTOs).ToList();
            }
            catch (Exception ex)
            {

                await Console.Out.WriteLineAsync($"ValidateCampaignMembersCreateAsync: {ex.Message}");
            }

            return InValidDatas;
        }
        public async Task<bool> CreateManyCampaignMembersAsync(List<CreateManyCampaignMemberDTO> createManyCampaignMemberDTOs, int campaignId, StringBuilder message)
        {
            var IsValid = false;
            try
            {
                if (createManyCampaignMemberDTOs == null || createManyCampaignMemberDTOs.Count == 0)
                {
                    throw new Exception("No data to create.");
                }
                List<CampaignMember> data = new List<CampaignMember>();
                foreach (var item in createManyCampaignMemberDTOs)
                {
                    data.Add(ConvertToBaseClass(item, campaignId));
                }
                IsValid = await cmRepository.InsertManyAsync(data);
                if (IsValid)
                    message.Append("Create CampaignMembers Successfully!");
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"CreateManyCampaignMembersAsync: {ex.Message}");
            }
            return IsValid;
        }

    }
}
