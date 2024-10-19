﻿using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.EntityFrameworkCore;

namespace FALOFinancialProofing.Services.RequestFormServices
{
    public class RequestFormServices : IRequestFormServices
    {
        private readonly IRepository<RequestForm, int> repository;
        private readonly IRepository<Campaign, int> campaignRepository;

        public RequestFormServices(IRepository<RequestForm, int> repository, IRepository<Campaign, int> campaignRepository)
        {
            this.repository = repository;
            this.campaignRepository = campaignRepository;
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

        public async Task<CreateFormRequest> ValidateRequestForm(CreateFormRequest requestFormRequest)
        {
            if (ValidatedRequestForm(requestFormRequest))
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


        private bool ValidatedRequestForm(CreateFormRequest requestForm)
        {
            if(requestForm == null || requestForm.ExpectedMoney == null || requestForm.ApproverId == null)
            {
                return false;
            }
            //check campaign id exist?
            var campainInDb = campaignRepository.GetAll(cp => cp.Id == requestForm.CampaignId)
                .Include(cp => cp.CampaignMembers)
                .FirstOrDefault();
            if (campainInDb == null)
            {
                return false;
            }
            //check createBy id exist, in campain
            var createByValidate = campainInDb.CampaignMembers.Any(cm => cm.UserId == requestForm.CreatedBy);
            if(!createByValidate)
            {
                return false;
            }
            // check ApproverId exist in campain
            var approveByValidate = campainInDb.CampaignMembers.Any(cm => cm.UserId == requestForm.ApproverId);
            if (!createByValidate)
            {
                return false;
            }

            if (requestForm.ExpectedMoney < 0)
            {
                return false;
            }
            return true;
        }

        public async Task<List<AttachmentFileRequest>> SaveUploadedFilesAsync(List<IFormFile> uploadFiles, int requestId)
        {
            var attachmentFiles = new List<AttachmentFileRequest>();
            try
            {
                if (uploadFiles != null && uploadFiles.Any())
                {
                    foreach (var file in uploadFiles)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "PrePayUploads", file.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        var attachmentFileDTO = new AttachmentFileRequest
                        {
                            FilePath = file.FileName,
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
    }
}
