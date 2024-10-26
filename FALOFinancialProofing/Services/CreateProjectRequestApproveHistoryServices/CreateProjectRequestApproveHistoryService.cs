using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.EntityFrameworkCore;

namespace FALOFinancialProofing.Services.CreateProjectRequestApproveHistoryServices
{
    public class CreateProjectRequestApproveHistoryService : ICreateProjectRequestApproveHistoryService
    {
        private readonly IRepository<CreateProjectRequestApproveHistory, int> _createProjectRequestApproveHistoriesRepository;

        public CreateProjectRequestApproveHistoryService(IRepository<CreateProjectRequestApproveHistory, int> createProjectRequestApproveHistoriesRepository)
        {
            _createProjectRequestApproveHistoriesRepository = createProjectRequestApproveHistoriesRepository;
        }

        public async Task<bool> CreateCreateProjectRequestApproveHistoryAsync(CreateProjectRequestApproveHistory createCreateProjectRequestApproveHistory)
        {
            try
            {
                if (createCreateProjectRequestApproveHistory == null)
                {
                    throw new Exception("CreateProjectRequestApproveHistory is null");
                }
                //var organization = ConvertToBaseEntity(createCreateProjectRequestApproveHistory);
                await _createProjectRequestApproveHistoriesRepository.InsertAsync(createCreateProjectRequestApproveHistory);

                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"CreateProjectRequestApproveHistory: {ex.Message}!");
            }

            return false;
        }

        public async Task<CreateProjectRequestApproveHistory> GetCreateProjectRequestApproveHistoryByIdAsync(int id)
        {
            CreateProjectRequestApproveHistory organization = null!;
            try
            {
                organization = await _createProjectRequestApproveHistoriesRepository.Get(id);
                if (organization == null)
                {
                    throw new Exception("CreateProjectRequestApproveHistory not found");
                }

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetCreateProjectRequestApproveHistoryById: {ex.Message}");
            }

            return organization;
        }
        

        public async Task<IEnumerable<CreateProjectRequestApproveHistory>> GetAllCreateProjectRequestApproveHistoriesAsync()
        {
            List<CreateProjectRequestApproveHistory> data = null!;
            try
            {
                data = await _createProjectRequestApproveHistoriesRepository.GetAll().ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllCreateProjectRequestApproveHistorys: {ex.Message}");
            }

            return data;
        }


        public async Task<bool> UpdateCreateProjectRequestApproveHistoryAsync(CreateProjectRequestApproveHistory updateCreateProjectRequestApproveHistory)
        {
            CreateProjectRequestApproveHistory organization = null!;
            bool result = false;
            try
            {
                organization = await _createProjectRequestApproveHistoriesRepository.Get(updateCreateProjectRequestApproveHistory.Id);
                if (organization == null)
                {
                    throw new Exception("CreateProjectRequestApproveHistory not found!");
                }
                //ConvertToBaseEntity(organization, updateCreateProjectRequestApproveHistory);
                result = await _createProjectRequestApproveHistoriesRepository.UpdateAsync(updateCreateProjectRequestApproveHistory);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"UpdateCreateProjectRequestApproveHistory: {ex.Message}");
            }

            return result;
        }
        // admin can delete transaction logs
        public async Task<bool> DeleteCreateProjectRequestApproveHistoryAsync(int id)
        {
            CreateProjectRequestApproveHistory organization = null!;
            bool result = false;
            try
            {
                organization = await _createProjectRequestApproveHistoriesRepository.Get(id);
                if (organization == null)
                {
                    throw new Exception("CreateProjectRequestApproveHistory not found!");
                }
                result = await _createProjectRequestApproveHistoriesRepository.DeleteAsync(organization);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"DeleteCreateProjectRequestApproveHistory: {ex.Message}");
            }

            return result;
        }
    }
}
