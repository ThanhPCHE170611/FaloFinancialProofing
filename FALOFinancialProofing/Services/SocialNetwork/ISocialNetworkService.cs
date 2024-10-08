using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Services.SocialNetworkService
{
    public interface ISocialNetworkService
    {
        Task<List<SocialNetwork>> GetAllSocialNetworksAsync();

        Task<SocialNetwork?> GetSocialNetworkByIdAsync(int id);

        Task<SocialNetwork?> GetSocialNetworkByUserIdAsync(string userId);

        Task<SocialNetwork?> CreateSocialNetworkAsync(SocialNetworkRequest socialNetworkRequest);
        //Task<SocialNetworkDto?> CreateSocialNetworkAsync(SocialNetworkRequest socialNetworkRequest);

        Task<bool> UpdateSocialNetworkAsync(SocialNetworkRequest socialNetworkRequest);

        Task<bool> DeleteSocialNetworkAsync(SocialNetworkRequest socialNetworkRequest);

        Task<bool> DeleteSocialNetworkByIdAsync(int id);


    }
}
