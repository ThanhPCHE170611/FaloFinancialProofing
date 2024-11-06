using FALOFinancialProofing.DTOs.CreateProjectFileDTO;
using FALOFinancialProofing.DTOs.CreateProjectRequestDTO;
using FALOFinancialProofing.DTOs.ProjectDTOs;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FALOFinancialProofing.Services.CreateProjectRequestServices
{
    public class CreateProjectRequestService : ICreateProjectRequestService
    {
        private readonly IRepository<CreateProjectRequest, int> _createProjectRequestRepository;
        public readonly UserManager<User> _userManager;

        public CreateProjectRequestService(IRepository<CreateProjectRequest, int> createProjectRequestRepository, UserManager<User> userManager)
        {
            _createProjectRequestRepository = createProjectRequestRepository;
            _userManager = userManager;
        }

        public async Task<bool> CreateCreateProjectRequestAsync(CreateProjectRequest createCreateProjectRequest)
        {
            try
            {
                if (createCreateProjectRequest == null)
                {
                    throw new Exception("CreateProjectRequest is null");
                }
                await _createProjectRequestRepository.InsertAsync(createCreateProjectRequest);

                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"CreateProjectRequest: {ex.Message}!");
            }

            return false;
        }

        public async Task<CreateProjectRequest> CreateCreateProjectRequestReturnEntityAsync(CreateProjectRequest createCreateProjectRequest)
        {
            try
            {
                if (createCreateProjectRequest == null)
                {
                    throw new Exception("CreateProjectRequest is null");
                }
                await _createProjectRequestRepository.InsertAsync(createCreateProjectRequest);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"CreateCreateProjectRequestReturnEntityAsync: {ex.Message}!");
            }

            return createCreateProjectRequest;
        }

        public async Task<CreateProjectRequestInformation> GetCreateProjectRequestByIdAsync(int id)
        {
            CreateProjectRequestInformation createProjectRequest = null!;
            try
            {
                createProjectRequest = await _createProjectRequestRepository.GetAll()
                    .Where(cpr => cpr.Id == id)
                    .Select(s => new CreateProjectRequestInformation()
                    {
                        Id = s.Id,
                        SenderId = s.SenderId,
                        SenderName = $"{s.SenderUser.FirstName} {s.SenderUser.LastName}",
                        ReceiverId = s.ReceiverId,
                        ReceiverName = $"{s.ReceiverUser.FirstName} {s.ReceiverUser.LastName}",
                        ProjectId = s.ProjectId,
                        Title = s.Title,
                        CreatedAt = s.CreatedAt,
                        Feedback = s.Feedback,
                        Status = s.Status,
                        CreateProjectFiles = s.CreateProjectFiles.Select(f => new CreateProjectFileInformation()
                        {
                            Id = f.Id,
                            RequestId = f.RequestId,
                            FilePath = f.FilePath
                        }).ToList()
                    }).SingleOrDefaultAsync();
                if (createProjectRequest == null)
                {
                    throw new Exception("CreateProjectRequest not found");
                }

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetCreateProjectRequestById: {ex.Message}");
            }

            return createProjectRequest;
        }

        public async Task<IEnumerable<CreateProjectRequest>> GetAllCreateProjectRequestsAsync()
        {
            List<CreateProjectRequest> data = null!;
            try
            {
                data = await _createProjectRequestRepository.GetAll().ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllCreateProjectRequests: {ex.Message}");
            }

            return data;
        }

        public async Task<IEnumerable<CreateProjectRequestInformation>> GetAllCreateProjectRequestsByPMBAsync(StringBuilder message)
        {
            List<CreateProjectRequestInformation> data = null!;
            try
            {
                data = await _createProjectRequestRepository.GetAll()
                    //.Where(pr => pr.Status.Equals(RequestStatus.Pending))
                    .Select(s => new CreateProjectRequestInformation()
                    {
                        Id = s.Id,
                        SenderId = s.SenderId,
                        SenderName = $"{s.SenderUser.FirstName} {s.SenderUser.LastName}",
                        ReceiverId = s.ReceiverId,
                        ReceiverName = $"{s.ReceiverUser.FirstName} {s.ReceiverUser.LastName}",
                        ProjectId = s.ProjectId,
                        Title = s.Title,
                        CreatedAt = s.CreatedAt,
                        Feedback = s.Feedback,
                        Status = s.Status,
                        CreateProjectFiles = s.CreateProjectFiles.Select(f => new CreateProjectFileInformation()
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
                await Console.Out.WriteLineAsync($"GetAllCreateProjectRequestsByPMBAsync: {ex.Message}");
            }

            return data;
        }

        public async Task<IEnumerable<CreateProjectRequestInformation>> GetAllCreateProjectRequestsByUserIdAsync(string userId, StringBuilder message)
        {
            List<CreateProjectRequestInformation> data = null!;
            try
            {
                data = await _createProjectRequestRepository.GetAll()
                    .Where(pr => pr.SenderId.Equals(userId))
                    .Select(s => new CreateProjectRequestInformation()
                    {
                        Id = s.Id,
                        SenderId = s.SenderId,
                        SenderName = $"{s.SenderUser.FirstName} {s.SenderUser.LastName}",
                        ReceiverId = s.ReceiverId,
                        ReceiverName = $"{s.ReceiverUser.FirstName} {s.ReceiverUser.LastName}",
                        ProjectId = s.ProjectId,
                        Title = s.Title,
                        CreatedAt = s.CreatedAt,
                        Feedback = s.Feedback,
                        Status = s.Status,
                        CreateProjectFiles = s.CreateProjectFiles.Select(f => new CreateProjectFileInformation()
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
                await Console.Out.WriteLineAsync($"GetAllCreateProjectRequestsByPMBAsync: {ex.Message}");
            }

            return data;
        }

        // admin can update transaction logs
        public async Task<bool> UpdateCreateProjectRequestAsync(CreateProjectRequest updateCreateProjectRequest)
        {
            CreateProjectRequest createProjectRequest = null!;
            bool result = false;
            try
            {
                createProjectRequest = await _createProjectRequestRepository.Get(updateCreateProjectRequest.Id);
                if (createProjectRequest == null)
                {
                    throw new Exception("CreateProjectRequest not found!");
                }
                //ConvertToBaseEntity(createProjectRequest, updateCreateProjectRequest);
                result = await _createProjectRequestRepository.UpdateAsync(updateCreateProjectRequest);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"UpdateCreateProjectRequest: {ex.Message}");
            }

            return result;
        }
        // admin can delete transaction logs
        public async Task<bool> DeleteCreateProjectRequestAsync(int id)
        {
            CreateProjectRequest createProjectRequest = null!;
            bool result = false;
            try
            {
                createProjectRequest = await _createProjectRequestRepository.Get(id);
                if (createProjectRequest == null)
                {
                    throw new Exception("CreateProjectRequest not found!");
                }
                result = await _createProjectRequestRepository.DeleteAsync(createProjectRequest);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"DeleteCreateProjectRequest: {ex.Message}");
            }

            return result;
        }
    }
}
