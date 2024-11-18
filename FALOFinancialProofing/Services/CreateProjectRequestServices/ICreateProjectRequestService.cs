using FALOFinancialProofing.DTOs.CreateProjectRequestDTO;
using FALOFinancialProofing.Models;
using System.Text;

namespace FALOFinancialProofing.Services.CreateProjectRequestServices
{
    public interface ICreateProjectRequestService
    {
        Task<bool> CreateCreateProjectRequestAsync(CreateProjectRequest createProjectRequest);
        Task<CreateProjectRequestInformation> GetCreateProjectRequestByIdAsync(int id);
        Task<IEnumerable<CreateProjectRequest>> GetAllCreateProjectRequestsAsync();
        Task<bool> UpdateCreateProjectRequestAsync(CreateProjectRequest updateProjectRequest);
        Task<bool> DeleteCreateProjectRequestAsync(int id);
        Task<CreateProjectRequest> CreateCreateProjectRequestReturnEntityAsync(CreateProjectRequest createCreateProjectRequest);
        Task<IEnumerable<CreateProjectRequestInformation>> GetAllCreateProjectRequestsByPMBAsync(StringBuilder message);
        Task<IEnumerable<CreateProjectRequestInformation>> GetAllCreateProjectRequestsByUserIdAsync(string userId, StringBuilder message);
        Task<bool> ValidateCreateProjectRequestByUserIdAndRequestIdAsync(string userId, int CreateProjectRequestId, StringBuilder message);
        Task<bool> CancelCreateProjectRequestAsync(string userId, int CreateProjectRequestId);
    }
}
