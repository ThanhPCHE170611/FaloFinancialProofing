using System.Text;
using FALOFinancialProofing.DTOs.MoveNextCampaignStatusRequestHistoryDTO;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.EntityFrameworkCore;

namespace FALOFinancialProofing.Services.MoveNextCampaignStatusRequestHistoryService
{
    public class MoveNextCampaignStatusRequestHistoryService : IMoveNextCampaignStatusRequestHistoryService
    {
        private readonly IRepository<MoveNextCampaignStatusRequestHistory, int> _moveNextCampaignStatusRequestHistoryRepository;
        private readonly IRepository<MoveNextCampaignStatusRequest, int> _moveNextCampaignStatusRequestRepository;
        private readonly AuthServices _authServices;
        private readonly IRepository<Campaign, int> _campaignRepository;
        public MoveNextCampaignStatusRequestHistoryService(IRepository<MoveNextCampaignStatusRequestHistory, int> moveNextCampaignStatusRequestHistoryRepository,
            IRepository<MoveNextCampaignStatusRequest, int> moveNextCampaignStatusRequestRepository,
            AuthServices authServices,
            IRepository<Campaign, int> campaignRepository)
        {

            _moveNextCampaignStatusRequestHistoryRepository = moveNextCampaignStatusRequestHistoryRepository;
            _moveNextCampaignStatusRequestRepository = moveNextCampaignStatusRequestRepository;
            _authServices = authServices;
            _campaignRepository = campaignRepository;
        }

        public async Task<MoveNextCampaignStatusRequestHistory> ConvertToBaseClass(MoveNextCampaignStatusRequestHistoryDTO historyDTO)
        {
            MoveNextCampaignStatusRequestHistory moveNextCampaignStatusRequestHistory = new MoveNextCampaignStatusRequestHistory()
            {
                MoveNextCampaignStatusRequestId = historyDTO.MoveNextCampaignStatusRequestId,
                ReceiverId = historyDTO.ReceiverId,
                DateOfApproval = historyDTO.DateOfApproval,
                Feedback = historyDTO.Feedback,
                IsAllowed = historyDTO.IsAllowed
            };
            return moveNextCampaignStatusRequestHistory;
        }
        public async Task<bool> CheckValidMoveNextCampaignStatusRequestHistory(MoveNextCampaignStatusRequestHistoryDTO moveNextCampaignStatusRequestHistoryDTO, StringBuilder message)
        {
            bool checkValid = false;
            try
            {
                // kiểm tra người dùng hợp lệ(không được gắn người dùng khác chấp nhận nếu đăng nhập tài khoản này)
                bool checkValidUser = await _authServices.CheckUserInRole(moveNextCampaignStatusRequestHistoryDTO.ReceiverId, AppRole.ProjectManagementBoard, message);
                if (!checkValidUser)
                {
                    return checkValid;
                }
                // kiểm tra MoveNextCampaignStatusRequest hợp lệ hay không
                var checkRequestId = await _moveNextCampaignStatusRequestRepository.Get(moveNextCampaignStatusRequestHistoryDTO.MoveNextCampaignStatusRequestId);
                if (checkRequestId == null)
                {
                    throw new Exception("MoveNextCampaignStatusRequest not found");
                }
                // kiểm tra thời điểm gửi yêu cầu phê duyệt
                if (moveNextCampaignStatusRequestHistoryDTO.DateOfApproval > DateTime.Now)
                {
                    throw new Exception("Date Of Approval cannot be in the future");
                }


                //await _createProjectRequestApproveHistoriesRepository.InsertAsync(createCreateProjectRequestApproveHistory);

                checkValid = true;
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"CheckValidMoveNextCampaignStatusRequestHistory: {ex.Message}!");
            }

            return checkValid;
        }

        public async Task<bool> DeleteMoveNextCampaignStatusRequestHistoryAsync(int id)
        {
            MoveNextCampaignStatusRequestHistory moveNextCampaignStatusRequestHistory = null!;
            bool result = false;
            try
            {
                moveNextCampaignStatusRequestHistory = await _moveNextCampaignStatusRequestHistoryRepository.Get(id);
                if (moveNextCampaignStatusRequestHistory == null)
                {
                    throw new Exception("MoveNextCampaignStatusRequestHistory not found!");
                }
                result = await _moveNextCampaignStatusRequestHistoryRepository.DeleteAsync(moveNextCampaignStatusRequestHistory);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"DeleteMoveNextCampaignStatusRequestHistory: {ex.Message}");
            }

