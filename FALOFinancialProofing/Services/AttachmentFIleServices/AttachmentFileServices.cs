﻿using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace FALOFinancialProofing.Services.AttachmentFIleServices
{
    public class AttachmentFileServices : IAttachmentFileServices
    {
        private readonly IRepository<AttachmentFile, int> repository;

        public AttachmentFileServices(IRepository<AttachmentFile, int> repository)
        {
            this.repository = repository;
        }


        public async Task<bool> CreateManyAttachmentFileAsync(List<AttachmentFileRequest> dtos)
        {
            try
            {
                var newAttachmentFiles = await ListDTOToListEntity(dtos);
                return await repository.InsertManyAsync(newAttachmentFiles);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<List<AttachmentFile>> ListDTOToListEntity(List<AttachmentFileRequest> dtos)
        {
            var attachmentFiles = new List<AttachmentFile>();
            foreach(var dto in dtos)
            {
                attachmentFiles.Add(await DTOToEntity(dto));
            }
            return attachmentFiles;
        }

        private async Task<AttachmentFile> DTOToEntity(AttachmentFileRequest dto)
        {
            return new AttachmentFile
            {
                Id = dto.Id != null ? dto.Id.Value : 0,
                FilePath = dto.FilePath,
                RequestId = dto.RequestId
            };
        }

        public async Task<bool> DeleteRequestFormAsync(AttachmentFileRequest dto)
        {
            try
            {
                var deletedAttachmentFiles = await repository.Get(x => x.Id == dto.Id);
                if (deletedAttachmentFiles == null) return false;

                return await repository.DeleteAsync(deletedAttachmentFiles);
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
                var deletedAttachmentFiles = await repository.Get(x => x.Id == id);
                if (deletedAttachmentFiles == null) return false;

                return await repository.DeleteAsync(deletedAttachmentFiles);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<AttachmentFile>> GetAllAttachmentFilesAsync()
        {
            try
            {
                return await repository.GetAll().Include(x => x.RequestForm)
                    .Select(af => new AttachmentFile
                    {
                        Id = af.Id,
                        FilePath = af.FilePath,
                        RequestId = af.RequestId,
                        RequestForm = new RequestForm 
                        {
                            Id = af.RequestForm.Id,
                            CreateAt = af.RequestForm.CreateAt,
                            Description = af.RequestForm.Description,
                        }
                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<AttachmentFile>();
            }
        }

        public async Task<AttachmentFile?> GetAttachmentFileByIdAsync(int id)
        {
            try
            {
                return await repository.GetAll(x => x.Id == id).Include(x => x.RequestForm)
                    .Select(af => new AttachmentFile
                    {
                        Id = af.Id,
                        FilePath = af.FilePath,
                        RequestId = af.RequestId,
                        RequestForm = new RequestForm
                        {
                            Id = af.RequestForm.Id,
                            CreateAt = af.RequestForm.CreateAt,
                            Description = af.RequestForm.Description,
                        }
                    }).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<AttachmentFile>?> GetAttachmentFileByRequestIdAsync(int requestId)
        {
            try
            {
                return await repository.GetAll(x => x.RequestId == requestId).Include(x => x.RequestForm)
                    .Select(af => new AttachmentFile
                    {
                        Id = af.Id,
                        FilePath = af.FilePath,
                        RequestId = af.RequestId,
                        RequestForm = new RequestForm
                        {
                            Id = af.RequestForm.Id,
                            CreateAt = af.RequestForm.CreateAt,
                            Description = af.RequestForm.Description,
                        }
                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> UpdateRequestFormAsync(AttachmentFileRequest dto)
        {
            try
            {
                var updatedRequestForm = await DTOToEntity(dto);
                return await repository.UpdateAsync(updatedRequestForm);
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
