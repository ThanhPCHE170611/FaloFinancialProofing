using FALOFinancialProofing.DTOs.CreateCampaignFileDTO;
using FALOFinancialProofing.DTOs.CreateCampaignRequestDTO;
using FALOFinancialProofing.DTOs.CreateProjectFileDTO;
using FALOFinancialProofing.DTOs.CreateCampaignRequestDTO;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace FALOFinancialProofing.Services.CreateCampaignRequestServices
{
    public class CreateCampaignRequestService : ICreateCampaignRequestService
    {
        private readonly IRepository<CreateCampaignRequest, int> _createCampaignRequestRepository;

        public CreateCampaignRequestService(IRepository<CreateCampaignRequest, int> createCampaignRequestRepository)
        {
            _createCampaignRequestRepository = createCampaignRequestRepository;
        }

        public async Task<bool> CreateCreateCampaignRequestAsync(CreateCampaignRequest createCreateCampaignRequest)
        {
            try
            {
                if (createCreateCampaignRequest == null)
                {
                    throw new Exception("CreateCampaignRequest is null");
                }
                await _createCampaignRequestRepository.InsertAsync(createCreateCampaignRequest);

                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"CreateCampaignRequest: {ex.Message}!");
            }

            return false;
        }

        //public async Task<CreateCampaignRequest> GetCreateCampaignRequestByIdAsync(int id)
        //{
        //    CreateCampaignRequest createCampaignRequest = null!;
        //    try
        //    {
        //        createCampaignRequest = await _createCampaignRequestRepository.Get(id);
        //        if (createCampaignRequest == null)
        //        {
        //            throw new Exception("CreateCampaignRequest not found");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        await Console.Out.WriteLineAsync($"GetCreateCampaignRequestById: {ex.Message}");
        //    }

        //    return createCampaignRequest;
        //}

        public async Task<IEnumerable<CreateCampaignRequest>> GetAllCreateCampaignRequestsAsync()
        {
            List<CreateCampaignRequest> data = null!;
            try
            {
                data = await _createCampaignRequestRepository.GetAll().ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllCreateCampaignRequests: {ex.Message}");
            }

            return data;
        }


        // admin can update transaction logs
        public async Task<bool> UpdateCreateCampaignRequestAsync(CreateCampaignRequest updateCreateCampaignRequest)
        {
            CreateCampaignRequest createCampaignRequest = null!;
            bool result = false;
            try
            {
                createCampaignRequest = await _createCampaignRequestRepository.Get(updateCreateCampaignRequest.Id);
                if (createCampaignRequest == null)
                {
                    throw new Exception("CreateCampaignRequest not found!");
                }
                //ConvertToBaseEntity(createCampaignRequest, updateCreateCampaignRequest);
                result = await _createCampaignRequestRepository.UpdateAsync(updateCreateCampaignRequest);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"UpdateCreateCampaignRequest: {ex.Message}");
            }

            return result;
        }
        // admin can delete transaction logs
        public async Task<bool> DeleteCreateCampaignRequestAsync(int id)
        {
            CreateCampaignRequest createCampaignRequest = null!;
            bool result = false;
            try
            {
                createCampaignRequest = await _createCampaignRequestRepository.Get(id);
                if (createCampaignRequest == null)
                {
                    throw new Exception("CreateCampaignRequest not found!");
                }
                result = await _createCampaignRequestRepository.DeleteAsync(createCampaignRequest);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"DeleteCreateCampaignRequest: {ex.Message}");
            }

            return result;
        }

        public async Task<CreateCampaignRequest> CreateCreateCampaignRequestReturnEntityAsync(CreateCampaignRequest createCampaignRequest)
        {
            try
            {
                if (createCampaignRequest == null)
                {
                    throw new Exception("createCampaignRequest is null");
                }
                await _createCampaignRequestRepository.InsertAsync(createCampaignRequest);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"CreateCreateCampaignRequestReturnEntityAsync: {ex.Message}!");
            }

            return createCampaignRequest;
        }
        public async Task<IEnumerable<CreateCampaignRequestInformation>> GetAllCreateCampaignRequestsByPMBAsync(StringBuilder message)
        {
            List<CreateCampaignRequestInformation> data = null!;
            try
            {
                data = await _createCampaignRequestRepository.GetAll()
                    //.Where(pr => pr.Status.Equals(RequestStatus.Pending))
                    .Select(s => new CreateCampaignRequestInformation()
                    {
                        ProjectName = s.Campaign.Project.ProjectName,
                        Id = s.Id,
                        SenderId = s.SenderId,
                        SenderName = $"{s.SenderUser.FirstName} {s.SenderUser.LastName}",
                        ReceiverId = s.ReceiverId,
                        ReceiverName = $"{s.ReceiverUser.FirstName} {s.ReceiverUser.LastName}",
                        CampaignId = s.CampaignId,
                        Title = s.Title,
                        CreatedAt = s.CreatedAt,
                        Feedback = s.Feedback,
                        Status = s.Status,
                        CreateCampaignFiles = s.CreateCampaignFiles.Select(f => new CreateCampaignFileInformation()
                        {
                            Id = f.Id,
                            RequestId = f.RequestId,
                            FilePath = f.FilePath
                        }).ToList()
                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"GetAllCreateCampaignRequestsByPMBAsync: {ex.Message}");
            }

            return data;
        }

        public async Task<IEnumerable<CreateCampaignRequestInformation>> GetAllCreateCampaignRequestsByUserIdAsync(string userId, StringBuilder message)
        {
            List<CreateCampaignRequestInformation> data = null!;
            try
            {
                data = await _createCampaignRequestRepository.GetAll()
                    //.Where(pr => pr.Status.Equals(RequestStatus.Pending))
                    .Select(s => new CreateCampaignRequestInformation()
                    {
                        ProjectName = s.Campaign.Project.ProjectName,
                        Id = s.Id,
                        SenderId = s.SenderId,
                        SenderName = $"{s.SenderUser.FirstName} {s.SenderUser.LastName}",
                        ReceiverId = s.ReceiverId,
                        ReceiverName = $"{s.ReceiverUser.FirstName} {s.ReceiverUser.LastName}",
                        CampaignId = s.CampaignId,
                        Title = s.Title,
                        CreatedAt = s.CreatedAt,
                        Feedback = s.Feedback,
                        Status = s.Status,
                        CreateCampaignFiles = s.CreateCampaignFiles.Select(f => new CreateCampaignFileInformation()
                        {
                            Id = f.Id,
                            RequestId = f.RequestId,
                            FilePath = f.FilePath
                        }).ToList()
                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"GetAllCreateCampaignRequestsByPMBAsync: {ex.Message}");
            }

            return data;
        }

        public async Task<CreateCampaignRequestInformation> GetCreateCampaignRequestByIdAsync(int id)
        {
            CreateCampaignRequestInformation data = null!;
            try
            {
                data = await _createCampaignRequestRepository.GetAll()
                     .Where(cpr => cpr.Id == id)
                    .Select(s => new CreateCampaignRequestInformation()
                    {
                        ProjectName = s.Campaign.Project.ProjectName,
                        Id = s.Id,
                        SenderId = s.SenderId,
                        SenderName = $"{s.SenderUser.FirstName} {s.SenderUser.LastName}",
                        ReceiverId = s.ReceiverId,
                        ReceiverName = $"{s.ReceiverUser.FirstName} {s.ReceiverUser.LastName}",
                        CampaignId = s.CampaignId,
                        Title = s.Title,
                        CreatedAt = s.CreatedAt,
                        Feedback = s.Feedback,
                        Status = s.Status,
                        CampaignDescription = s.Campaign.Description,
                        CreateCampaignFiles = s.CreateCampaignFiles.Select(f => new CreateCampaignFileInformation()
                        {
                            Id = f.Id,
                            RequestId = f.RequestId,
                            FilePath = f.FilePath
                        }).ToList()
                    }).SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllCreateCampaignRequestsByPMBAsync: {ex.Message}");
            }

            return data;
        }

        public async Task<bool> ValidateCreateCampaignRequestByUserIdAndRequestIdAsync(string userId, int CreateCampaignRequestId, StringBuilder message)
        {
            bool checkValidUser = false;
            try
            {
                var request = await _createCampaignRequestRepository
                     .Get(r => r.Id == CreateCampaignRequestId && userId.Equals(r.SenderId));
                if (request == null)
                {
                    throw new Exception("Not Valid User To Cancel Request!");
                }
                if (!request.Status.Equals(RequestStatus.Pending, StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("Can not Cancel, request is not pending!");
                }
                checkValidUser = true;
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"ValidateCreateCampaignRequestByUserIdAndRequestIdAsync: {ex.Message}");
            }

            return checkValidUser;
        }

        public async Task<bool> CancelCreateCampaignRequestAsync(string userId, int CreateCampaignRequestId)
        {
            bool result = false;
            try
            {
                var createCampaignRequest = await _createCampaignRequestRepository.Get(CreateCampaignRequestId);
                createCampaignRequest.Status = RequestStatus.Cancel;
                result = await _createCampaignRequestRepository.UpdateAsync(createCampaignRequest);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"CancelCreateCampaignRequestAsync: {ex.Message}");
            }

            return result;
        }

    }
}
