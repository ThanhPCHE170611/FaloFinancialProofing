using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Services.AttachmentFIleServices
{
    public interface IAttachmentFileServices
    {
        Task<List<AttachmentFile>> GetAllAttachmentFilesAsync();

        Task<AttachmentFile?> GetAttachmentFileByIdAsync(int id);

        Task<List<AttachmentFile>?> GetAttachmentFileByRequestIdAsync(int requestId);

        Task<bool> CreateManyAttachmentFileAsync(List<AttachmentFileRequest> dtos);

        Task<bool> UpdateRequestFormAsync(AttachmentFileRequest dto);

        Task<bool> DeleteRequestFormAsync(AttachmentFileRequest dto);

        Task<bool> DeleteRequestFormByIdAsync(int id);
    }
}
