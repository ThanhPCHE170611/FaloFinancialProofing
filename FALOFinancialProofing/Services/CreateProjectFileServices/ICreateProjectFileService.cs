using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Services.CreateProjectFileServices
{
    public interface ICreateProjectFileService
    {
        Task<bool> CreateCreateProjectFileAsync(CreateProjectFile createProjectFile);
        Task<bool> CreateCreateProjectFilesAsync(List<CreateProjectFile> createProjectFiles);
        Task<CreateProjectFile> GetCreateProjectFileByIdAsync(int id);
        Task<IEnumerable<CreateProjectFile>> GetAllCreateProjectFilesAsync();
        Task<bool> UpdateCreateProjectFileAsync(CreateProjectFile updateProjectFile);
        Task<bool> DeleteCreateProjectFileAsync(int id);
        Task<List<CreateProjectFile>> SaveUploadedFilesAsync(List<IFormFile> uploadFiles, int requestId);
    }
}
