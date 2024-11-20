using FALOFinancialProofing.Attributes.RoleAttributes;
using FALOFinancialProofing.Constant;
using FALOFinancialProofing.DTOs.CampaignDTO;
using FALOFinancialProofing.DTOs.OrganizationDTO;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.OrganizationMemberServices;
using FALOFinancialProofing.Services.OrganizationServices;
using FALOFinancialProofing.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;
        private readonly IOrganizationMemberService _organizationMemberService;

        public OrganizationsController(IOrganizationService organizationService, IOrganizationMemberService organizationMemberService)
        {
            _organizationService = organizationService;
            _organizationMemberService = organizationMemberService;
        }

        // GET: api/Organizations
        [HttpGet("GetAllOrganizationInSystem")]
        public async Task<IActionResult> GetAllOrganizationInSystem(string? OrganizationName, int currentPage = IntConstant.PageNumberDefault)
        {
            List<OrganizationInformation> data = null;
            FilterPagingData filterPagingData = new FilterPagingData();
            try
            {
                data = (await _organizationService.GetAllOrganizationsAsync(Request)).ToList();
                if (data == null || data.Count == 0)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = "Get All Organizations In System Failed!",
                        Data = data
                    });
                }
                if (!string.IsNullOrEmpty(OrganizationName))
                {
                    OrganizationName = OrganizationName.Trim();
                    data = data.FindAll(x => x.Name.Contains(OrganizationName, StringComparison.OrdinalIgnoreCase));
                }
                filterPagingData.DataCount = data.Count;
                filterPagingData.CurrentPage = currentPage;
                data = PaginationHelper.Paginate<OrganizationInformation>(data.AsQueryable(), currentPage, IntConstant.PageSizeCustom).ToList();
                filterPagingData.Data = data;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllOrganizationInSystem: {ex.Message}");
            }
            return Ok(new ApiResponse()
            {
                Success = true,
                Message = "Get All Organizations In System Successfully!",
                Data = filterPagingData
            });
        }

        [HttpGet("GetOrganizationById/{id}")]
        public async Task<IActionResult> GetOrganizationDetailsById(int id)
        {
            var organization = await _organizationService.GetOrganizationsByIdAsync(id, Request);
            if (organization == null)
            {
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = $"Organization with Id = {id} not found."
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Organization retrieved successfully.",
                Data = organization
            });
        }


        [HttpGet("GetOrganizationsByUserId/{userId}")]
        public async Task<IActionResult> GetOrganizationsByUserId(string userId, string? OrganizationName, int currentPage = IntConstant.PageNumberDefault)
        {
            List<OrganizationInformation> data = null;
            FilterPagingData filterPagingData = new FilterPagingData();
            try
            {
                data = (await _organizationService.GetAllOrganizationsByUserIdAsync(userId, Request)).ToList();
                if (data == null || data.Count == 0)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = "Get All Organizations In System Failed!",
                        Data = data
                    });
                }
                if (!string.IsNullOrEmpty(OrganizationName))
                {
                    OrganizationName = OrganizationName.Trim();
                    data = data.FindAll(x => x.Name.Contains(OrganizationName, StringComparison.OrdinalIgnoreCase));
                }
                filterPagingData.DataCount = data.Count;
                filterPagingData.CurrentPage = currentPage;
                data = PaginationHelper.Paginate<OrganizationInformation>(data.AsQueryable(), currentPage, IntConstant.PageSizeCustom).ToList();
                filterPagingData.Data = data;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllOrganizationInSystem: {ex.Message}");
            }
            return Ok(new ApiResponse()
            {
                Success = true,
                Message = "Get All Organizations In System Successfully!",
                Data = filterPagingData
            });
        }
        // PUT: api/Organizations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [RoleAttribute(AppRole.ProjectManagementBoard, AppRole.ProjectManager, AppRole.Admin)]
        [HttpPut("UpdateOrganization")]
        public async Task<IActionResult> PutOrganization([FromForm] UpdateOrganization updateOrganization)
        {
            StringBuilder message = new StringBuilder();
            bool checkValid = false;
            try
            {
                checkValid = await _organizationService.ValidateOrganizationUpdateAsync(updateOrganization, message);
                if (!checkValid)
                {
                    return Ok(new ApiResponse()
                    {
                        Message = message.ToString(),
                        Success = checkValid
                    });
                }
                checkValid = await _organizationService.UpdateOrganizationAsync(updateOrganization, message);
                if (checkValid)
                    message.Append("Organization updated successfully!");
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"UpdateOrganization: Error {ex.Message}");
            }

            return Ok(new ApiResponse()
            {
                Message = message.ToString(),
                Success = checkValid
            });
        }

        // POST: api/Organizations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [RoleAttribute(AppRole.ProjectManagementBoard, AppRole.ProjectManager, AppRole.Admin)]
        [HttpPost("CreateOrganization", Name = "CreateOrganization")]
        public async Task<ActionResult> PostOrganization([FromForm] CreateOrganization createOrganization)
        {
            StringBuilder statusMessage = new StringBuilder();
            Organization createOrganizationResult = null!;
            bool checkValid = false;
            try
            {
                checkValid = await _organizationService.ValidateCreateOrganizationAsync(createOrganization, statusMessage);
                if (!checkValid)
                {
                    return Ok(new ApiResponse
                    {
                        Success = checkValid,
                        Message = statusMessage.ToString()
                    });
                }
                createOrganizationResult = await _organizationService.CreateOrganizationAsync(createOrganization, statusMessage);
                if (createOrganizationResult == null)
                {
                    checkValid = false;
                    return Ok(new ApiResponse
                    {
                        Success = checkValid,
                        Message = statusMessage.ToString()
                    });
                }
                await _organizationMemberService.CreateOrganizationMemberAsync(new OrganizationMember
                {
                    UserId = createOrganization.UserId,
                    OrganizationId = createOrganizationResult.Id,
                });
                checkValid = true;
            }
            catch (Exception ex)
            {
                statusMessage.Append(ex.Message);
                await Console.Out.WriteLineAsync($"PostOrganization: {ex.Message}");
            }

            return Ok(new ApiResponse
            {
                Success = checkValid,
                Message = statusMessage.ToString()
            });
        }

        // DELETE: api/Organizations/5
        [HttpDelete("DeleteOrganization/{id}")]
        public async Task<IActionResult> DeleteOrganization(int id)
        {
            var statusMessage = "";
            try
            {
                statusMessage = await _organizationService.DeleteOrganizationAsync(id)
                    != false ? "DELETE Organization Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "DELETE Organization Failed!";
                await Console.Out.WriteLineAsync("PostOrganization: Error");
            }

            return Content(statusMessage);
        }
    }
}
