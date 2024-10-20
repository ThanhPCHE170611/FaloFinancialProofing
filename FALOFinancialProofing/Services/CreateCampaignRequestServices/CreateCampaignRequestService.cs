using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.EntityFrameworkCore;

namespace FALOFinancialProofing.Services.CreateCampaignRequestServices
{
    public class CreateCampaignRequestService : ICreateCampaignRequestService
    {
        private readonly IRepository<CreateCampaignRequest, int> _createCampaignRequestRepository;

        public CreateCampaignRequestService(IRepository<CreateCampaignRequest, int> createCampaignRequestRepository)
        {
            _createCampaignRequestRepository = createCampaignRequestRepository;
        }

        public async Task<bool> CreateCreateCampaignRequestAsync(CreateCampaignRequest createCreateCampaignRequest)
        {
            try
            {
                if (createCreateCampaignRequest == null)
                {
                    throw new Exception("CreateCampaignRequest is null");
                }
                await _createCampaignRequestRepository.InsertAsync(createCreateCampaignRequest);

                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"CreateCampaignRequest: {ex.Message}!");
            }

            return false;
        }

        public async Task<CreateCampaignRequest> GetCreateCampaignRequestByIdAsync(int id)
        {
            CreateCampaignRequest createCampaignRequest = null!;
            try
            {
                createCampaignRequest = await _createCampaignRequestRepository.Get(id);
                if (createCampaignRequest == null)
                {
                    throw new Exception("CreateCampaignRequest not found");
                }

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetCreateCampaignRequestById: {ex.Message}");
            }

            return createCampaignRequest;
        }

        public async Task<IEnumerable<CreateCampaignRequest>> GetAllCreateCampaignRequestsAsync()
        {
            List<CreateCampaignRequest> data = null!;
            try
            {
                data = await _createCampaignRequestRepository.GetAll().ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllCreateCampaignRequests: {ex.Message}");
            }

            return data;
        }


        // admin can update transaction logs
        public async Task<bool> UpdateCreateCampaignRequestAsync(CreateCampaignRequest updateCreateCampaignRequest)
        {
            CreateCampaignRequest createCampaignRequest = null!;
            bool result = false;
            try
            {
                createCampaignRequest = await _createCampaignRequestRepository.Get(updateCreateCampaignRequest.Id);
                if (createCampaignRequest == null)
                {
                    throw new Exception("CreateCampaignRequest not found!");
                }
                //ConvertToBaseEntity(createCampaignRequest, updateCreateCampaignRequest);
                result = await _createCampaignRequestRepository.UpdateAsync(updateCreateCampaignRequest);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"UpdateCreateCampaignRequest: {ex.Message}");
            }

            return result;
        }
        // admin can delete transaction logs
        public async Task<bool> DeleteCreateCampaignRequestAsync(int id)
        {
            CreateCampaignRequest createCampaignRequest = null!;
            bool result = false;
            try
            {
                createCampaignRequest = await _createCampaignRequestRepository.Get(id);
                if (createCampaignRequest == null)
                {
                    throw new Exception("CreateCampaignRequest not found!");
                }
                result = await _createCampaignRequestRepository.DeleteAsync(createCampaignRequest);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"DeleteCreateCampaignRequest: {ex.Message}");
            }

            return result;
        }
    }
}
