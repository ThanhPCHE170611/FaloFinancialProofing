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

        public SDGServices(IRepository<SDG, int> _sdgRepository)
        {
            this.sdgRepository = _sdgRepository;
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
            try
            {
                return new SDG
                {
                    Id = sdg.Id != null ? sdg.Id.Value : 0,
                    SDGName = sdg.SDGName,
                    UserId = sdg.UserId,
                };
            } catch (Exception ex)
            {
                return null;
            }
            
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
                var deletedSDG = await sdgRepository.Get(x => x.Id == sdg.Id);
                if (sdg == null) return false;

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
                return await sdgRepository.GetAll().Include(x => x.User).Select(
                    sdg => new SDG
                    {
                        Id = sdg.Id,
                        SDGName = sdg.SDGName,
                        UserId = sdg.UserId,
                        User = new User
                        {
                            Id = sdg.User.Id,
                            FirstName = sdg.User.FirstName,
                            LastName = sdg.User.LastName,
                        }
                    }
                    ).ToListAsync();
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
                return await sdgRepository.GetAll(x => x.Id == id).Include(x => x.User).Select(
                    sdg => new SDG
                    {
                        Id = sdg.Id,
                        SDGName = sdg.SDGName,
                        UserId = sdg.UserId,
                        User = new User
                        {
                            Id = sdg.User.Id,
                            FirstName = sdg.User.FirstName,
                            LastName = sdg.User.LastName,
                        }
                    }
                    ).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<SDG>?> GetSDGByUserIdAsync(string userId)
        {
            try
            {
                return await sdgRepository.GetAll(x => x.UserId.Equals(userId)).Include(x => x.User).Select(
                   sdg => new SDG
                   {
                       Id = sdg.Id,
                       SDGName = sdg.SDGName,
                       UserId = sdg.UserId,
                       User = new User
                       {
                           Id = sdg.User.Id,
                           FirstName = sdg.User.FirstName,
                           LastName = sdg.User.LastName,
                       }
                   }
                   ).ToListAsync();
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
