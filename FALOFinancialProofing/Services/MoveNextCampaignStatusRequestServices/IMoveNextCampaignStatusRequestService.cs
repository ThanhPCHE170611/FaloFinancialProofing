using System.Text;
using FALOFinancialProofing.DTOs.CampaignDTO;
using FALOFinancialProofing.DTOs.MoveNextCampaignStatusRequestDTO;
using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Services.MoveNextCampaignStatusRequestServices
{
    public interface IMoveNextCampaignStatusRequestService
    {
        Task<List<MoveNextCampaignStatusRequest>> GetAllMoveNextCampaignStatusRequestAsync();
        Task<MoveNextCampaignStatusRequest?> GetMoveNextCampaignStatusRequestByIdAsync(int id);
        //Task<MoveNextCampaignStatusRequest?> CreateMoveNextCampaignStatusRequestAsync(CreateMoveNextCampaignStatusRequestDTO createMoveNextCampaignStatusRequestDTO);
        //Task<bool> UpdateCampaignAsync(UpdateCampaignDTO updateCampaignDTO);
        //Task<bool> DeleteCampaignByIdAsync(int id);

        Task<MoveNextCampaignStatusRequest> CreateMoveNextCampaignStatusRequestAsync(CreateMoveNextCampaignStatusRequestDTO requestDto);
        Task<CreateMoveNextCampaignStatusRequestDTO?> MapToDto(MoveNextCampaignStatusRequest request);
        Task<bool> ApproveOrRejectRequestAsync(int requestId, bool isApproved);
        Task<bool> ValidateCreateMoveNextCampaignStatusRequestAsync(CreateMoveNextCampaignStatusRequestDTO requestDTO, StringBuilder message);
        Task<bool> UpdateMoveNextCampaignStatusRequestAsync(UpdateMoveNextCampaignStatusRequestDTO updateMoveNextCampaignStatusRequestDTO);
        Task<bool> ValidateProjectCreateAsync(CreateMoveNextCampaignStatusRequestDTO createMoveNextCampaignStatusRequestDTO, StringBuilder message);
    }
}
