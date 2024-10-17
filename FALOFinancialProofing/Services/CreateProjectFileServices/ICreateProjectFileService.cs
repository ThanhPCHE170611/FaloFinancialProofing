using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Services.CreateProjectFileServices
{
    public interface ICreateProjectFileService
    {
        Task<bool> CreateCreateProjectFileAsync(CreateProjectFile createProjectFile);
        Task<CreateProjectFile> GetCreateProjectFileByIdAsync(int id);
        Task<IEnumerable<CreateProjectFile>> GetAllCreateProjectFilesAsync();
        Task<bool> UpdateCreateProjectFileAsync(CreateProjectFile updateProjectFile);
        Task<bool> DeleteCreateProjectFileAsync(int id);
    }
}
