using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.EntityFrameworkCore;

namespace FALOFinancialProofing.Services.VoucherServices
{
    public class VoucherServices : IVoucherServices
    {
        private readonly IRepository<Voucher, int> repository;

        public VoucherServices(IRepository<Voucher, int> repository)
        {
            this.repository = repository;
        }

        public async Task<bool> CreateManyVoucherAsync(List<VoucherRequest> dtos)
        {
            try
            {
                var newVouchers = await ListDTOToListEntity(dtos);
                return await repository.InsertManyAsync(newVouchers);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #region Mapping Data
        private async Task<List<Voucher>> ListDTOToListEntity(List<VoucherRequest> dtos)
        {
            var vouchers = new List<Voucher>();
            foreach (var dto in dtos)
            {
                vouchers.Add(await DTOToEntity(dto));
            }
            return vouchers;
        }

        private async Task<Voucher> DTOToEntity(VoucherRequest dto)
        {
            return new Voucher
            {
                Id = dto.Id != null ? dto.Id.Value : 0,
                FilePath = dto.FilePath,
                Status = Resource.ProcessStatus,
                ApproveId = dto.ApproveId
            };
        }
        #endregion

        public async Task<bool> DeleteVoucherAsync(VoucherRequest dto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteVoucherByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Voucher>?> GetVouchersByApproveIdAsync(int approveId)
        {
            try
            {
                return await repository.GetAll(x => x.ApproveId == approveId)
                    .Include(x => x.ApproveProcess)
                    .Select(v => new Voucher
                    {
                        Id = v.Id,
                        FilePath = v.FilePath,
                        Status = v.Status,
                        ApproveId = v.ApproveProcess.Id,
                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> UpdateVoucherAsync(VoucherRequest dto)
        {
            throw new NotImplementedException();
        }

        public async Task<(byte[] fileBytes, string contentType, string downloadFileName)> DownloadPrePayAttachmentFileByFileName(string fileName)
        {
            try
            {
                // Đường dẫn đầy đủ đến file trong server
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Pre-PayVouchers", fileName);

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
        public async Task<(byte[] fileBytes, string contentType, string downloadFileName)> DownloadPaymentAttachmentFileByFileName(string fileName)
        {
            try
            {
                // Đường dẫn đầy đủ đến file trong server
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "PaymentVouchers", fileName);

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
    }
}
