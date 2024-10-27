using FALOFinancialProofing.DTOs.CreateProjectRequestApproveHistoryDTO;
using FALOFinancialProofing.Models;
using System.Text;

namespace FALOFinancialProofing.Services.CreateProjectRequestApproveHistoryServices
{
    public interface ICreateProjectRequestApproveHistoryService
    {
        Task<bool> CreateCreateProjectRequestApproveHistoryAsync(CreateProjectRequestApproveHistoryClientRequest createProjectRequestApproveHistoryClientRequest, StringBuilder message);
        Task<CreateProjectRequestApproveHistory> GetCreateProjectRequestApproveHistoryByIdAsync(int id);
        Task<IEnumerable<CreateProjectRequestApproveHistory>> GetAllCreateProjectRequestApproveHistoriesAsync();
        Task<bool> UpdateCreateProjectRequestApproveHistoryAsync(CreateProjectRequestApproveHistory updateCreateProjectRequestApproveHistory);
        Task<bool> DeleteCreateProjectRequestApproveHistoryAsync(int id);
        Task<bool> CheckValidCreateProjectRequestApproveHistory(CreateProjectRequestApproveHistoryClientRequest createProjectRequestApproveHistoryClientRequest, StringBuilder message);
    }
}
