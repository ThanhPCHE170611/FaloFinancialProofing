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

        public async Task<List<CreateCampaignFile>> SaveUploadedFilesAsync(List<IFormFile> uploadFiles, int requestId)
        {
            var attachmentFiles = new List<CreateCampaignFile>();
            try
            {
                // Kiểm tra nếu danh sách file không null và có file
                if (uploadFiles != null && uploadFiles.Any())
                {
                    // Tạo đường dẫn thư mục lưu trữ
                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "CampaignFileUploads");

                    // Kiểm tra và tạo thư mục nếu chưa tồn tại
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    foreach (var file in uploadFiles)
                    {
                        // Tạo tên file mới để tránh trùng lặp bằng cách thêm GUID vào tên file
                        var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";

                        // Tạo đường dẫn đầy đủ tới file sẽ lưu
                        var filePath = Path.Combine(uploadFolder, uniqueFileName);

                        // Sử dụng FileStream để lưu file vào đường dẫn
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Tạo DTO lưu thông tin file đã upload
                        var attachmentFile = new CreateCampaignFile
                        {
                            FilePath = uniqueFileName,  // Lưu tên file hoặc có thể lưu cả đường dẫn nếu cần
                            RequestId = requestId
                        };
                        attachmentFiles.Add(attachmentFile);
                    }
                }

            }
            catch (Exception ex)
            {
                // Log lỗi hoặc xử lý ngoại lệ tùy theo yêu cầu

            }
            return attachmentFiles;
        }

        public async Task<bool> CreateCreateCampaignFilesAsync(List<CreateCampaignFile> createCampaignFiles)
        {
            var checkValid = false;
            try
            {
                bool createdCreateCampaignFile = await _createCampaignFileRepository.InsertManyAsync(createCampaignFiles);
                if (!createdCreateCampaignFile)
                {
                    throw new Exception("CreateCampaignFiles Error");
                }
                checkValid = true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"CreateCreateCampaignFilesAsync: {ex.Message}");
            }
            return checkValid;
        }
    }
}
