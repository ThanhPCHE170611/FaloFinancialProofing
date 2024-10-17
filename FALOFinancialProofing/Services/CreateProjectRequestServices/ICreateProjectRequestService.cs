using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Services.CreateProjectRequestServices
{
    public interface ICreateProjectRequestService
    {
        Task<bool> CreateCreateProjectRequestAsync(CreateProjectRequest createProjectRequest);
        Task<CreateProjectRequest> GetCreateProjectRequestByIdAsync(int id);
        Task<IEnumerable<CreateProjectRequest>> GetAllCreateProjectRequestsAsync();
        Task<bool> UpdateCreateProjectRequestAsync(CreateProjectRequest updateProjectRequest);
        Task<bool> DeleteCreateProjectRequestAsync(int id);
    }
}
