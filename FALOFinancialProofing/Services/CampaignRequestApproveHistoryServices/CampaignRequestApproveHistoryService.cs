using FALOFinancialProofing.DTOs.CampaignRequestApproveHistoryDTO;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace FALOFinancialProofing.Services.CampaignRequestApproveHistoryServices
{
    public class CampaignRequestApproveHistoryService : ICampaignRequestApproveHistoryService
    {
        private readonly IRepository<CampaignRequestApproveHistory, int> _campaignRequestApproveHistoriesRepository;
        private readonly IRepository<CreateCampaignRequest, int> _createCampaignRequestRepository;
        private readonly AuthServices _authServices;
        private readonly IRepository<Campaign, int> _campaignRepository;
        public CampaignRequestApproveHistoryService(IRepository<CampaignRequestApproveHistory, int> createCampaignRequestApproveHistoriesRepository, AuthServices authServices, IRepository<CreateCampaignRequest, int> createCampaignRequestRepository, IRepository<Campaign, int> campaignRepository)
        {
            _campaignRequestApproveHistoriesRepository = createCampaignRequestApproveHistoriesRepository;
            _authServices = authServices;
            _createCampaignRequestRepository = createCampaignRequestRepository;
            _campaignRepository = campaignRepository;
        }
        public async Task<CampaignRequestApproveHistory> ConvertToBaseClass(CampaignRequestApproveHistoryClientRequest createCampaignRequestApproveHistoryClientRequest)
        {
            CampaignRequestApproveHistory createCampaignRequestApproveHistory = new CampaignRequestApproveHistory()
            {
                CampaignRequestId = createCampaignRequestApproveHistoryClientRequest.CampaignRequestId,
                ApproverId = createCampaignRequestApproveHistoryClientRequest.ApproverId,
                DateOfApproval = createCampaignRequestApproveHistoryClientRequest.DateOfApproval,
                IsAllowed = createCampaignRequestApproveHistoryClientRequest.IsAllowed
            };
            return createCampaignRequestApproveHistory;
        }
        public async Task<bool> CreateCampaignRequestApproveHistoryAsync(CampaignRequestApproveHistoryClientRequest createCampaignRequestApproveHistoryClientRequest, StringBuilder message)
        {
            bool checkValid = false;
            try
            {
                if (createCampaignRequestApproveHistoryClientRequest == null)
                {
                    throw new Exception("CampaignRequestApproveHistory is null");
                }
                // kiểm tra tồn tại history nào chưa
                var requestHistoriesCreated = await _campaignRequestApproveHistoriesRepository.GetAll()
                    .Where(x => x.CampaignRequestId == createCampaignRequestApproveHistoryClientRequest.CampaignRequestId)
                    .ToListAsync();
                // dữ liệu bảng liên quan để cập nhật thuộc tính
                var CampaignRequest = await _createCampaignRequestRepository.Get(createCampaignRequestApproveHistoryClientRequest.CampaignRequestId);
                var campaign = await _campaignRepository.Get(CampaignRequest.CampaignId);
                if (requestHistoriesCreated.Count == 0)
                {
                    // tạo history mới
                    CampaignRequestApproveHistory createCampaignRequestApproveHistory = await ConvertToBaseClass(createCampaignRequestApproveHistoryClientRequest);
                    await _campaignRequestApproveHistoriesRepository.InsertAsync(createCampaignRequestApproveHistory);
                    // kiểm tra lại trong db( bước này quan trọng vì có thể có nhiều người cùng phê duyệt)
                    requestHistoriesCreated = await _campaignRequestApproveHistoriesRepository.GetAll()
                    .Where(x => x.CampaignRequestId == createCampaignRequestApproveHistoryClientRequest.CampaignRequestId)
                    .ToListAsync();
                    // th có một history mới được tạo => thực hiện cập nhật bảng createRequest,Campaign,(từ chối hoặc đồng ý)
                    if (requestHistoriesCreated.Count == 1)
                    {
                        if (createCampaignRequestApproveHistoryClientRequest.IsAllowed)
                        {
                            CampaignRequest.Status = RequestStatus.Accepted;
                            campaign.IsActive = true;
                            campaign.Status = RequestStatus.FundRaising;
                        }
                        else
                        {
                            CampaignRequest.Status = RequestStatus.Rejected;
                            campaign.IsActive = false;
                            campaign.Status = RequestStatus.Rejected;
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
                            CampaignRequest.ReceiverId = requestHistoryRejected.ApproverId;
                            CampaignRequest.CreatedAt = requestHistoryRejected.DateOfApproval;
                            CampaignRequest.Status = RequestStatus.Rejected;
                            campaign.IsActive = false;
                            campaign.Status = RequestStatus.Rejected;
                            await _campaignRepository.UpdateAsync(campaign);
                            await _createCampaignRequestRepository.UpdateAsync(CampaignRequest);
                            checkValid = true;
                            return checkValid;
                        }
                        else
                        {
                            var requestHistoryAccepted = requestHistoriesCreated.OrderBy(r => r.DateOfApproval).First();
                            CampaignRequest.Status = RequestStatus.Accepted;
                            campaign.IsActive = true;
                            campaign.Status = RequestStatus.FundRaising;
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
                    if (requestHistoryAccepted.IsAllowed && !createCampaignRequestApproveHistoryClientRequest.IsAllowed)
                    {
                        // tạo history mới
                        CampaignRequestApproveHistory createCampaignRequestApproveHistory = await ConvertToBaseClass(createCampaignRequestApproveHistoryClientRequest);
                        await _campaignRequestApproveHistoriesRepository.InsertAsync(createCampaignRequestApproveHistory);
                        CampaignRequest.Status = RequestStatus.Rejected;
                        campaign.IsActive = false;
                        campaign.Status = RequestStatus.Rejected;
                    }
                    else
                    {
                        throw new Exception("This request has been Accepted before");
                    }
                }
                CampaignRequest.ReceiverId = createCampaignRequestApproveHistoryClientRequest.ApproverId;
                CampaignRequest.CreatedAt = createCampaignRequestApproveHistoryClientRequest.DateOfApproval;
                await _campaignRepository.UpdateAsync(campaign);
                await _createCampaignRequestRepository.UpdateAsync(CampaignRequest);
                checkValid = true;
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"CampaignRequestApproveHistory: {ex.Message}!");
            }

            return checkValid;
        }
        public async Task<bool> CheckValidCampaignRequestApproveHistory(CampaignRequestApproveHistoryClientRequest createCampaignRequestApproveHistoryClientRequest, StringBuilder message)
        {
            bool checkValid = false;
            try
            {
                // kiểm tra người dùng hợp lệ(không được gắn người dùng khác chấp nhận nếu đăng nhập tài khoản này)
                bool checkValidUser = await _authServices.CheckUserInRole(createCampaignRequestApproveHistoryClientRequest.ApproverId, AppRole.ProjectManagementBoard, message);
                if (!checkValidUser)
                {
                    return checkValid;
                }
                // kiểm tra CampaignRequest hợp lệ hay không
                var checkRequestId = await _createCampaignRequestRepository.Get(createCampaignRequestApproveHistoryClientRequest.CampaignRequestId);
                if (checkRequestId == null)
                {
                    throw new Exception("CampaignRequest not found");
                }
                // kiểm tra thời điểm gửi yêu cầu phê duyệt
                if (createCampaignRequestApproveHistoryClientRequest.DateOfApproval > DateTime.Now)
                {
                    throw new Exception("Date Of Approval cannot be in the future");
                }


                //await _campaignRequestApproveHistoriesRepository.InsertAsync(createCampaignRequestApproveHistory);

                checkValid = true;
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"CheckValidCampaignRequestApproveHistory: {ex.Message}!");
            }

            return checkValid;
        }

        public async Task<CampaignRequestApproveHistory> GetCampaignRequestApproveHistoryByIdAsync(int id)
        {
            CampaignRequestApproveHistory organization = null!;
            try
            {
                organization = await _campaignRequestApproveHistoriesRepository.Get(id);
                if (organization == null)
                {
                    throw new Exception("CampaignRequestApproveHistory not found");
                }

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetCampaignRequestApproveHistoryById: {ex.Message}");
            }

            return organization;
        }


        public async Task<IEnumerable<CampaignRequestApproveHistory>> GetAllCampaignRequestApproveHistoriesAsync()
        {
            List<CampaignRequestApproveHistory> data = null!;
            try
            {
                data = await _campaignRequestApproveHistoriesRepository.GetAll().ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllCampaignRequestApproveHistorys: {ex.Message}");
            }

            return data;
        }


        public async Task<bool> UpdateCampaignRequestApproveHistoryAsync(CampaignRequestApproveHistory updateCampaignRequestApproveHistory)
        {
            CampaignRequestApproveHistory organization = null!;
            bool result = false;
            try
            {
                organization = await _campaignRequestApproveHistoriesRepository.Get(updateCampaignRequestApproveHistory.Id);
                if (organization == null)
                {
                    throw new Exception("CampaignRequestApproveHistory not found!");
                }
                //ConvertToBaseEntity(organization, updateCampaignRequestApproveHistory);
                result = await _campaignRequestApproveHistoriesRepository.UpdateAsync(updateCampaignRequestApproveHistory);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"UpdateCampaignRequestApproveHistory: {ex.Message}");
            }

            return result;
        }
        // admin can delete transaction logs
        public async Task<bool> DeleteCampaignRequestApproveHistoryAsync(int id)
        {
            CampaignRequestApproveHistory organization = null!;
            bool result = false;
            try
            {
                organization = await _campaignRequestApproveHistoriesRepository.Get(id);
                if (organization == null)
                {
                    throw new Exception("CampaignRequestApproveHistory not found!");
                }
                result = await _campaignRequestApproveHistoriesRepository.DeleteAsync(organization);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"DeleteCampaignRequestApproveHistory: {ex.Message}");
            }

            return result;
        }
    }
}
