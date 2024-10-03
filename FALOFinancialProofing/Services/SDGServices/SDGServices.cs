using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FALOFinancialProofing.Services.SDGServices
{
    public class SDGServices : ISDGServices
    {
        private readonly IRepository<SDG, int> sdgRepository;
        private readonly UserManager<User> userManager;

        public SDGServices(IRepository<SDG, int> _sdgRepository,
                                 UserManager<User> _userManager)
        {
            this.sdgRepository = _sdgRepository;
            this.userManager = _userManager;
        }

        public async Task<SDG?> CreateSDGAsync(SDGRequest sdg)
        {
            try
            {
                var newSDG = await SDGDTOToEntity(sdg);
                return await sdgRepository.InsertAsync(newSDG);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private async Task<SDG> SDGDTOToEntity(SDGRequest sdg)
        {
            return new SDG
            {
                Id = sdg.Id != null ? sdg.Id.Value : 0,
                SDGName = sdg.SDGName,
                UserId = sdg.UserId,
                User = await userManager.FindByIdAsync(sdg.UserId.ToString()),
            };
        }

        public async Task<bool> DeleteSDGByIdAsync(int id)
        {
            try
            {
                var sdg = await sdgRepository.Get(x => x.Id == id);
                if (sdg == null) return false;

                return await sdgRepository.DeleteAsync(sdg);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> DeleteSDGAsync(SDGRequest sdg)
        {
            try
            {
                var deletedSDG = await SDGDTOToEntity(sdg);
                return await sdgRepository.DeleteAsync(deletedSDG);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<List<SDG>> GetAllSDGsAsync()
        {
            try
            {
                return await sdgRepository.GetAll().ToListAsync();
            }
            catch (Exception e)
            {
                return new List<SDG>();
            }
        }

        public async Task<SDG?> GetSDGByIdAsync(int id)
        {
            try
            {
                return await sdgRepository.Get(x => x.Id == id);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<SDG?> GetSDGByUserIdAsync(string userId)
        {
            try
            {
                return await sdgRepository.Get(x => x.User.Id == userId);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> UpdateSDGAsync(SDGRequest sdg)
        {
            try
            {
                var updatedSDG = await SDGDTOToEntity(sdg);
                return await sdgRepository.UpdateAsync(updatedSDG);
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
