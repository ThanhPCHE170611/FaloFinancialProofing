using FALOFinancialProofing.Constant;
using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace FALOFinancialProofing.Services.AttachmentFIleServices
{
    public class AttachmentFileServices : IAttachmentFileServices
    {
        private readonly IRepository<AttachmentFile, int> repository;
        private readonly IRepository<RequestForm, int> requestFormRepository;

        public AttachmentFileServices(IRepository<AttachmentFile, int> repository,
            IRepository<RequestForm, int> requestFormRepository)
        {
            this.repository = repository;
            this.requestFormRepository = requestFormRepository;
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
        public async Task<(byte[] fileBytes, string contentType, string fileName)> DownloadPrePayAttachmentFileByFileName(string fileName)
        {
            try
            {
                // Đường dẫn đầy đủ đến file trong server
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "PrePayUploads", fileName);

                // Kiểm tra xem file có tồn tại không
                if (!System.IO.File.Exists(filePath))
                {
                    return (null, null, null);
                }

                // Đọc file vào stream
                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

                // Kiểm tra định dạng file và đặt Content-Type tương ứng
                string contentType;
                if (fileName.EndsWith(".rar"))
                {
                    contentType = "application/x-rar-compressed";  // MIME type cho file .rar
                }
                else if (fileName.EndsWith(".zip"))
                {
                    contentType = "application/zip";  // MIME type cho file .zip
                }
                else
                {
                    contentType = "application/octet-stream";  // Định dạng mặc định
                }

                // Trả về thông tin file
                return (fileBytes, contentType, fileName);
            }
            catch (Exception ex)
            {
                return (null, null, null);
            }
        }
        public async Task<(byte[] fileBytes, string contentType, string fileName)> DownloadPaymentAttachmentFileByFileName(string fileName)
        {
            try
            {
                var attachmentWithRequest = await repository.GetAll(x => x.FilePath == fileName)
                    .Include(x => x.RequestForm).FirstOrDefaultAsync();
                if (attachmentWithRequest == null)
                {
                    return (null, null, null);
                }
                // Đường dẫn đầy đủ đến file trong server
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "PaymentUploads", fileName);

                // Kiểm tra xem file có tồn tại không
                if (!System.IO.File.Exists(filePath))
                {
                    return (null, null, null);
                }

                // Đọc file vào stream
                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

                // Kiểm tra định dạng file và đặt Content-Type tương ứng
                string contentType;
                if (fileName.EndsWith(".rar"))
                {
                    contentType = "application/x-rar-compressed";  // MIME type cho file .rar
                }
                else if (fileName.EndsWith(".zip"))
                {
                    contentType = "application/zip";  // MIME type cho file .zip
                }
                else
                {
                    contentType = "application/octet-stream";  // Định dạng mặc định
                }

                // Trả về thông tin file
                return (fileBytes, contentType, fileName);
            }
            catch (Exception ex)
            {
                return (null, null, null);
            }
        }
        public async Task<(byte[] fileBytes, string contentType, string fileName)> DownloadAttachmentFileWithNoTypeByFileName(string fileName)
        {
            try
            {
                var attachmentWithRequest = await repository.GetAll(x => x.FilePath == fileName)
                    .Include(x => x.RequestForm).FirstOrDefaultAsync();
                if (attachmentWithRequest == null)
                {
                    return (null, null, null);
                }
                var folderName = attachmentWithRequest.RequestForm.TypeId == 1 ? "PrePayUploads" : "PaymentUploads";

                // Đường dẫn đầy đủ đến file trong server
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName, fileName);

                // Kiểm tra xem file có tồn tại không
                if (!System.IO.File.Exists(filePath))
                {
                    return (null, null, null);
                }

                // Đọc file vào stream
                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

                // Kiểm tra định dạng file và đặt Content-Type tương ứng
                string contentType;
                if (fileName.EndsWith(".rar"))
                {
                    contentType = "application/x-rar-compressed";  // MIME type cho file .rar
                }
                else if (fileName.EndsWith(".zip"))
                {
                    contentType = "application/zip";  // MIME type cho file .zip
                }
                else
                {
                    contentType = "application/octet-stream";  // Định dạng mặc định
                }

                // Trả về thông tin file
                return (fileBytes, contentType, fileName);
            }
            catch (Exception ex)
            {
                return (null, null, null);
            }
        }

        public async Task<List<AttachmentFile>> GetAllCurrentAttachmentInCampaign(int campaignId)
        {
            var attachmentFiles = new List<AttachmentFile>();
            try
            {
                attachmentFiles = await repository.GetAll(x => x.RequestForm.CampaignId == campaignId)
                    .Include(x => x.RequestForm)
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
                            TypeId = af.RequestForm.TypeId,
                        }
                    }).ToListAsync();
                return attachmentFiles;
            }
            catch (Exception ex)
            {
                return attachmentFiles;
            }
        }

        public async Task<List<RequestForm>> GetAllAttachmentInCampaignByRequest(int campaignId)
        {
            var requestInCampaignsWithAttachment = new List<RequestForm>();
            try
            {
                requestInCampaignsWithAttachment = await requestFormRepository.GetAll(x => x.CampaignId == campaignId)
                    .Include(x => x.AttachmentFiles)
                    .Select(x => new RequestForm
                    {
                        Id = x.Id,
                    })
                    .ToListAsync();
                return requestInCampaignsWithAttachment;
            }
            catch (Exception ex)
            {
                return requestInCampaignsWithAttachment;
            }
        }

        public async Task<List<RequestWithAttachmentFileResult>> GetAllPaymentAttachmentInCampaignWithRequest(int campaignId)
        {
            var requestsWithAttachment = new List<RequestWithAttachmentFileResult>();
            try
            {
                requestsWithAttachment = await requestFormRepository.GetAll(x => x.CampaignId == campaignId
                && x.Status.Equals(Resource.ApprovedStatus)
                && x.TypeId == IntConstant.PaymentRequestType)
                    .Include(x => x.User)
                    .Include(x => x.AttachmentFiles)
                    .Select(x => new RequestWithAttachmentFileResult
                    {
                        Id = x.Id,
                        CreateAt = x.CreateAt,
                        Description = x.Description,
                        ExpectedMoney = x.ExpectedMoney,
                        CreateByName = x.User.FirstName + " " + x.User.LastName,
                        CreateByEmail = x.User.Email,
                        AttachmentFilePath = (x.AttachmentFiles.FirstOrDefault() != null ? x.AttachmentFiles.First().FilePath : null)
                    })
                    .ToListAsync();
                return requestsWithAttachment;
            }
            catch (Exception ex)
            {
                return requestsWithAttachment;
            }
        }
    }
}
