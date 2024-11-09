using System.Text;
using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.DTOs.CampaignDTO;
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
        private readonly IRepository<CampaignMember, int> _campaignMemberRepository;
        public MoveNextCampaignStatusRequestService(IRepository<MoveNextCampaignStatusRequest, int> moveNextCampaignStatusRequestRepository,
            IRepository<Campaign, int> campaignRepository,
            AuthServices authServices,
            IRepository<CampaignMember, int> campaignMemberRepository)
        {
            _moveNextCampaignStatusRequestRepository = moveNextCampaignStatusRequestRepository;
            _campaignRepository = campaignRepository;
            _authServices = authServices;
            _campaignMemberRepository = campaignMemberRepository;
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
        //tao don move next campaign status request
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
                string nextStatus = "";
                if (campaign.Status == Resource.CampaignStatus_FundRaising)
                {
                    bool Ok = await CheckMoneyOfCampaignAsync(requestDto.CampaignID);
                    if (Ok)
                    {
                        nextStatus = Resource.CampaignStatus_Implement;
                    }
                }
                else if (campaign.Status == Resource.CampaignStatus_Implement)
                {
                    nextStatus = Resource.CampaignStatus_Disbursement;
                }
                else if (campaign.Status == Resource.CampaignStatus_Disbursement)
                {
                    bool Ok = await HasDebtInCampaignAsync(requestDto.CampaignID);
                    if (Ok)
                    {
                        nextStatus = Resource.CampaignStatus_Close;
                    }
                    //throw new InvalidOperationException("debt is not over yet");
                }
                else
                {
                    throw new InvalidOperationException("Campaign closed, can not move next");
                }

                if (nextStatus != "")
                {
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

        private async Task<bool> HasDebtInCampaignAsync(int campaignId)
        {
            bool IsValid = true;
            try
            {
                List<CampaignMember> campaignMembers = new List<CampaignMember>();
                campaignMembers = await _campaignMemberRepository.GetAll().Where(cm => cm.CampaignId == campaignId && cm.Debt != 0).ToListAsync();
                if (campaignMembers.Count > 0)
                {
                    IsValid = false;
                }
            }
            catch (Exception ex)
            {
            }
            return IsValid;
        }


        private async Task<bool> CheckMoneyOfCampaignAsync(int campaignId)
        {
            bool IsValid = true;
            try
            {
                var data = await _campaignRepository.GetAll()
                    .Where(p => p.Id == campaignId)
                       .Select(p => new CampaignInformation()
                       {
                           FirstName = p.User.FirstName,
                           LastName = p.User.LastName,
                           CampaignId = p.Id,
                           ProjectId = p.ProjectId,
                           CreateBy = p.CreateBy,
                           Title = p.Title,
                           Description = p.Description,
                           DateOfCreation = p.DateOfCreation,
                           FundTarget = p.FundTarget,
                           Image = p.Image,
                           EndDate = p.EndDate,
                           Address = p.Address,
                           IsActive = p.IsActive,
                           BankingNumber = p.BankingNumber,
                           BankId = p.BankId,
                           Status = p.Status,
                           TotalMoneyEarned = p.TransactionLogs.Sum(x => x.Amount)
                       }).SingleOrDefaultAsync();
                double sumOfAmount = 0;
                //Campaign campaign = new Campaign();
                //campaign = await _campaignRepository.Get(campaignId);
                sumOfAmount = data.TotalMoneyEarned;

                if (sumOfAmount < data.FundTarget)
                {
                    IsValid = false;
                }
            }
            catch (Exception ex)
            {
            }
            return IsValid;
        }


        public async Task<bool> ApproveOrRejectRequestAsync(int requestId, bool isApproved)
        {
            var request = await GetMoveNextCampaignStatusRequestByIdAsync(requestId);
            if (request == null || request.Status != "Pending")
                throw new InvalidOperationException("Request not found or already processed.");

            if (isApproved)
            {
                request.Status = "Approve";
                await _moveNextCampaignStatusRequestRepository.UpdateAsync(request);
                // Cập nhật trạng thái Campaign
                var campaign = await GetCampaignByIdAsync(request.CampaignID);
                if (campaign == null) throw new InvalidOperationException("Campaign not found.");

                campaign.Status = request.StatusOfCampaign;
                await _campaignRepository.UpdateAsync(campaign);
            }
            else
            {
                request.Status = "Reject";
                await _moveNextCampaignStatusRequestRepository.UpdateAsync(request);
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
        public async Task<CreateMoveNextCampaignStatusRequestDTO?> MapToDto(MoveNextCampaignStatusRequest request)
        {
            return new CreateMoveNextCampaignStatusRequestDTO
            {
                Id = request.Id,
                CampaignID = request.CampaignID,
                StatusOfCampaign = request.StatusOfCampaign,
                Status = request.Status,
                Title = request.Title,
                SenderId = request.SenderId,
                CreatedAt = request.CreatedAt
            };
        }

        public Task<bool> UpdateMoveNextCampaignStatusRequestAsync(UpdateMoveNextCampaignStatusRequestDTO updateMoveNextCampaignStatusRequestDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ValidateCampaignCreateAsync(CreateMoveNextCampaignStatusRequestDTO createMoveNextCampaignStatusRequestDTO, StringBuilder message)
        {
            bool IsValid = false;
            try
            {
                bool checkValidUser = await _authServices.CheckUserInRole(createMoveNextCampaignStatusRequestDTO.SenderId, AppRole.ProjectManager, message);
                if (!checkValidUser)
                {
                    return IsValid;
                }



                var campaign = await _campaignRepository.Get(x => x.Id == createMoveNextCampaignStatusRequestDTO.CampaignID);
                if (campaign == null)
                {
                    throw new Exception($"CampaignID ={createMoveNextCampaignStatusRequestDTO.CampaignID} does not exist!");
                }
                else
                {
                    var campaignIsActive = campaign.IsActive;
                    if (!campaignIsActive)
                    {
                        throw new Exception($"CampaignID ={createMoveNextCampaignStatusRequestDTO.CampaignID} inactive!");
                    }
                }
                if (createMoveNextCampaignStatusRequestDTO.CreatedAt > DateTime.Now)
                {
                    throw new Exception("Date of creation cannot be in the future");
                }

                // check xem có phải PM của campaign đấy không
                var campaignMember = await _campaignMemberRepository.Get(x => x.UserId == createMoveNextCampaignStatusRequestDTO.SenderId && x.CampaignId == createMoveNextCampaignStatusRequestDTO.CampaignID);
                if (campaignMember == null)
                {
                    throw new Exception($"User with ID = {createMoveNextCampaignStatusRequestDTO.SenderId} is not associated with Campaign ID = {createMoveNextCampaignStatusRequestDTO.CampaignID}.");
                }
                var projectManagerRoleId = "205d4496-4ac8-40d9-84b9-e09e1ada7a49"; // ID của Project Manager
                if (campaignMember.RoleId != projectManagerRoleId)
                {
                    throw new Exception("User is not a Project Manager for the specified campaign.");
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
    }
}
