using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Services.CreateProjectRequestApproveHistoryServices
{
    public interface ICreateProjectRequestApproveHistoryService
    {
        Task<bool> CreateCreateProjectRequestApproveHistoryAsync(CreateProjectRequestApproveHistory createCreateProjectRequestApproveHistory);
        Task<CreateProjectRequestApproveHistory> GetCreateProjectRequestApproveHistoryByIdAsync(int id);
        Task<IEnumerable<CreateProjectRequestApproveHistory>> GetAllCreateProjectRequestApproveHistoriesAsync();
        Task<bool> UpdateCreateProjectRequestApproveHistoryAsync(CreateProjectRequestApproveHistory updateCreateProjectRequestApproveHistory);
        Task<bool> DeleteCreateProjectRequestApproveHistoryAsync(int id);
    }
}
