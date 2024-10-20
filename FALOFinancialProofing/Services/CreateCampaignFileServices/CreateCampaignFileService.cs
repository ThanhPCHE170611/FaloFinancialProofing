using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.EntityFrameworkCore;

namespace FALOFinancialProofing.Services.CreateCampaignFileServices
{
    public class CreateCampaignFileService : ICreateCampaignFileService
    {
        private readonly IRepository<CreateCampaignFile, int> _createCampaignFileRepository;

        public CreateCampaignFileService(IRepository<CreateCampaignFile, int> createCampaignFileRepository)
        {
            _createCampaignFileRepository = createCampaignFileRepository;
        }

        public async Task<bool> CreateCreateCampaignFileAsync(CreateCampaignFile createCampaignFile)
        {
            try
            {
                if (createCampaignFile == null)
                {
                    throw new Exception("CreateCampaignFile is null");
                }
                await _createCampaignFileRepository.InsertAsync(createCampaignFile);

                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"CreateCampaignFile: {ex.Message}!");
            }

            return false;
        }

        public async Task<CreateCampaignFile> GetCreateCampaignFileByIdAsync(int id)
        {
            CreateCampaignFile createCampaignFile = null!;
            try
            {
                createCampaignFile = await _createCampaignFileRepository.Get(id);
                if (createCampaignFile == null)
                {
                    throw new Exception("CreateCampaignFile not found");
                }

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetCreateCampaignFileById: {ex.Message}");
            }

            return createCampaignFile;
        }

        public async Task<IEnumerable<CreateCampaignFile>> GetAllCreateCampaignFilesAsync()
        {
            List<CreateCampaignFile> data = null!;
            try
            {
                data = await _createCampaignFileRepository.GetAll().ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllCreateCampaignFiles: {ex.Message}");
            }

            return data;
        }

        public async Task<bool> UpdateCreateCampaignFileAsync(CreateCampaignFile updateCampaignFile)
        {
            try
            {
                if (updateCampaignFile == null)
                {
                    throw new Exception("UpdateCampaignFile is null");
                }
                await _createCampaignFileRepository.UpdateAsync(updateCampaignFile);

                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"UpdateCreateCampaignFile: {ex.Message}");
            }

            return false;
        }

        public async Task<bool> DeleteCreateCampaignFileAsync(int id)
        {
            CreateCampaignFile createCampaignFile = null!;
            bool result = false;
            try
            {
                createCampaignFile = await _createCampaignFileRepository.Get(id);
                if (createCampaignFile == null)
                {
                    throw new Exception("CreateCampaignFile not found!");
                }
                result = await _createCampaignFileRepository.DeleteAsync(createCampaignFile);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"DeleteCreateCampaignFile: {ex.Message}");
            }

            return result;
        }
    }
}
