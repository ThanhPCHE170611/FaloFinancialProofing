using System.Text;
using FALOFinancialProofing.DTOs.CampaignDTO;
using FALOFinancialProofing.DTOs.CreateCampaignRequestDTO;
using FALOFinancialProofing.DTOs.MoveNextCampaignStatusRequestDTO;
using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Services.MoveNextCampaignStatusRequestServices
{
    public interface IMoveNextCampaignStatusRequestService
    {
        Task<List<MoveNextCampaignStatusRequest>> GetAllMoveNextCampaignStatusRequestAsync();
        Task<MoveNextCampaignStatusRequest?> GetMoveNextCampaignStatusRequestByIdAsync1(int id);
        Task<MoveNextCampaignStatusRequest> CreateMoveNextCampaignStatusRequestAsync(CreateMoveNextCampaignStatusRequestDTO requestDto, StringBuilder message);
        Task<CreateMoveNextCampaignStatusRequestDTO?> MapToDto(MoveNextCampaignStatusRequest request);
        Task<bool> ApproveOrRejectRequestAsync(int requestId, bool isApproved);
        Task<bool> ValidateCreateMoveNextCampaignStatusRequestAsync(CreateMoveNextCampaignStatusRequestDTO requestDTO, StringBuilder message);
        //Task<bool> UpdateMoveNextCampaignStatusRequestAsync(UpdateMoveNextCampaignStatusRequestDTO updateMoveNextCampaignStatusRequestDTO);
        Task<bool> ValidateCampaignCreateAsync(CreateMoveNextCampaignStatusRequestDTO createMoveNextCampaignStatusRequestDTO, StringBuilder message);

        Task<IEnumerable<MoveNextCampaignStatusRequestInformation>> GetAllMoveNextCampaignStatusRequestsByPMBAsync(StringBuilder message);
        Task<IEnumerable<MoveNextCampaignStatusRequestInformation>> GetAllMoveNextCampaignStatusRequestsByUserIdAsync(string userId, StringBuilder message);
        Task<bool> UpdateMoveNextCampaignStatusRequestAsync(MoveNextCampaignStatusRequest updateMoveNextCampaignStatusRequest);
        Task<bool> CancelMoveNextCampaignStatusRequestAsync(int requestId, string senderId, StringBuilder message);
        Task<MoveNextCampaignStatusRequestInformation> GetMoveNextCampaignStatusRequestByIdAsync(int id);

    }
}
