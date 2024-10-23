using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Extensions;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FALOFinancialProofing.Services.RequestFormServices
{
    public class RequestFormServices : IRequestFormServices
    {
        private readonly IRepository<RequestForm, int> repository;
        private readonly IRepository<Campaign, int> campaignRepository;
        private readonly IRepository<CampaignMember, int> campaignMemberRepository;
        public RequestFormServices(IRepository<RequestForm, int> repository, 
            IRepository<Campaign, int> campaignRepository,
            IRepository<CampaignMember, int> campaignMemberRepository
            )
        {
            this.repository = repository;
            this.campaignRepository = campaignRepository;
            this.campaignMemberRepository = campaignMemberRepository;   
        }

        public async Task<RequestForm?> CreateRequestFormAsync(RequestFormDTO dto)
        {
            try
            {
                var newRequestForm = await DTOToEntity(dto);
                return await repository.InsertAsync(newRequestForm);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<RequestForm?> CreateRequestFormAsync(RequestFormInformation dto)
        {
            try
            {
                var newRequestForm = await DTOToEntity(dto);
                return await repository.InsertAsync(newRequestForm);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<RequestForm> DTOToEntity(RequestFormInformation dto)
        {
            return new RequestForm
            {
                Id = dto.Id != null ? dto.Id.Value : 0,
                CreateAt = dto.CreateAt,
                Description = dto.Description,
                ExpectedMoney = dto.ExpectedMoney,
                Status = dto.Status,
                CreatedBy = dto.CreatedBy,
                CampaignId = dto.CampaignId,
                TypeId = dto.TypeId,
            };
        }

        private async Task<RequestForm> DTOToEntity(RequestFormDTO dto)
        {
            return new RequestForm
            {
                Id = dto.Id != null ? dto.Id.Value : 0,
                CreateAt = dto.CreateAt,
                Description = dto.Description,
                ExpectedMoney = dto.ExpectedMoney,
                Status = dto.Status,
                CreatedBy = dto.CreatedBy,
                CampaignId = dto.CampaignId,
                TypeId = dto.TypeId,
                AttachmentFiles = dto.AttachmentFiles,
                ApproveProcesses = dto.ApproveProcesses
            };
        }

        public async Task<bool> DeleteRequestFormAsync(RequestFormDTO dto)
        {
            try
            {
                var requestForm = await repository.Get(x => x.Id == dto.Id);
                if(requestForm == null) return false;

                return await repository.DeleteAsync(requestForm);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteRequestFormByIdAsync(int id)
        {
            try
            {
                var requestForm = await repository.Get(x => x.Id == id);
                if (requestForm == null) return false;

                return await repository.DeleteAsync(requestForm);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<RequestForm>> GetAllRequestFormsAsync()
        {
            try
            {
                return await repository.GetAll()
                    .Include(x => x.User)
                    .Include(x => x.Campaign)
                    .Include(x => x.RequestType)
                    .Select(rf => new RequestForm
                    {
                        Id = rf.Id,
                        Description = rf.Description,
                        ExpectedMoney = rf.ExpectedMoney,
                        CreateAt = rf.CreateAt,
                        Status = rf.Status,
                        User = new User
                        {
                            Id = rf.User.Id,
                            FirstName = rf.User.FirstName,
                            LastName = rf.User.LastName,
                        },
                        CreatedBy = rf.User.Id,
                        Campaign = new Campaign
                        {
                            Id = rf.Campaign.Id
                        },
                        CampaignId = rf.Campaign.Id,
                        RequestType = new RequestType
                        {
                            Id = rf.RequestType.Id,
                            TypeName = rf.RequestType.TypeName
                        }
                    }).ToListAsync();
            }
            catch (Exception e)
            {
                return new List<RequestForm>();
            }
        }

        public async Task<RequestForm?> GetRequestFormByIdAsync(int id)
        {
            try
            {
                return await repository.GetAll(x => x.Id == id)
                    .Include(x => x.User)
                    .Include(x => x.Campaign)
                    .Include(x => x.RequestType)
                    .Select(rf => new RequestForm
                    {
                        Id = rf.Id,
                        Description = rf.Description,
                        ExpectedMoney = rf.ExpectedMoney,
                        CreateAt = rf.CreateAt,
                        Status = rf.Status,
                        User = new User
                        {
                            Id = rf.User.Id,
                            FirstName = rf.User.FirstName,
                            LastName = rf.User.LastName,
                        },
                        CreatedBy = rf.User.Id,
                        Campaign = new Campaign
                        {
                            Id = rf.Campaign.Id
                        },
                        CampaignId = rf.Campaign.Id,
                        RequestType = new RequestType
                        {
                            Id = rf.RequestType.Id,
                            TypeName = rf.RequestType.TypeName
                        }
                    }).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<RequestForm>?> GetRequestFormByUserIdAsync(string userId)
        {
            try
            {
                return await repository.GetAll(x => x.CreatedBy.Equals(userId))
                    .Include(x => x.User)
                    .Include(x => x.Campaign)
                    .Include(x => x.RequestType)
                    .Select(rf => new RequestForm
                    {
                        Id = rf.Id,
                        Description = rf.Description,
                        ExpectedMoney = rf.ExpectedMoney,
                        CreateAt = rf.CreateAt,
                        Status = rf.Status,
                        User = new User
                        {
                            Id = rf.User.Id,
                            FirstName = rf.User.FirstName,
                            LastName = rf.User.LastName,
                        },
                        CreatedBy = rf.User.Id,
                        Campaign = new Campaign
                        {
                            Id = rf.Campaign.Id
                        },
                        CampaignId = rf.Campaign.Id,
                        RequestType = new RequestType
                        {
                            Id = rf.RequestType.Id,
                            TypeName = rf.RequestType.TypeName
                        }
                    }).ToListAsync();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> UpdateRequestFormAsync(RequestFormDTO dto)
        {
            try
            {
                var updatedRequestForm = await DTOToEntity(dto);
                return await repository.UpdateAsync(updatedRequestForm);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<CreateFormRequest> ValidateRequestForm(CreateFormRequest requestFormRequest, System.Text.StringBuilder message)
        {
            if (ValidatedRequestForm(requestFormRequest, message))
            {
                requestFormRequest.IsValidate = true;
                return requestFormRequest;
            }
            else
            {
                requestFormRequest.IsValidate = false;
                return requestFormRequest;
            }
        }

        private bool ValidatedRequestForm(CreateFormRequest requestForm, System.Text.StringBuilder message)
        {
            if(requestForm == null || requestForm.ExpectedMoney == null || requestForm.ApproverId == null)
            {
                message.Append("Expected money and ApproverId cannot be null");
                return false;
            }
            //check campaign id exist?
            var campainInDb = campaignRepository.GetAll(cp => cp.Id == StringExtension.ParseStringToInt(requestForm.CampaignId))
                .Include(cp => cp.CampaignMembers)
                .FirstOrDefault();
            if (campainInDb == null)
            {
                message.Append("CampaignID is not exist");
                return false;
            }
            //check createBy id exist, in campaign
            var createByValidate = campainInDb.CampaignMembers.Any(cm => cm.UserId == requestForm.CreatedBy);
            if(!createByValidate)
            {
                message.Append("CreateBy is not exist or maybe in wrong campaign");
                return false;
            }
            // check ApproverId exist in campain && ApproverId != CreatedBy && Role of ApproverId is greater than CreatedBy
            if(requestForm.ApproverId == requestForm.CreatedBy)
            {
                message.Append("Approver ID cannot equal CreatBy");
                return false;
            }
            var approveByValidate = campainInDb.CampaignMembers.Any(cm => cm.UserId == requestForm.ApproverId);
            if (!approveByValidate)
            {
                message.Append("ApproverId is not exist or maybe in wrong campaign");
                return false;
            }
            // Compare role of ApproverID and CreatedBy (voluntear, leader, accounting)
            var haveEnoughPermission = CheckPermission(requestForm.CreatedBy, requestForm.ApproverId);
            if (!haveEnoughPermission)
            {
                message.Append("The selected Approver don have enough permission");
                return false;
            }

            if (requestForm.ExpectedMoney < 0)
            {
                message.Append("Expected money must be greater than 0");
                return false;
            }
            return true;
        }

        private bool CheckPermission(string createdBy, string approverId)
        {
            // 2 case that (volunteer, leader, accounting) and PM
            var createByRole = campaignRepository.GetAll(cp => cp.CampaignMembers.Any(cm => cm.UserId == createdBy))
                .Include(cp => cp.CampaignMembers)
                .ThenInclude(x => x.IdentityRole)
                .FirstOrDefault()
                .CampaignMembers
                .FirstOrDefault(cm => cm.UserId == createdBy)
                .IdentityRole.Name;
            var approverRole = campaignMemberRepository.GetAll(x => x.UserId == approverId)
                    .Include(x => x.IdentityRole)
                    .FirstOrDefault()
                    .IdentityRole.Name;
            if (createByRole.Equals("Project Managerment"))
            {
                // check approveId role is accoungting
                if (approverRole.Equals("Accounting"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            } else
            {
                // check approveId role is greater than createBy
                if(createByRole.Equals("Volunteer") && approverRole.Equals("Volunteer Leader"))
                {
                    return true;
                }
                if(createByRole.Equals("Volunteer Leader") && approverRole.Equals("Accounting"))
                {
                    return true;
                }
                if(createByRole.Equals("Accounting") && approverRole.Equals("Project Managerment"))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<List<AttachmentFileRequest>> SaveUploadedFilesAsync(List<IFormFile> uploadFiles, int requestId)
        {
            var attachmentFiles = new List<AttachmentFileRequest>();
            try
            {
                if (uploadFiles != null && uploadFiles.Any())
                {
                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "PrePayUploads");

                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    foreach (var file in uploadFiles)
                    {
                        var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";

                        var filePath = Path.Combine(uploadFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var attachmentFileDTO = new AttachmentFileRequest
                        {
                            FilePath = uniqueFileName,  
                            RequestId = requestId
                        };
                        attachmentFiles.Add(attachmentFileDTO);
                    }
                }

                return attachmentFiles;
            }
            catch (Exception ex)
            {
                return attachmentFiles;
            }
        }

        public async Task<List<UserWithRole>> GetApproverListForVolunteer(int campaignId)
        {
            var approverList = new List<UserWithRole>();
            try
            {
                approverList = await campaignMemberRepository.GetAll(x => x.CampaignId == campaignId)
                    .Include(x => x.User)
                    .Include(x => x.IdentityRole)
                    .Where(x => x.IdentityRole.Name == "Volunteer Leader")
                    .Select(x => new UserWithRole
                    {
                        UserId = x.UserId,
                        FullName = $"{x.User.FirstName} {x.User.LastName}",
                        RoleId = x.RoleId,
                        RoleName = x.IdentityRole.Name
                    })
                    .ToListAsync();
                return approverList;
            }
            catch (Exception ex)
            {
                return approverList;
            }
        }

        public async Task<UserWithRole?> GetApproverForVolunteerLeader(int campaignId)
        {
            try
            {
                var approverForLeader = await campaignMemberRepository.GetAll(x => x.CampaignId == campaignId)
                    .Include(x => x.User)
                    .Include(x => x.IdentityRole)
                    .Where(x => x.IdentityRole.Name == "Accounting")
                    .Select(x => new UserWithRole
                    {
                        UserId = x.UserId,
                        FullName = $"{x.User.FirstName} {x.User.LastName}",
                        RoleId = x.RoleId,
                        RoleName = x.IdentityRole.Name
                    })
                    .FirstOrDefaultAsync();
                return approverForLeader;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<UserWithRole?> GetApproverForAccounting(int campaignId)
        {
            try
            {
                var approverForLeader = await campaignMemberRepository.GetAll(x => x.CampaignId == campaignId)
                    .Include(x => x.User)
                    .Include(x => x.IdentityRole)
                    .Where(x => x.IdentityRole.Name == "Project Management")
                    .Select(x => new UserWithRole
                    {
                        UserId = x.UserId,
                        FullName = $"{x.User.FirstName} {x.User.LastName}",
                        RoleId = x.RoleId,
                        RoleName = x.IdentityRole.Name
                    })
                    .FirstOrDefaultAsync();
                return approverForLeader;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<UserWithRole?> GetApproverForProjectManagement(int campaignId)
        {
            try
            {
                var approverForLeader = await campaignMemberRepository.GetAll(x => x.CampaignId == campaignId)
                    .Include(x => x.User)
                    .Include(x => x.IdentityRole)
                    .Where(x => x.IdentityRole.Name == "Accounting")
                    .Select(x => new UserWithRole
                    {
                        UserId = x.UserId,
                        FullName = $"{x.User.FirstName} {x.User.LastName}",
                        RoleId = x.RoleId,
                        RoleName = x.IdentityRole.Name
                    })
                    .FirstOrDefaultAsync();
                return approverForLeader;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<VoucherRequest>> SaveUploadedVoucherAsync(int approveId, List<IFormFile> files)
        {
            var voucherFiles = new List<VoucherRequest>();
            try
            {
                if (files != null && files.Any())
                {
                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Vouchers");

                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    foreach (var file in files)
                    {
                        var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";

                        var filePath = Path.Combine(uploadFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var voucherDTO = new VoucherRequest
                        {
                            FilePath = uniqueFileName,
                            ApproveId = approveId
                        };
                        voucherFiles.Add(voucherDTO);
                    }
                }

                return voucherFiles;
            }
            catch (Exception ex)
            {
                return voucherFiles;
            }
        }
    }
}
