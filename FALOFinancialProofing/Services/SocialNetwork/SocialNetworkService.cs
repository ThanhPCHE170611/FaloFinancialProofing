using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FALOFinancialProofing.Services.SocialNetworkService
{
    public class SocialNetworkService : ISocialNetworkService
    {
        private readonly IRepository<SocialNetwork, int> socialNetworksRepository;
        private readonly UserManager<User> userManager;

        public SocialNetworkService(IRepository<SocialNetwork, int> _socialNetworksRepository,
                                 UserManager<User> _userManager)
        {
            this.socialNetworksRepository = _socialNetworksRepository;
            this.userManager = _userManager;
        }

        public async Task<SocialNetwork?> CreateSocialNetworkAsync(SocialNetworkRequest socialNetworkRequest)
        {
            try
            {
                var newsocialNetwork = await SocialNetworkDTOToEntity(socialNetworkRequest);
                return await socialNetworksRepository.InsertAsync(newsocialNetwork);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        //public async Task<SocialNetworkDto?> CreateSocialNetworkAsync(SocialNetworkRequest socialNetworkRequest)
        //{
        //    var createdSocialNetwork = await socialNetworksRepository.InsertAsync(new SocialNetwork
        //    {
        //        SocialNetworksLink = socialNetworkRequest.SocialNetworksLink,
        //        UserId = socialNetworkRequest.UserId
        //    });

        //    if (createdSocialNetwork == null) return null;

        //    // Map to DTO
        //    return new SocialNetworkDto
        //    {
        //        SocialNetworksLink = createdSocialNetwork.SocialNetworksLink,
        //    };
        //}

        private async Task<SocialNetwork> SocialNetworkDTOToEntity(SocialNetworkRequest socialNetworkRequest)
        {
            return new SocialNetwork
            {
                Id = socialNetworkRequest.Id != null ? socialNetworkRequest.Id.Value : 0,
                SocialNetworksLink = socialNetworkRequest.SocialNetworksLink,
                UserId = socialNetworkRequest.UserId,
                //User = await userManager.FindByIdAsync(socialNetworkRequest.UserId.ToString()),
            };
        }

        public async Task<bool> DeleteSocialNetworkByIdAsync(int id)
        {
            try
            {
                var socialNetwork = await socialNetworksRepository.Get(x => x.Id == id);
                if (socialNetwork == null) return false;

                return await socialNetworksRepository.DeleteAsync(socialNetwork);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> DeleteSocialNetworkAsync(SocialNetworkRequest socialNetworkRequest)
        {
            try
            {
                var deletedSocialNetwork = await SocialNetworkDTOToEntity(socialNetworkRequest);
                return await socialNetworksRepository.DeleteAsync(deletedSocialNetwork);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<List<SocialNetwork>> GetAllSocialNetworksAsync()
        {
            try
            {
                return await socialNetworksRepository.GetAll().ToListAsync();
            }
            catch (Exception e)
            {
                return new List<SocialNetwork>();
            }
        }

        public async Task<SocialNetwork?> GetSocialNetworkByIdAsync(int id)
        {
            try
            {
                return await socialNetworksRepository.Get(x => x.Id == id);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<SocialNetwork?> GetSocialNetworkByUserIdAsync(string userId)
        {
            try
            {
                return await socialNetworksRepository.Get(x => x.User.Id == userId);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> UpdateSocialNetworkAsync(SocialNetworkRequest socialNetworkRequest)
        {
            try
            {
                var updatedSocialNetwork = await SocialNetworkDTOToEntity(socialNetworkRequest);
                return await socialNetworksRepository.UpdateAsync(updatedSocialNetwork);
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
