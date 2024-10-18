using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.EntityFrameworkCore;

namespace FALOFinancialProofing.Services.CreateProjectFileServices
{
    public class CreateProjectFileService: ICreateProjectFileService
    {
        private readonly IRepository<CreateProjectFile, int> _createProjectFileRepository;

        public CreateProjectFileService(IRepository<CreateProjectFile, int> createProjectFileRepository)
        {
            _createProjectFileRepository = createProjectFileRepository;
        }

        public async Task<bool> CreateCreateProjectFileAsync(CreateProjectFile createProjectFile)
        {
            try
            {
                if (createProjectFile == null)
                {
                    throw new Exception("CreateProjectFile is null");
                }
                await _createProjectFileRepository.InsertAsync(createProjectFile);

                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"CreateProjectFile: {ex.Message}!");
            }

            return false;
        }

        public async Task<CreateProjectFile> GetCreateProjectFileByIdAsync(int id)
        {
            CreateProjectFile createProjectFile = null!;
            try
            {
                createProjectFile = await _createProjectFileRepository.Get(id);
                if (createProjectFile == null)
                {
                    throw new Exception("CreateProjectFile not found");
                }

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetCreateProjectFileById: {ex.Message}");
            }

            return createProjectFile;
        }

        public async Task<IEnumerable<CreateProjectFile>> GetAllCreateProjectFilesAsync()
        {
            List<CreateProjectFile> data = null!;
            try
            {
                data = await _createProjectFileRepository.GetAll().ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllCreateProjectFiles: {ex.Message}");
            }

            return data;
        }

        public async Task<bool> UpdateCreateProjectFileAsync(CreateProjectFile updateProjectFile)
        {
            try
            {
                if (updateProjectFile == null)
                {
                    throw new Exception("UpdateProjectFile is null");
                }
                await _createProjectFileRepository.UpdateAsync(updateProjectFile);

                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"UpdateCreateProjectFile: {ex.Message}");
            }

            return false;
        }

        public async Task<bool> DeleteCreateProjectFileAsync(int id)
        {
            CreateProjectFile createProjectFile = null!;
            bool result = false;
            try
            {
                createProjectFile = await _createProjectFileRepository.Get(id);
                if (createProjectFile == null)
                {
                    throw new Exception("CreateProjectFile not found!");
                }
                result = await _createProjectFileRepository.DeleteAsync(createProjectFile);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"DeleteCreateProjectFile: {ex.Message}");
            }

            return result;
        }
    }
}
