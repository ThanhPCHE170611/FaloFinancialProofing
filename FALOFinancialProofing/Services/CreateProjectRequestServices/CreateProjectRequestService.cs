using FALOFinancialProofing.DTOs.ProjectDTOs;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

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
                await Console.Out.WriteLineAsync($"CreateProjectRequest: {ex.Message}!");
            }

            return createCreateProjectRequest;
        }

        public async Task<CreateProjectRequest> GetCreateProjectRequestByIdAsync(int id)
        {
            CreateProjectRequest createProjectRequest = null!;
            try
            {
                createProjectRequest = await _createProjectRequestRepository.Get(id);
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

        public async Task<IEnumerable<CreateProjectRequest>> GetAllCreateProjectRequestsByPMBAsync(string userId, StringBuilder message)
        {
            List<CreateProjectRequest> data = null!;
            try
            {
                // check user whether is PMB or not

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                var isInRole = await _userManager.IsInRoleAsync(user, AppRole.ProjectManagementBoard);
                if (!isInRole)
                {
                    throw new Exception("User Role is not permitted");
                }
                //var user = 
                data = await _createProjectRequestRepository.GetAll().Where(pr => pr.Status.Equals(ProjectRequestStatus.Pending)).ToListAsync();
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"GetAllCreateProjectRequests: {ex.Message}");
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
