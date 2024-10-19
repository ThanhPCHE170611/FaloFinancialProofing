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
                Status = dto.Status,
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
    }
}
