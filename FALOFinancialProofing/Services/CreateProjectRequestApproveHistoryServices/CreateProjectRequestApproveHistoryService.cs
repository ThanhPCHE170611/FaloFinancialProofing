using FALOFinancialProofing.DTOs.CreateProjectRequestApproveHistoryDTO;
using FALOFinancialProofing.DTOs.ProjectDTOs;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using FALOFinancialProofing.Services.CreateProjectRequestServices;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace FALOFinancialProofing.Services.CreateProjectRequestApproveHistoryServices
{
    public class CreateProjectRequestApproveHistoryService : ICreateProjectRequestApproveHistoryService
    {
        private readonly IRepository<CreateProjectRequestApproveHistory, int> _createProjectRequestApproveHistoriesRepository;
        private readonly IRepository<CreateProjectRequest, int> _createProjectRequestRepository;
        private readonly AuthServices _authServices;
        private readonly IRepository<Project, int> _projectRepository;
        public CreateProjectRequestApproveHistoryService(IRepository<CreateProjectRequestApproveHistory, int> createProjectRequestApproveHistoriesRepository, AuthServices authServices, IRepository<CreateProjectRequest, int> createProjectRequestRepository, IRepository<Project, int> projectRepository)
        {
            _createProjectRequestApproveHistoriesRepository = createProjectRequestApproveHistoriesRepository;
            _authServices = authServices;
            _createProjectRequestRepository = createProjectRequestRepository;
            _projectRepository = projectRepository;
        }
        public async Task<CreateProjectRequestApproveHistory> ConvertToBaseClass(CreateProjectRequestApproveHistoryClientRequest createProjectRequestApproveHistoryClientRequest)
        {
            CreateProjectRequestApproveHistory createProjectRequestApproveHistory = new CreateProjectRequestApproveHistory()
            {
                CreateProjectRequestId = createProjectRequestApproveHistoryClientRequest.CreateProjectRequestId,
                ApproverId = createProjectRequestApproveHistoryClientRequest.ApproverId,
                DateOfApproval = createProjectRequestApproveHistoryClientRequest.DateOfApproval,
                IsAllowed = createProjectRequestApproveHistoryClientRequest.IsAllowed,
                FeedBack = createProjectRequestApproveHistoryClientRequest.FeedBack
            };
            return createProjectRequestApproveHistory;
        }
        public async Task<bool> CreateCreateProjectRequestApproveHistoryAsync(CreateProjectRequestApproveHistoryClientRequest createProjectRequestApproveHistoryClientRequest, StringBuilder message)
        {
            bool checkValid = false;
            try
            {
                if (createProjectRequestApproveHistoryClientRequest == null)
                {
                    throw new Exception("CreateProjectRequestApproveHistory is null");
                }
                // kiểm tra tồn tại history nào chưa
                var requestHistoriesCreated = await _createProjectRequestApproveHistoriesRepository.GetAll()
                    .Where(x => x.CreateProjectRequestId == createProjectRequestApproveHistoryClientRequest.CreateProjectRequestId)
                    .ToListAsync();
                // dữ liệu bảng liên quan để cập nhật thuộc tính
                var CreateProjectRequest = await _createProjectRequestRepository.Get(createProjectRequestApproveHistoryClientRequest.CreateProjectRequestId);
                var project = await _projectRepository.Get(CreateProjectRequest.ProjectId);
                if (requestHistoriesCreated.Count == 0)
                {
                    // tạo history mới
                    CreateProjectRequestApproveHistory createProjectRequestApproveHistory = await ConvertToBaseClass(createProjectRequestApproveHistoryClientRequest);
                    await _createProjectRequestApproveHistoriesRepository.InsertAsync(createProjectRequestApproveHistory);
                    // kiểm tra lại trong db( bước này quan trọng vì có thể có nhiều người cùng phê duyệt)
                    requestHistoriesCreated = await _createProjectRequestApproveHistoriesRepository.GetAll()
                    .Where(x => x.CreateProjectRequestId == createProjectRequestApproveHistoryClientRequest.CreateProjectRequestId)
                    .ToListAsync();
                    // th có một history mới được tạo => thực hiện cập nhật bảng createRequest,project,(từ chối hoặc đồng ý)
                    if (requestHistoriesCreated.Count == 1)
                    {
                        if (createProjectRequestApproveHistoryClientRequest.IsAllowed)
                        {
                            CreateProjectRequest.Status = RequestStatus.Accepted;
                            project.IsActive = true;
                            project.Status = RequestStatus.Running;
                        }
                        else
                        {
                            CreateProjectRequest.Feedback = createProjectRequestApproveHistoryClientRequest.FeedBack;
                            CreateProjectRequest.Status = RequestStatus.Rejected;
                            project.IsActive = false;
                            project.Status = RequestStatus.Rejected;
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
                            CreateProjectRequest.ReceiverId = requestHistoryRejected.ApproverId;
                            CreateProjectRequest.CreatedAt = requestHistoryRejected.DateOfApproval;
                            CreateProjectRequest.Status = RequestStatus.Rejected;
                            CreateProjectRequest.Feedback = requestHistoryRejected.FeedBack;
                            project.IsActive = false;
                            project.Status = RequestStatus.Rejected;
                            await _projectRepository.UpdateAsync(project);
                            await _createProjectRequestRepository.UpdateAsync(CreateProjectRequest);
                            checkValid = true;
                            return checkValid;
                        }
                        else
                        {
                            var requestHistoryAccepted = requestHistoriesCreated.OrderBy(r => r.DateOfApproval).First();
                            CreateProjectRequest.Status = RequestStatus.Accepted;
                            project.IsActive = true;
                            project.Status = RequestStatus.Running;
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
                    if (requestHistoryAccepted.IsAllowed && !createProjectRequestApproveHistoryClientRequest.IsAllowed)
                    {
                        // tạo history mới
                        CreateProjectRequestApproveHistory createProjectRequestApproveHistory = await ConvertToBaseClass(createProjectRequestApproveHistoryClientRequest);
                        await _createProjectRequestApproveHistoriesRepository.InsertAsync(createProjectRequestApproveHistory);
                        CreateProjectRequest.Status = RequestStatus.Rejected;
                        CreateProjectRequest.Feedback = createProjectRequestApproveHistoryClientRequest.FeedBack;
                        project.IsActive = false;
                        project.Status = RequestStatus.Rejected;
                    }
                    else
                    {
                        throw new Exception("This request has been Accepted before");
                    }
                }
                CreateProjectRequest.ReceiverId = createProjectRequestApproveHistoryClientRequest.ApproverId;
                CreateProjectRequest.CreatedAt = createProjectRequestApproveHistoryClientRequest.DateOfApproval;
                await _projectRepository.UpdateAsync(project);
                await _createProjectRequestRepository.UpdateAsync(CreateProjectRequest);
                checkValid = true;
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"CreateProjectRequestApproveHistory: {ex.Message}!");
            }

            return checkValid;
        }
        public async Task<bool> CheckValidCreateProjectRequestApproveHistory(CreateProjectRequestApproveHistoryClientRequest createProjectRequestApproveHistoryClientRequest, StringBuilder message)
        {
            bool checkValid = false;
            try
            {
                // kiểm tra người dùng hợp lệ(không được gắn người dùng khác chấp nhận nếu đăng nhập tài khoản này)
                bool checkValidUser = await _authServices.CheckUserInRole(createProjectRequestApproveHistoryClientRequest.ApproverId, AppRole.ProjectManagementBoard, message);
                if (!checkValidUser)
                {
                    return checkValid;
                }
                // kiểm tra CreateProjectRequest hợp lệ hay không
                var checkRequestId = await _createProjectRequestRepository.Get(createProjectRequestApproveHistoryClientRequest.CreateProjectRequestId);
                if (checkRequestId == null)
                {
                    throw new Exception("CreateProjectRequest not found");
                }
                if (checkRequestId.Status.Equals(RequestStatus.Cancel))
                {
                    throw new Exception("Project Request has been cancel before");
                }
                // kiểm tra thời điểm gửi yêu cầu phê duyệt
                if (createProjectRequestApproveHistoryClientRequest.DateOfApproval > DateTime.Now)
                {
                    throw new Exception("Date Of Approval cannot be in the future");
                }
                if (!createProjectRequestApproveHistoryClientRequest.IsAllowed && createProjectRequestApproveHistoryClientRequest.FeedBack == null)
                {
                    throw new Exception("Feedback is required when rejecting a request");
                }
                checkValid = true;
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"CheckValidCreateProjectRequestApproveHistory: {ex.Message}!");
            }

            return checkValid;
        }

        public async Task<CreateProjectRequestApproveHistory> GetCreateProjectRequestApproveHistoryByIdAsync(int id)
        {
            CreateProjectRequestApproveHistory organization = null!;
            try
            {
                organization = await _createProjectRequestApproveHistoriesRepository.Get(id);
                if (organization == null)
                {
                    throw new Exception("CreateProjectRequestApproveHistory not found");
                }

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetCreateProjectRequestApproveHistoryById: {ex.Message}");
            }

            return organization;
        }


        public async Task<IEnumerable<CreateProjectRequestApproveHistory>> GetAllCreateProjectRequestApproveHistoriesAsync()
        {
            List<CreateProjectRequestApproveHistory> data = null!;
            try
            {
                data = await _createProjectRequestApproveHistoriesRepository.GetAll().ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllCreateProjectRequestApproveHistorys: {ex.Message}");
            }

            return data;
        }


        public async Task<bool> UpdateCreateProjectRequestApproveHistoryAsync(CreateProjectRequestApproveHistory updateCreateProjectRequestApproveHistory)
        {
            CreateProjectRequestApproveHistory organization = null!;
            bool result = false;
            try
            {
                organization = await _createProjectRequestApproveHistoriesRepository.Get(updateCreateProjectRequestApproveHistory.Id);
                if (organization == null)
                {
                    throw new Exception("CreateProjectRequestApproveHistory not found!");
                }
                //ConvertToBaseEntity(organization, updateCreateProjectRequestApproveHistory);
                result = await _createProjectRequestApproveHistoriesRepository.UpdateAsync(updateCreateProjectRequestApproveHistory);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"UpdateCreateProjectRequestApproveHistory: {ex.Message}");
            }

            return result;
        }
        // admin can delete transaction logs
        public async Task<bool> DeleteCreateProjectRequestApproveHistoryAsync(int id)
        {
            CreateProjectRequestApproveHistory organization = null!;
            bool result = false;
            try
            {
                organization = await _createProjectRequestApproveHistoriesRepository.Get(id);
                if (organization == null)
                {
                    throw new Exception("CreateProjectRequestApproveHistory not found!");
                }
                result = await _createProjectRequestApproveHistoriesRepository.DeleteAsync(organization);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"DeleteCreateProjectRequestApproveHistory: {ex.Message}");
            }

            return result;
        }
    }
}
