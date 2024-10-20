using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.EntityFrameworkCore;

namespace FALOFinancialProofing.Services.MoveNextCampaignStatusRequestServices
{
    public class MoveNextCampaignStatusRequestService : IMoveNextCampaignStatusRequestService
    {
        private readonly IRepository<MoveNextCampaignStatusRequest, int> _moveNextCampaignStatusRequestRepository;

        public MoveNextCampaignStatusRequestService(IRepository<MoveNextCampaignStatusRequest, int> moveNextCampaignStatusRequestRepository)
        {
            _moveNextCampaignStatusRequestRepository = moveNextCampaignStatusRequestRepository;
        }
        public async Task<bool> CreateMoveNextCampaignStatusRequestAsync(MoveNextCampaignStatusRequest createMoveNextCampaignStatusRequest)
        {
            try
            {
                if (createMoveNextCampaignStatusRequest == null)
                {
                    throw new Exception("MoveNextCampaignStatusRequest is null");
                }
                //var organization = ConvertToBaseEntity(createMoveNextCampaignStatusRequest);
                await _moveNextCampaignStatusRequestRepository.InsertAsync(createMoveNextCampaignStatusRequest);

                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"MoveNextCampaignStatusRequest: {ex.Message}!");
            }

            return false;
        }

        public async Task<MoveNextCampaignStatusRequest> GetMoveNextCampaignStatusRequestByIdAsync(int id)
        {
            MoveNextCampaignStatusRequest organization = null!;
            try
            {
                organization = await _moveNextCampaignStatusRequestRepository.Get(id);
                if (organization == null)
                {
                    throw new Exception("MoveNextCampaignStatusRequest not found");
                }

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetMoveNextCampaignStatusRequestById: {ex.Message}");
            }

            return organization;
        }

        public async Task<IEnumerable<MoveNextCampaignStatusRequest>> GetAllMoveNextCampaignStatusRequestsAsync()
        {
            List<MoveNextCampaignStatusRequest> data = null!;
            try
            {
                data = await _moveNextCampaignStatusRequestRepository.GetAll().ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllMoveNextCampaignStatusRequests: {ex.Message}");
            }

            return data;
        }


        // admin can update transaction logs
        public async Task<bool> UpdateMoveNextCampaignStatusRequestAsync(MoveNextCampaignStatusRequest updateMoveNextCampaignStatusRequest)
        {
            MoveNextCampaignStatusRequest organization = null!;
            bool result = false;
            try
            {
                organization = await _moveNextCampaignStatusRequestRepository.Get(updateMoveNextCampaignStatusRequest.Id);
                if (organization == null)
                {
                    throw new Exception("MoveNextCampaignStatusRequest not found!");
                }
                //ConvertToBaseEntity(organization, updateMoveNextCampaignStatusRequest);
                result = await _moveNextCampaignStatusRequestRepository.UpdateAsync(updateMoveNextCampaignStatusRequest);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"UpdateMoveNextCampaignStatusRequest: {ex.Message}");
            }

            return result;
        }
        // admin can delete transaction logs
        public async Task<bool> DeleteMoveNextCampaignStatusRequestAsync(int id)
        {
            MoveNextCampaignStatusRequest organization = null!;
            bool result = false;
            try
            {
                organization = await _moveNextCampaignStatusRequestRepository.Get(id);
                if (organization == null)
                {
                    throw new Exception("MoveNextCampaignStatusRequest not found!");
                }
                result = await _moveNextCampaignStatusRequestRepository.DeleteAsync(organization);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"DeleteMoveNextCampaignStatusRequest: {ex.Message}");
            }

            return result;
        }
    }
}