            return result;
        }

        public async Task<IEnumerable<MoveNextCampaignStatusRequestHistory>> GetAllMoveNextCampaignStatusRequestHistoriesAsync()
        {
            List<MoveNextCampaignStatusRequestHistory> data = null!;
            try
            {
                data = await _moveNextCampaignStatusRequestHistoryRepository.GetAll().ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllMoveNextCampaignStatusRequestHistories: {ex.Message}");
            }

            return data;
        }

        public async Task<MoveNextCampaignStatusRequestHistory> GetMoveNextCampaignStatusRequestHistoryByIdAsync(int id)
        {
            MoveNextCampaignStatusRequestHistory moveNextCampaignStatusRequestHistory = null!;
            try
            {
                moveNextCampaignStatusRequestHistory = await _moveNextCampaignStatusRequestHistoryRepository.Get(id);
                if (moveNextCampaignStatusRequestHistory == null)
                {
                    throw new Exception("MoveNextCampaignStatusRequestHistory not found");
                }

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetMoveNextCampaignStatusRequestHistoryById: {ex.Message}");
            }

            return moveNextCampaignStatusRequestHistory;
        }

        public async Task<bool> CreateMoveNextCampaignStatusRequestHistoryAsync(MoveNextCampaignStatusRequestHistoryDTO moveNextCampaignStatusRequestHistoryDTO, StringBuilder message)
        {
            bool checkValid = false;
            try
            {
                if (moveNextCampaignStatusRequestHistoryDTO == null)
                {
                    throw new Exception("MoveNextCampaignStatusRequestHistory is null");
                }
                // kiểm tra tồn tại history nào chưa
                var requestHistoriesCreated = await _moveNextCampaignStatusRequestHistoryRepository.GetAll()
                    .Where(x => x.MoveNextCampaignStatusRequestId == moveNextCampaignStatusRequestHistoryDTO.MoveNextCampaignStatusRequestId)
                    .ToListAsync();
                // dữ liệu bảng liên quan để cập nhật thuộc tính
                var moveNextCampaignStatusRequest = await _moveNextCampaignStatusRequestRepository.Get(moveNextCampaignStatusRequestHistoryDTO.MoveNextCampaignStatusRequestId);
                var campaign = await _campaignRepository.Get(moveNextCampaignStatusRequest.CampaignID);
                if (requestHistoriesCreated.Count == 0)
                {
                    // tạo history mới
                    MoveNextCampaignStatusRequestHistory moveNextCampaignStatusRequestHistory = await ConvertToBaseClass(moveNextCampaignStatusRequestHistoryDTO);
                    await _moveNextCampaignStatusRequestHistoryRepository.InsertAsync(moveNextCampaignStatusRequestHistory);
                    // kiểm tra lại trong db( bước này quan trọng vì có thể có nhiều người cùng phê duyệt)
                    requestHistoriesCreated = await _moveNextCampaignStatusRequestHistoryRepository.GetAll()
                    .Where(x => x.MoveNextCampaignStatusRequestId == moveNextCampaignStatusRequestHistoryDTO.MoveNextCampaignStatusRequestId)
                    .ToListAsync();
                    // th có một history mới được tạo => thực hiện cập nhật bảng MoveNextCampaignStatusRequest,Campaign,(từ chối hoặc đồng ý - chuyển giai đoạn)
                    if (requestHistoriesCreated.Count == 1)
                    {
                        if (moveNextCampaignStatusRequestHistoryDTO.IsAllowed)
                        {
                            moveNextCampaignStatusRequest.Status = RequestStatus.Accepted;
                            campaign.Status = moveNextCampaignStatusRequest.StatusOfCampaign;
                        }
                        else
                        {
                            moveNextCampaignStatusRequest.Status = RequestStatus.Rejected;
                        }
                    }
                    else
                    {
                        bool checkRejectedExist = requestHistoriesCreated.Any(x => x.IsAllowed == false);
                        if (checkRejectedExist)
                        {
                            // tìm ra thằng rejectĐầu tiên trong Db chứ k phải theo request hiện tại( vì có thể là đồng ý)
                            var requestHistoryRejected = requestHistoriesCreated.OrderBy(r => r.DateOfApproval)
                                .Where(r => !r.IsAllowed).First();
                            moveNextCampaignStatusRequest.ReceiverId = requestHistoryRejected.ReceiverId;
                            moveNextCampaignStatusRequest.CreatedAt = requestHistoryRejected.DateOfApproval;
                            moveNextCampaignStatusRequest.Feedback = requestHistoryRejected.Feedback;
                            moveNextCampaignStatusRequest.Status = RequestStatus.Rejected;
                            await _moveNextCampaignStatusRequestRepository.UpdateAsync(moveNextCampaignStatusRequest);
                            checkValid = true;
                            return checkValid;
                        }
                        else
                        {
                            var requestHistoryAccepted = requestHistoriesCreated.OrderBy(r => r.DateOfApproval).First();
                            moveNextCampaignStatusRequest.Status = RequestStatus.Accepted;
                            campaign.Status = moveNextCampaignStatusRequest.StatusOfCampaign;
                            await _campaignRepository.UpdateAsync(campaign);
                            await _moveNextCampaignStatusRequestRepository.UpdateAsync(moveNextCampaignStatusRequest);
                        }
                    }

                }
                else // th đã tồn tại history và xác nhận tuần tự 
                {
                    var checkRejectedExist = requestHistoriesCreated.Any(x => x.IsAllowed == false);
                    if (checkRejectedExist)
                    {
                        throw new Exception("This request has been rejected before");
                    }
                    var requestHistoryAccepted = requestHistoriesCreated
                        .OrderBy(r => r.DateOfApproval).First();
                    // db dong y, hien tai tu choi => cap nhat thang hien tai
                    if (requestHistoryAccepted.IsAllowed && !moveNextCampaignStatusRequestHistoryDTO.IsAllowed)
                    {
                        // tạo history mới
                        MoveNextCampaignStatusRequestHistory moveNextCampaignStatusRequestHistory = await ConvertToBaseClass(moveNextCampaignStatusRequestHistoryDTO);
                        await _moveNextCampaignStatusRequestHistoryRepository.InsertAsync(moveNextCampaignStatusRequestHistory);
                        moveNextCampaignStatusRequest.Status = RequestStatus.Rejected;
                        var beforeStatus = BeforeStatus(campaign.Status);
                        campaign.Status = beforeStatus;
                    }
                    else
                    {
                        throw new Exception("This request has been Accepted before");
                    }
                }
                moveNextCampaignStatusRequest.ReceiverId = moveNextCampaignStatusRequestHistoryDTO.ReceiverId;
                moveNextCampaignStatusRequest.CreatedAt = moveNextCampaignStatusRequestHistoryDTO.DateOfApproval;
                moveNextCampaignStatusRequest.Feedback = moveNextCampaignStatusRequestHistoryDTO.Feedback;
                await _campaignRepository.UpdateAsync(campaign);
                await _moveNextCampaignStatusRequestRepository.UpdateAsync(moveNextCampaignStatusRequest);
                checkValid = true;
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"MoveNextCampaignStatusRequestHistory: {ex.Message}!");
            }

            return checkValid;
        }

        public async Task<bool> UpdateMoveNextCampaignStatusRequestHistoryAsync(MoveNextCampaignStatusRequestHistory updateMoveNextCampaignStatusRequestHistory)
        {
            MoveNextCampaignStatusRequestHistory moveNextCampaignStatusRequestHistory = null!;
            bool result = false;
            try
            {
                moveNextCampaignStatusRequestHistory = await _moveNextCampaignStatusRequestHistoryRepository.Get(updateMoveNextCampaignStatusRequestHistory.Id);
                if (moveNextCampaignStatusRequestHistory == null)
                {
                    throw new Exception("MoveNextCampaignStatusRequestHistory not found!");
                }
                result = await _moveNextCampaignStatusRequestHistoryRepository.UpdateAsync(updateMoveNextCampaignStatusRequestHistory);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"MoveNextCampaignStatusRequestHistory: {ex.Message}");
            }

            return result;
        }

        //private string BeforeStatus(string status) 
        //{
        //    string beforeStatus;
        //    if (status == CampaignStatus.Implement)
        //    {
        //        beforeStatus = CampaignStatus.FundRaising;
        //    }
        //    else if (status == CampaignStatus.Disbursement)
        //    {
        //        beforeStatus = CampaignStatus.Implement;
        //    }
        //    else if (status == CampaignStatus.Close)
        //    {
        //        beforeStatus = CampaignStatus.Disbursement;
        //    }
        //    else 
        //    {
        //        throw new InvalidOperationException("The campaign has just begun, no previous stage");
        //    }
        //    return beforeStatus;
        //}
        private string BeforeStatus(string status) 
        {
            string beforeStatus;
            if (status == Resource.CampaignStatus_Implement)
            {
                beforeStatus = Resource.CampaignStatus_FundRaising;
            }
            else if (status == Resource.CampaignStatus_Disbursement)
            {
                beforeStatus = Resource.CampaignStatus_Implement;
            }
            else if (status == Resource.CampaignStatus_Close)
            {
                beforeStatus = Resource.CampaignStatus_Disbursement;
            }
            else 
            {
                throw new InvalidOperationException("The campaign has just begun, no previous stage");
            }
            return beforeStatus;
        }



    }
}
