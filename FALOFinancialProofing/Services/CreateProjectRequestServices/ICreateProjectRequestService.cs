using FALOFinancialProofing.Models;
using System.Text;

namespace FALOFinancialProofing.Services.CreateProjectRequestServices
{
    public interface ICreateProjectRequestService
    {
        Task<bool> CreateCreateProjectRequestAsync(CreateProjectRequest createProjectRequest);
        Task<CreateProjectRequest> GetCreateProjectRequestByIdAsync(int id);
        Task<IEnumerable<CreateProjectRequest>> GetAllCreateProjectRequestsAsync();
        Task<bool> UpdateCreateProjectRequestAsync(CreateProjectRequest updateProjectRequest);
        Task<bool> DeleteCreateProjectRequestAsync(int id);
        Task<CreateProjectRequest> CreateCreateProjectRequestReturnEntityAsync(CreateProjectRequest createCreateProjectRequest);
        Task<IEnumerable<CreateProjectRequest>> GetAllCreateProjectRequestsByPMBAsync(StringBuilder message);
    }
}
