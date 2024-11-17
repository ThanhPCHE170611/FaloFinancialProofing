using System.Text;
using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.DTOs.CampaignDTO;
using FALOFinancialProofing.DTOs.CreateCampaignFileDTO;
using FALOFinancialProofing.DTOs.CreateCampaignRequestDTO;
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

        public async Task<MoveNextCampaignStatusRequest?> GetMoveNextCampaignStatusRequestByIdAsync1(int id)
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
        public async Task<MoveNextCampaignStatusRequest> CreateMoveNextCampaignStatusRequestAsync(CreateMoveNextCampaignStatusRequestDTO requestDto, StringBuilder message)
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
                    throw new Exception("Not Found Campaign");
                }
                bool check = await CheckRequestHasBeenCreated(requestDto.CampaignID);
                if (!check)
                {
                    throw new Exception("The request to change the campaign status has been created, you cannot create more");
                }
                string nextStatus = "";
                if (campaign.Status == Resource.CampaignStatus_FundRaising)
                {
                    bool Ok = await CheckMoneyOfCampaignAsync(requestDto.CampaignID);
                    if (Ok)
                    {
                        nextStatus = Resource.CampaignStatus_Implement;
                    }
                    else
                    {
                        throw new Exception("Haven't raised enough funds yet");
                    }
                }
                else if (campaign.Status == Resource.CampaignStatus_Implement)
                {
                    nextStatus = Resource.CampaignStatus_Disbursement;
                }
                else if (campaign.Status == Resource.CampaignStatus_Disbursement)
                {
                    CampaignDebtResult campaignDebtResult = new CampaignDebtResult();
                    campaignDebtResult = await HasDebtInCampaignAsync(requestDto.CampaignID);
                    if (campaignDebtResult.IsValid)
                    {
                        nextStatus = Resource.CampaignStatus_Close;
                    }
                    else
                    {
                        throw new Exception($"Debt is not over yet.\nPlease contact {campaignDebtResult.NameOfAccounting} for more information");
                    }
                }
                else
                {
                    throw new Exception("Campaign closed, can not move next");
                }

                if (nextStatus != "")
                {
                    var request = new MoveNextCampaignStatusRequest
                    {
                        CampaignID = requestDto.CampaignID,
                        StatusOfCampaign = nextStatus,
                        Status = "Pending",
                        Title = requestDto.Title,
                        SenderId = requestDto.SenderId,
                        CreatedAt = DateTime.Now,
                        Description = requestDto.Description
                    };
                    await _moveNextCampaignStatusRequestRepository.InsertAsync(request);
                    return request;
                }

            }
            catch (Exception ex)
            {
                message.Append(ex.Message.ToString());
                Console.WriteLine(ex.Message.ToString());
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

        //private async Task<bool> HasDebtInCampaignAsync(int campaignId)
        //{
        //    bool IsValid = true;
        //    try
        //    {
        //        List<CampaignMember> campaignMembers = new List<CampaignMember>();
        //        campaignMembers = await _campaignMemberRepository.GetAll().Where(cm => cm.CampaignId == campaignId && cm.Debt != 0).ToListAsync();
        //        if (campaignMembers.Count > 0)
        //        {
        //            IsValid = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return IsValid;
        //}

        private async Task<CampaignDebtResult> HasDebtInCampaignAsync(int campaignId)
        {
            var campaignDebtResult = new CampaignDebtResult
            {
                IsValid = true,
                NameOfAccounting = ""
            };

            try
            {
                List<CampaignMember> listCampaignMembersDebtOtherThanZero = new List<CampaignMember>();
                listCampaignMembersDebtOtherThanZero = await _campaignMemberRepository
                    .GetAll().Include(x=> x.User)
                    .Where(cm => cm.CampaignId == campaignId && cm.Debt != 0)
                    .ToListAsync();
                
                //campaignDebtResult.NameOfAccounting = campaignMembers.Select(cm => cm.User.FirstName).ToList();

                var Accounting = await _campaignMemberRepository
                    .GetAll()
                    .Include(x=> x.User)
                    .Where(cm => cm.CampaignId == campaignId && cm.RoleId == "83292e2c-6c86-4153-bdc5-760d05ec2293")
                    .SingleAsync();

                campaignDebtResult.NameOfAccounting = Accounting.User.FirstName + " " + Accounting.User.LastName + " with gmail: " + Accounting.User.Email;
                if (listCampaignMembersDebtOtherThanZero.Count > 0)
                {
                    campaignDebtResult.IsValid = false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"HasDebtInCampaignAsync: {ex.Message}");
            }

            return campaignDebtResult;
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

        private async Task<bool> CheckRequestHasBeenCreated(int campaignId)
        {
            bool IsValid = true;
            try
            {
                var mncsrWithStatusPending = await _moveNextCampaignStatusRequestRepository.GetAll().Where(m => m.CampaignID == campaignId && m.Status == "Pending").SingleOrDefaultAsync();
                if(mncsrWithStatusPending != null)
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
            var request = await GetMoveNextCampaignStatusRequestByIdAsync1(requestId);
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
                CreatedAt = request.CreatedAt,
                Description = request.Description
            };
        }

        //public Task<bool> UpdateMoveNextCampaignStatusRequestAsync(UpdateMoveNextCampaignStatusRequestDTO updateMoveNextCampaignStatusRequestDTO)
        //{
        //    throw new NotImplementedException();
        //}



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

        public async Task<IEnumerable<MoveNextCampaignStatusRequestInformation>> GetAllMoveNextCampaignStatusRequestsByPMBAsync(StringBuilder message)
        {
            List<MoveNextCampaignStatusRequestInformation> data = null!;
            try
            {
                data = await _moveNextCampaignStatusRequestRepository.GetAll()
                    .Select(m => new MoveNextCampaignStatusRequestInformation()
                    {
                        Id = m.Id,
                        SenderId = m.SenderId,
                        SenderName = $"{m.SenderUser.FirstName} {m.SenderUser.LastName}",
                        ReceiverId = m.ReceiverId,
                        ReceiverName = $"{m.ReceiverUser.FirstName} {m.ReceiverUser.LastName}",
                        CampaignID = m.CampaignID,
                        CampaignName = m.Campaign.Title,
                        Title = m.Title,
                        CreatedAt = m.CreatedAt,
                        Feedback = m.Feedback,
                        Description = m.Description,
                        Status = m.Status,
                        StatusOfCampaign = m.StatusOfCampaign,
                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"GetAllMoveNextCampaignStatusRequestsByPMBAsync: {ex.Message}");
            }

            return data;
        }

        public async Task<IEnumerable<MoveNextCampaignStatusRequestInformation>> GetAllMoveNextCampaignStatusRequestsByUserIdAsync(string userId, StringBuilder message)
        {
            List<MoveNextCampaignStatusRequestInformation> data = null!;
            try
            {
                data = await _moveNextCampaignStatusRequestRepository.GetAll()
                    .Select(m => new MoveNextCampaignStatusRequestInformation()
                    {
                        Id = m.Id,
                        SenderId = m.SenderId,
                        SenderName = $"{m.SenderUser.FirstName} {m.SenderUser.LastName}",
                        ReceiverId = m.ReceiverId,
                        ReceiverName = $"{m.ReceiverUser.FirstName} {m.ReceiverUser.LastName}",
                        CampaignID = m.CampaignID,
                        CampaignName = m.Campaign.Title,
                        Title = m.Title,
                        CreatedAt = m.CreatedAt,
                        Feedback = m.Feedback,
                        Description = m.Description,
                        Status = m.Status,
                        StatusOfCampaign = m.StatusOfCampaign,
                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"GetAllMoveNextCampaignStatusRequestsByUserIdAsync: {ex.Message}");
            }

            return data;
        }

        public async Task<bool> UpdateMoveNextCampaignStatusRequestAsync(MoveNextCampaignStatusRequest updateMoveNextCampaignStatusRequest)
        {
            MoveNextCampaignStatusRequest moveNextCampaignStatusRequest = null!;
            bool result = false;
            try
            {
                moveNextCampaignStatusRequest = await _moveNextCampaignStatusRequestRepository.Get(updateMoveNextCampaignStatusRequest.Id);
                if (moveNextCampaignStatusRequest == null)
                {
                    throw new Exception("MoveNextCampaignStatusRequest not found!");
                }
                //ConvertToBaseEntity(createCampaignRequest, updateCreateCampaignRequest);
                result = await _moveNextCampaignStatusRequestRepository.UpdateAsync(updateMoveNextCampaignStatusRequest);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"UpdateMoveNextCampaignStatusRequestAsync: {ex.Message}");
            }

            return result;
        }
        public async Task<bool> CancelMoveNextCampaignStatusRequestAsync(int requestId, string senderId, StringBuilder message)
        {
            try
            {
                // Tìm yêu cầu MoveNextCampaignStatusRequest theo ID
                var request = await _moveNextCampaignStatusRequestRepository.Get(requestId);

                // Kiểm tra nếu yêu cầu không tồn tại
                if (request == null)
                {
                    message.Append($"MoveNextCampaignStatusRequest with ID = {requestId} not found.");
                    return false;
                }

                // Kiểm tra nếu người gọi không phải là người tạo yêu cầu (SenderId)
                if (request.SenderId != senderId)
                {
                    message.Append("You are not authorized to cancel this request.");
                    return false;
                }

                // Kiểm tra nếu trạng thái của yêu cầu không phải là "pending"
                if (request.Status != "Pending")
                {
                    message.Append("Only requests with status 'pending' can be cancelled.");
                    return false;
                }

                // Chuyển trạng thái sang "cancel"
                request.Status = "Cancel";
                await _moveNextCampaignStatusRequestRepository.UpdateAsync(request);

                message.Append("Request cancelled successfully.");
                return true;
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                Console.WriteLine($"CancelMoveNextCampaignStatusRequestAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<MoveNextCampaignStatusRequestInformation> GetMoveNextCampaignStatusRequestByIdAsync(int id)
        {
            MoveNextCampaignStatusRequestInformation data = null!;
            try
            {
                data = await _moveNextCampaignStatusRequestRepository.GetAll()
                     .Where(mncpsr => mncpsr.Id == id)
                    .Select(s => new MoveNextCampaignStatusRequestInformation()
                    {
                        SenderId = s.SenderId,
                        SenderName = $"{s.SenderUser.FirstName} {s.SenderUser.LastName}",
                        ReceiverId = s.ReceiverId,
                        ReceiverName = $"{s.ReceiverUser.FirstName} {s.ReceiverUser.LastName}",
                        CampaignID = s.CampaignID,
                        CampaignName = s.Campaign.Title,
                        Title = s.Title,
                        Description = s.Description,
                        CreatedAt = s.CreatedAt,
                        Feedback = s.Feedback,
                        Status = s.Status,
                        StatusOfCampaign = s.StatusOfCampaign
                    }).SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetMoveNextCampaignStatusRequestByIdAsync: {ex.Message}");
            }

            return data;
        }
    }
}
