using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.DTOs.CampaignDTO;
using FALOFinancialProofing.DTOs.CampaignMemberDTO;
using FALOFinancialProofing.DTOs.RoleDTOs;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using FALOFinancialProofing.Services.CampaignService;
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
        private readonly ICampaignService campaignService;
        public CampaignMemberService(IRepository<CampaignMember, int> _cmRepository, IRepository<Campaign, int> campaignRepository, AuthServices authServices, ICampaignService campaignService)
        {
            cmRepository = _cmRepository;
            this.campaignRepository = campaignRepository;
            this.authServices = authServices;
            this.campaignService = campaignService;
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
                        Email = cm.User.Email,
                        roleInformation = new RoleInformation()
                        {
                            RoleId = cm.RoleId,
                            RoleName = cm.Role.Name
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
                campaignMembers = await cmRepository.GetAll().Where(cm => cm.UserId.Equals(userId) && roleId.Equals(roleId)).OrderBy(cm => cm.Id).Select(cm => new CampaignMemberInformation()
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
                    Email = cm.User.Email,
                    roleInformation = new RoleInformation()
                    {
                        RoleId = cm.RoleId,
                        RoleName = cm.Role.Name
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
                        Email = cm.User.Email,
                        roleInformation = new RoleInformation()
                        {
                            RoleId = cm.RoleId,
                            RoleName = cm.Role.Name
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
                        Email = cm.User.Email,
                        roleInformation = new RoleInformation()
                        {
                            RoleId = cm.RoleId,
                            RoleName = cm.Role.Name
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
                        Email = cm.User.Email,
                        roleInformation = new RoleInformation()
                        {
                            RoleId = cm.RoleId,
                            RoleName = cm.Role.Name
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
                var campaignOwner = await campaignService.GetCampaignByUserIdAndCampaignIdAsync(updateCampaignMemberStatusDTO.PmUserId, updateCampaignMemberStatusDTO.CampaignId);
                bool checkAdmin = await authServices.CheckUserInRole(updateCampaignMemberStatusDTO.PmUserId, AppRole.Admin, new StringBuilder());
                bool checkPMB = await authServices.CheckUserInRole(updateCampaignMemberStatusDTO.PmUserId, AppRole.ProjectManagementBoard, new StringBuilder());
                if ((campaignOwner == null && !checkPMB && !checkAdmin) || (campaignOwner != null && !campaignOwner.IsActive))
                {
                    throw new Exception("You don't have permission to update campaign!");
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

        public async Task<List<CreateManyCampaignMemberDTO>> ValidateCampaignMembersCreateAsync(List<CreateManyCampaignMemberDTO> createManyCampaignMemberDTOs, int campaignId, string pmUserId, StringBuilder message)
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
                if (!campaign.IsActive)
                {
                    throw new Exception($"Campaign is not active.");
                }
                // chỉ người tạo hoặc pmb mới có quyền add người vào chiến dịch
                //var campaignMember = await cmRepository.Get(cm=>cm.CampaignId==campaignId&&pmUserId.Equals(cm.))
                var pmUser = await campaignRepository.Get(c => c.Id == campaignId && c.CreateBy.Equals(pmUserId));
                // kiểm tra thằng add này có phải là pmb không
                var checkPMB = await authServices.CheckUserInRoleId(pmUserId, AppRole.ProjectManagementBoard, new StringBuilder());
                var checkAdmin = await authServices.CheckUserInRoleId(pmUserId, AppRole.Admin, new StringBuilder());
                // người dùng không tạo ra chiến dịch, hoặc tạo ra nhưng bị vô hiệu hóa hoặc không phải là pmb hoặc admin
                if ((pmUser == null && !checkPMB && !checkAdmin) || (pmUser != null && !pmUser.IsActive))
                {
                    throw new Exception("You do not have permission to add members to this campaign.");
                }
                var DbData = await cmRepository.GetAll().Include(cm => cm.Role)
                    .Where(x => x.CampaignId == campaignId)
                    .ToListAsync(); //**
                                    //var isExist = DbData.Any(cm => createManyCampaignMemberDTOs.Any(cmd => cmd.UserId == cm.UserId));
                var checkExistFailData = false;
                foreach (var item in createManyCampaignMemberDTOs)
                {
                    // phải check ở đây vì kiểm tra trong từng lần add
                    var checkExistActiveAccounting = DbData.Any(cm => cm.Role.Name.Equals(AppRole.Accounting) && cm.IsActive);
                    var checkUserInRole = await authServices.CheckUserInRoleId(item.UserId, item.RoleId, new StringBuilder());
                    if (!checkUserInRole)
                    {
                        checkExistFailData = true;
                        continue;
                    }
                    var isExist = DbData.Any(cm => cm.UserId.Equals(item.UserId));
                    if (isExist)
                    {
                        checkExistFailData = true;
                        continue;
                    }
                    // nếu trong campaigin có accountting rồi và ở trạng thái is active thì không được add accountting nữa
                    bool checkIsAccountingRole = await authServices.CheckIsAccountingRole(item.RoleId, new StringBuilder());
                    if (checkExistActiveAccounting && checkIsAccountingRole)
                    {
                        checkExistFailData = true;
                        continue;
                    }
                    bool checkNotAllowRole = await authServices.CheckIsDonorAdminPmbPmRole(item.RoleId, new StringBuilder());
                    if (checkNotAllowRole)
                    {
                        checkExistFailData = true;
                        continue;
                    }
                    successDatas.Add(item);
                }
                if (checkExistFailData)
                {
                    message.Append($"Not Valid User(s) Occurs! ");
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
