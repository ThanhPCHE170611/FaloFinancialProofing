using System.Text;
using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.DTOs.MoveNextCampaignStatusRequestDTO;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FALOFinancialProofing.Services.MoveNextCampaignStatusRequestServices
{
    public class MoveNextCampaignStatusRequestService : IMoveNextCampaignStatusRequestService
    {
        private readonly IRepository<Campaign, int> _campaignRepository;
        private readonly AuthServices _authServices;


        private readonly IRepository<MoveNextCampaignStatusRequest, int> _moveNextCampaignStatusRequestRepository;

        public MoveNextCampaignStatusRequestService(IRepository<MoveNextCampaignStatusRequest, int> moveNextCampaignStatusRequestRepository,
            IRepository<Campaign, int> campaignRepository,
            AuthServices authServices)
        {
            _moveNextCampaignStatusRequestRepository = moveNextCampaignStatusRequestRepository;
            _campaignRepository = campaignRepository;
            _authServices = authServices;
        }

        // Manh moi them vao
        public async Task<List<MoveNextCampaignStatusRequest>> GetAllMoveNextCampaignStatusRequestAsync()
        {
            try
            {
                return await _moveNextCampaignStatusRequestRepository.GetAll().ToListAsync();
            }
            catch (Exception e)
            {
                return new List<MoveNextCampaignStatusRequest>();
            }
        }

        public async Task<MoveNextCampaignStatusRequest?> GetMoveNextCampaignStatusRequestByIdAsync(int id)
        {
            try
            {
                return await _moveNextCampaignStatusRequestRepository.Get(x => x.Id == id);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<MoveNextCampaignStatusRequest> CreateMoveNextCampaignStatusRequestAsync(CreateMoveNextCampaignStatusRequestDTO requestDto)
        {
            try
            {
                if (requestDto == null)
                {
                    throw new ArgumentNullException(nameof(requestDto), "DTO not null.");
                }
                Campaign campaign = await GetCampaignByIdAsync(requestDto.CampaignID);

                if (campaign == null)
                {
                    throw new InvalidOperationException("Not Found Campaign");
                }
                string nextStatus;
                if (campaign.Status == CampaignStatus.FundRaising)
                {
                    nextStatus = CampaignStatus.Implement;
                }
                else if (campaign.Status == CampaignStatus.Implement)
                {
                    nextStatus = CampaignStatus.Disbursement;
                }
                else if (campaign.Status == CampaignStatus.Disbursement)
                {
                    nextStatus = CampaignStatus.Close;
                }
                else
                {
                    throw new InvalidOperationException("Campaign closed, can not move next");
                }

                var request = new MoveNextCampaignStatusRequest
                {
                    CampaignID = requestDto.CampaignID,
                    StatusOfCampaign = nextStatus,
                    Status = requestDto.Status,
                    Title = requestDto.Title,
                    SenderId = requestDto.SenderId,
                    CreatedAt = requestDto.CreatedAt
                };
                await _moveNextCampaignStatusRequestRepository.InsertAsync(request);
                return request;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CreateMoveNextCampaignStatusRequestAsync: {ex.Message}");
            }
            return null;    
        }

        private async Task<Campaign?> GetCampaignByIdAsync(int id)
        {
            try
            {
                return await _campaignRepository.Get(x => x.Id == id);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> ApproveOrRejectRequestAsync(int requestId, bool isApproved)
        {
            var request = await GetMoveNextCampaignStatusRequestByIdAsync(requestId);
            if (request == null || request.Status != "Pending")
                throw new InvalidOperationException("Request not found or already processed.");

            if (isApproved)
            {
                request.Status = "Approve";

                // Cập nhật trạng thái Campaign
                var campaign = await GetCampaignByIdAsync(request.CampaignID);
                if (campaign == null) throw new InvalidOperationException("Campaign not found.");

                campaign.Status = request.StatusOfCampaign;
            }
            else
            {
                request.Status = "Reject";
            }

            return isApproved;
        }

        public async Task<bool> ValidateCreateMoveNextCampaignStatusRequestAsync(CreateMoveNextCampaignStatusRequestDTO creatRequestDTO, StringBuilder message)
        {
            bool IsValid = false;
            try
            {
                bool checkValidUser = await _authServices.CheckUserInRole(creatRequestDTO.SenderId, AppRole.ProjectManager, message);
                if (!checkValidUser)
                {
                    return IsValid;
                }

                //trạng thái dự án chưa được phép true
                if (creatRequestDTO.Status == "Pending")
                {
                    throw new Exception("Status must be Approve");
                }
                // ngày tạo không được lớn hơn ngày hiện tại
                if (creatRequestDTO.CreatedAt > DateTime.Now)
                {
                    throw new Exception("Date of creation cannot be in the future");
                }
                IsValid = true;
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"ValidateProjectCreate: {ex.Message}");
            }

            return IsValid;
        }
        //public async Task<MoveNextCampaignStatusRequest> ConvertDtoToBaseClass(CreateMoveNextCampaignStatusRequestDTO requestDTO)
        //{
        //    MoveNextCampaignStatusRequest moveNextCSR = null!;
        //    try
        //    {
        //        moveNextCSR = new MoveNextCampaignStatusRequest
        //        {
        //            SenderId = requestDTO.SenderId,
        //            ProjectName = createProject.ProjectName,
        //            Description = createProject.Description,
        //            DateOfCreation = createProject.DateOfCreation,
        //            Status = createProject.Status,
        //            OrganizationId = createProject.OrganizationId != 0 ? createProject.OrganizationId : null
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        await Console.Out.WriteLineAsync($"ConvertDtoToBaseClass: {ex.Message}");
        //    }

        //    return project;
        //}
        public async Task<MoveNextCampaignStatusResponseDTO?> MapToDto(MoveNextCampaignStatusRequest request)
        {
            return new MoveNextCampaignStatusResponseDTO
            {
                CampaignID = request.CampaignID,
                StatusOfCampaign = request.StatusOfCampaign,
                Status = request.Status,
                Title = request.Title,
                SenderId = request.SenderId,
                CreatedAt = request.CreatedAt
            };
        }
    }
}
