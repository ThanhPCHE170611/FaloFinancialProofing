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
        Task<(byte[] fileBytes, string contentType, string fileName)> DownloadPrePayAttachmentFileByFileName(string fileName);
        Task<List<AttachmentFile>> GetAllCurrentAttachmentInCampaign(int campaignId);
        Task<(byte[] fileBytes, string contentType, string fileName)> DownloadPaymentAttachmentFileByFileName(string fileName);
        Task<List<RequestForm>> GetAllAttachmentInCampaignByRequest(int campaignId);
        Task<(byte[] fileBytes, string contentType, string fileName)> DownloadAttachmentFileWithNoTypeByFileName(string fileName);
        Task<List<RequestWithAttachmentFileResult>> GetAllPaymentAttachmentInCampaignWithRequest(int campaignId);
    }
}
