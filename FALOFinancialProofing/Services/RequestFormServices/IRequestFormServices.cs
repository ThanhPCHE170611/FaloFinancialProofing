using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Services.RequestFormServices
{
    public interface IRequestFormServices
    {
        Task<List<RequestForm>> GetAllRequestFormsAsync();

        Task<RequestForm?> GetRequestFormByIdAsync(int id);

        Task<List<RequestForm>?> GetRequestFormByUserIdAsync(string userId);

        Task<RequestForm?> CreateRequestFormAsync(RequestFormDTO dto);

        Task<bool> UpdateRequestFormAsync(RequestFormDTO dto);

        Task<bool> DeleteRequestFormAsync(RequestFormDTO dto);

        Task<bool> DeleteRequestFormByIdAsync(int id);

        Task<RequestForm?> CreateRequestFormAsync(RequestFormInformation requestForm);

        Task<CreateFormRequest> ValidateRequestForm(CreateFormRequest requestFormRequest, System.Text.StringBuilder message);

        Task<List<AttachmentFileRequest>> SaveUploadedFilesAsync(List<IFormFile> uploadFiles, int value);
        Task<List<UserWithRole>> GetApproverListForVolunteer(int campaignId);
        Task<UserWithRole?> GetApproverForVolunteerLeader(int campaignId);
        Task<UserWithRole?> GetApproverForAccounting(int campaignId);
        Task<UserWithRole?> GetApproverForProjectManagement(int campaignId);
        Task<List<VoucherRequest>> SaveUploadedVoucherAsync(int approveId, List<IFormFile> files);
    }
}
