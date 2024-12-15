using FALOFinancialProofing.Constant;
using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace FALOFinancialProofing.Services.ApproveProcessServices
{
    public class ApproveProcessServices : IApproveProcessServices
    {

        private readonly IRepository<ApproveProcess, int> repository;
        private readonly IRepository<Campaign, int> campaignRepository;
        private readonly IRepository<CampaignMember, int> campaignMemberRepository;
        private readonly IRepository<RequestForm, int> requestFormRepository;

        public ApproveProcessServices(IRepository<ApproveProcess, int> repository,
            IRepository<Campaign, int> campaignRepository,
            IRepository<CampaignMember, int> campaignMemberRepository,
            IRepository<RequestForm, int> requestFormRepository)
        {
            this.repository = repository;
            this.campaignRepository = campaignRepository;
            this.campaignMemberRepository = campaignMemberRepository;
            this.requestFormRepository = requestFormRepository;
        }

        public async Task<ApproveProcess?> CreateApproveProcessAsync(ApproveProcessRequest dto)
        {
            try
            {
                var newApproveProcess = await DTOToEntity(dto);
                return await repository.InsertAsync(newApproveProcess);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<ApproveProcess> DTOToEntity(ApproveProcessRequest dto)
        {
            return new ApproveProcess
            {
                Id = dto.Id != null ? dto.Id.Value : 0,
                ApproveNumber = dto.ApproveNumber,
                ApproveStatus = dto.ApproveStatus,
                RequestId = dto.RequestId,
                ApproverId = dto.ApproverId,
                Vouchers = dto.Vouchers
            };
        }

        public async Task<bool> DeleteApproveProcessAsync(ApproveProcessRequest dto)
        {
            try
            {
                var deletedApproveProcess = await repository.Get(x => x.Id == dto.Id);
                if (deletedApproveProcess == null) return false;

                return await repository.DeleteAsync(deletedApproveProcess);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteApproveProcessByIdAsync(int id)
        {
            try
            {
                var deletedApproveProcess = await repository.Get(x => x.Id == id);
                if (deletedApproveProcess == null) return false;

                return await repository.DeleteAsync(deletedApproveProcess);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<ApproveProcess>> GetAllApproveProcesssAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ApproveProcess?> GetApproveProcessByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ApproveProcess>?> GetApproveProcessesByRequestIdAsync(int requestId)
        {
            try
            {
                return await repository.GetAll(ap => ap.RequestId == requestId)
                    .Include(ap => ap.RequestForm)
                    .Include(ap => ap.User)
                    .Include(ap => ap.Vouchers)
                    .Select(ap => new ApproveProcess
                    {
                        Id = ap.Id,
                        ApproveNumber = ap.ApproveNumber,
                        ApproveStatus = ap.ApproveStatus,
                        RequestId = ap.RequestForm.Id,
                        ApproverId = ap.User.Id,
                    }).ToListAsync();

            }
            catch (Exception ex)
            {
                return null;

            }
        }

        public async Task<bool> UpdateApproveProcessAsync(ApproveProcessRequest dto)
        {
            try
            {
                var updatedApproveProcess = await DTOToEntity(dto);
                return await repository.UpdateAsync(updatedApproveProcess);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> ApproveRequestForLeader(string userid, string currentLoggingRole, int requestid, StringBuilder message)
        {
            try
            {
                // validate if current logged in user is not Volunteer Leader
                if (currentLoggingRole != Resource.VolunteerLeaderRoleName)
                {
                    message.Append("Current logged in user is not Volunteer Leader");
                    return false;
                }
                // check if user have permission and process is not approved
                var approveProcess = repository.GetAll(x => x.RequestId == requestid
                    && x.ApproverId.Equals(userid)
                    && x.ApproveStatus.Equals(Resource.ProcessStatus))
                    .Include(x => x.RequestForm)
                    .FirstOrDefault();
                if (approveProcess == null)
                {
                    message.Append("User does not have permission or process is already done");
                    return false;
                }
                //check if campaign is active and not closed
                var campaign = await campaignRepository.GetAll(x => x.Id == approveProcess.RequestForm.CampaignId 
                    && !x.Status.Equals(Resource.CampaignStatus_Close)
                    && x.IsActive)
                    .Include(x => x.CampaignMembers)
                    .ThenInclude(x => x.Role)
                    .FirstOrDefaultAsync();
                if (campaign == null)
                {
                    message.Append("Campaign is not active or closed");
                    return false;
                }
                // check if user is a leader of the campaign and active
                var activeInCampaign = campaign.CampaignMembers.Any(x => x.UserId.Equals(userid)
                                   && x.Role.Name.Equals(Resource.VolunteerLeaderRoleName)
                                                      && x.IsActive);
                if (!activeInCampaign)
                {
                    message.Append("User is not a leader of the campaign or not active");
                    return false;
                }
                approveProcess.ApproveStatus = Resource.ApprovedStatus;
                var updatedComplete = await repository.UpdateAsync(approveProcess);
                if (!updatedComplete)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ApproveRequestForAccounting(string userid, string currentLoggingRole, int requestid, StringBuilder message)
        {
            try
            {
                // validate if current logged in user is not Accounting
                if (currentLoggingRole != Resource.AccountingRoleName)
                {
                    message.Append("Current logged in user is not Accounting");
                    return false;
                }
                // check if user have permission and process is not approved
                var approveProcess = repository.GetAll(x => x.RequestId == requestid
                    && x.ApproverId.Equals(userid)
                    && x.ApproveStatus.Equals(Resource.ProcessStatus))
                    .Include(x => x.RequestForm)
                    .FirstOrDefault();
                if (approveProcess == null)
                {
                    message.Append("User does not have permission or process is already done");
                    return false;
                }
                //check if campaign is active and not closed
                var campaign = await campaignRepository.GetAll(x => x.Id == approveProcess.RequestForm.CampaignId
                    && !x.Status.Equals(Resource.CampaignStatus_Close)
                    && x.IsActive)
                    .Include(x => x.CampaignMembers)
                    .ThenInclude(x => x.Role)
                    .FirstOrDefaultAsync();
                if (campaign == null)
                {
                    message.Append("Campaign is not active or closed");
                    return false;
                }
                // check if user is a leader of the campaign and active
                var activeInCampaign = campaign.CampaignMembers.Any(x => x.UserId.Equals(userid)
                                   && x.Role.Name.Equals(Resource.AccountingRoleName)
                                                      && x.IsActive);
                if (!activeInCampaign)
                {
                    message.Append("User is not a accountancy of the campaign or not active");
                    return false;
                }
                approveProcess.ApproveStatus = Resource.ApprovedStatus;
                var updatedComplete = await repository.UpdateAsync(approveProcess);
                if (!updatedComplete)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ApproveRequestForProjectManager(string userid, string currentLoggingRole, int requestid, StringBuilder message)
        {
            try
            {
                // validate if current logged in user is not PM
                if (currentLoggingRole != Resource.ProjectManagerRoleName)
                {
                    message.Append("Current logged in user is not Project Manager");
                    return false;
                }
                // check if user have permission and process is not approved
                var approveProcess = repository.GetAll(x => x.RequestId == requestid
                    && x.ApproverId.Equals(userid)
                    && x.ApproveStatus.Equals(Resource.ProcessStatus))
                    .Include(x => x.RequestForm)
                    .FirstOrDefault();
                if (approveProcess == null)
                {
                    message.Append("User does not have permission or process is already done");
                    return false;
                }
                //check if campaign is active and not closed
                var campaign = await campaignRepository.GetAll(x => x.Id == approveProcess.RequestForm.CampaignId
                    && !x.Status.Equals(Resource.CampaignStatus_Close)
                    && x.IsActive)
                    .Include(x => x.CampaignMembers)
                     .ThenInclude(x => x.Role)
                    .FirstOrDefaultAsync();
                if (campaign == null)
                {
                    message.Append("Campaign is not active or closed");
                    return false;
                }
                approveProcess.ApproveStatus = Resource.ApprovedStatus;
                var updatedComplete = await repository.UpdateAsync(approveProcess);
                if (!updatedComplete)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> RejectPrePayRequestForLeader(string userid, string currentLoggingRole, int requestid, StringBuilder msg, string feedback, StringBuilder feedbackStringBuilder)
        {
            try
            {
                // vaidate if feedback is empty
                if (String.IsNullOrEmpty(feedback))
                {
                    msg.Append("Feedback must not empty");
                    return false;
                }
                feedbackStringBuilder.Append(Resource.VolunteerLeaderFeedBackStart + " " + feedback);
                // validate if current logged in user is not Volunteer Leader
                if (currentLoggingRole != Resource.VolunteerLeaderRoleName)
                {
                    msg.Append("Current logged in user is not Volunteer Leader");
                    return false;
                }
                // check if user have permission and process is not approved
                var approveProcess = repository.GetAll(x => x.RequestId == requestid
                    && x.ApproverId.Equals(userid)
                    && x.ApproveStatus.Equals(Resource.ProcessStatus))
                    .Include(x => x.RequestForm)
                    .FirstOrDefault();
                if (approveProcess == null)
                {
                    msg.Append("User does not have permission or process is already done");
                    return false;
                }
                //check if campaign is active and not closed
                var campaign = await campaignRepository.GetAll(x => x.Id == approveProcess.RequestForm.CampaignId
                    && !x.Status.Equals(Resource.CampaignStatus_Close)
                    && x.IsActive)
                    .Include(x => x.CampaignMembers)
                    .ThenInclude(x => x.Role)
                    .FirstOrDefaultAsync();
                if (campaign == null)
                {
                    msg.Append("Campaign is not active or closed");
                    return false;
                }
                // check if user is a leader of the campaign and active
                var activeInCampaign = campaign.CampaignMembers.Any(x => x.UserId.Equals(userid)
                                   && x.Role.Name.Equals(Resource.VolunteerLeaderRoleName)
                                                      && x.IsActive);
                if (!activeInCampaign)
                {
                    msg.Append("User is not a leader of the campaign or not active");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> RejectPrePayRequestForAccounting(string userid, string currentLoggingRole, int requestid, StringBuilder msg, string feedback, StringBuilder feedbackStringBuilder)
        {
            try
            {
                // vaidate if feedback is empty
                if (String.IsNullOrEmpty(feedback))
                {
                    msg.Append("Feedback must not empty");
                    return false;
                }
                feedbackStringBuilder.Append(Resource.AccountingFeedBackStart + " " + feedback);
                // validate if current logged in user is not Accounting
                if (currentLoggingRole != Resource.AccountingRoleName)
                {
                    msg.Append("Current logged in user is not Accounting");
                    return false;
                }
                // check if user have permission and process is not approved
                var approveProcess = repository.GetAll(x => x.RequestId == requestid
                    && x.ApproverId.Equals(userid)
                    && x.ApproveStatus.Equals(Resource.ProcessStatus))
                    .Include(x => x.RequestForm)
                    .FirstOrDefault();
                if (approveProcess == null)
                {
                    msg.Append("User does not have permission or process is already done");
                    return false;
                }
                //check if campaign is active and not closed
                var campaign = await campaignRepository.GetAll(x => x.Id == approveProcess.RequestForm.CampaignId
                    && !x.Status.Equals(Resource.CampaignStatus_Close)
                    && x.IsActive)
                    .Include(x => x.CampaignMembers)
                    .ThenInclude(x => x.Role)
                    .FirstOrDefaultAsync();
                if (campaign == null)
                {
                    msg.Append("Campaign is not active or closed");
                    return false;
                }
                // check if user is a leader of the campaign and active
                var activeInCampaign = campaign.CampaignMembers.Any(x => x.UserId.Equals(userid)
                                   && x.Role.Name.Equals(Resource.AccountingRoleName)
                                                      && x.IsActive);
                if (!activeInCampaign)
                {
                    msg.Append("User is not a accountancy of the campaign or not active");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> RejectPrePayRequestForProjectManager(string userid, string currentLoggingRole, int requestid, StringBuilder msg, string feedback, StringBuilder feedbackStringBuilder)
        {
            try
            {
                // vaidate if feedback is empty
                if (String.IsNullOrEmpty(feedback))
                {
                    msg.Append("Feedback must not empty");
                    return false;
                }
                feedbackStringBuilder.Append(Resource.ProjectManagerFeedBackStart + " " + feedback);
                // validate if current logged in user is not Project Manager
                if (currentLoggingRole != Resource.ProjectManagerRoleName)
                {
                    msg.AppendLine("Current logged in user is not Project Manager");
                    return false;
                }
                // check if user have permission and process is not approved
                var approveProcess = repository.GetAll(x => x.RequestId == requestid
                    && x.ApproverId.Equals(userid)
                    && x.ApproveStatus.Equals(Resource.ProcessStatus))
                    .Include(x => x.RequestForm)
                    .FirstOrDefault();
                if (approveProcess == null)
                {
                    msg.AppendLine("User does not have permission or process is already done");
                    return false;
                }
                //check if campaign is active and not closed
                var campaign = await campaignRepository.GetAll(x => x.Id == approveProcess.RequestForm.CampaignId
                    && !x.Status.Equals(Resource.CampaignStatus_Close)
                    && x.IsActive)
                    .Include(x => x.CampaignMembers)
                    .ThenInclude(x => x.Role)
                    .FirstOrDefaultAsync();
                if (campaign == null)
                {
                    msg.Append("Campaign is not active or closed");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<ApproveProcess?> GetApproveProcessesByRequestIdAndApproveIdAsync(int requestid, string userid)
        {
            try
            {
                var approveProcess = await repository.GetAll(x => x.RequestId == requestid && x.ApproverId.Equals(userid))
                    .FirstOrDefaultAsync();
                return approveProcess;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<PrePayRequestFormViewRequest>?> GetAllPrepayRequestForVolunteerLeader(string userid, string currentRoleLoggedIn)
        {
            var requests = new List<PrePayRequestFormViewRequest>();
            try
            {
                // check if current logged in user is not Volunteer Leader
                if (currentRoleLoggedIn != Resource.VolunteerLeaderRoleName)
                {
                    return new List<PrePayRequestFormViewRequest>();
                }
                // get all the campaign that userid is a Volunteer Leader
                var campaigns = await campaignMemberRepository.GetAll(cm => cm.UserId == userid
                        && cm.Role.Name.Equals(Resource.VolunteerLeaderRoleName)
                        && cm.IsActive)
                    .Include(cm => cm.Campaign)
                    .Include(cm => cm.Role)
                    .Select(cm => new Campaign
                    {
                        Id = cm.Campaign.Id,
                    }).ToListAsync();

                foreach (var campaign in campaigns)
                {
                    var requestForms = await requestFormRepository.GetAll(x => x.CampaignId == campaign.Id
                    && x.TypeId == IntConstant.PrePayRequestType
                    && x.ApproveProcesses.Any(x => x.ApproverId.Equals(userid)))
                        .Include(x => x.AttachmentFiles)
                        .Include(x => x.ApproveProcesses)
                        .ThenInclude(x => x.Vouchers)
                        .Include(x => x.User)
                        .Select(rf => new PrePayRequestFormViewRequest
                        {
                            Id = rf.Id,
                            CreateAt = rf.CreateAt,
                            Description = rf.Description,
                            ExpectedMoney = rf.ExpectedMoney,
                            Status = rf.Status,
                            CreatedBy = rf.CreatedBy,
                            CreateByName = rf.User.FirstName + " " + rf.User.LastName,
                            CreateByEmail = rf.User.Email,
                            CampaignId = rf.CampaignId,
                            AttachmentFiles = rf.AttachmentFiles.Select(af => new AttachmentFileRequest
                            {
                                FilePath = af.FilePath,
                                RequestId = af.RequestId
                            }).ToList(),
                            Vouchers = rf.ApproveProcesses.Select(ap => new VoucherRequest
                            {
                                FilePath = ap.Vouchers.FirstOrDefault(vc => vc.Id != null).FilePath,
                            }).Where(cv => cv.FilePath != null).ToList(),
                            ApproveProcessStatus = (rf.ApproveProcesses.FirstOrDefault(x => x.ApproverId.Equals(userid)) == null ? "null" : rf.ApproveProcesses.FirstOrDefault(x => x.ApproverId.Equals(userid)).ApproveStatus)
                        }).ToListAsync();
                    if (requestForms != null && requestForms.Count > 0)
                    {
                        requests.AddRange(requestForms);
                    }
                }
                return requests;
            }
            catch (Exception ex)
            {
                return requests;
            }
        }

        public async Task<List<PrePayRequestFormViewRequest>?> GetAllPaymentRequestForVolunteerLeader(string userid, string currentRoleLoggedIn)
        {
            var requests = new List<PrePayRequestFormViewRequest>();
            try
            {
                // check if current logged in user is not Volunteer Leader
                if (currentRoleLoggedIn != Resource.VolunteerLeaderRoleName)
                {
                    return new List<PrePayRequestFormViewRequest>();
                }
                // get all the campaign that userid is a Volunteer Leader
                var campaigns = await campaignMemberRepository.GetAll(cm => cm.UserId == userid
                        && cm.Role.Name.Equals(Resource.VolunteerLeaderRoleName)
                        && cm.IsActive)
                    .Include(cm => cm.Campaign)
                    .Include(cm => cm.Role)
                    .Select(cm => new Campaign
                    {
                        Id = cm.Campaign.Id,
                    }).ToListAsync();

                foreach (var campaign in campaigns)
                {
                    var requestForms = await requestFormRepository.GetAll(x => x.CampaignId == campaign.Id
                        && x.TypeId == IntConstant.PaymentRequestType
                        && x.ApproveProcesses.Any(x => x.ApproverId.Equals(userid)))
                        .Include(x => x.AttachmentFiles)
                        .Include(x => x.ApproveProcesses)
                        .ThenInclude(x => x.Vouchers)
                        .Include(x => x.User)
                        .Select(rf => new PrePayRequestFormViewRequest
                        {
                            Id = rf.Id,
                            CreateAt = rf.CreateAt,
                            Description = rf.Description,
                            ExpectedMoney = rf.ExpectedMoney,
                            Status = rf.Status,
                            CreatedBy = rf.CreatedBy,
                            CreateByName = rf.User.FirstName + " " + rf.User.LastName,
                            CreateByEmail = rf.User.Email,
                            CampaignId = rf.CampaignId,
                            AttachmentFiles = rf.AttachmentFiles.Select(af => new AttachmentFileRequest
                            {
                                FilePath = af.FilePath,
                                RequestId = af.RequestId
                            }).ToList(),
                            Vouchers = rf.ApproveProcesses.Select(ap => new VoucherRequest
                            {
                                FilePath = ap.Vouchers.FirstOrDefault(vc => vc.Id != null).FilePath,
                            }).Where(cv => cv.FilePath != null).ToList(),
                            ApproveProcessStatus = (rf.ApproveProcesses.FirstOrDefault(x => x.ApproverId.Equals(userid)) == null ? "null" : rf.ApproveProcesses.FirstOrDefault(x => x.ApproverId.Equals(userid)).ApproveStatus)
                        }).ToListAsync();
                    if (requestForms != null && requestForms.Count > 0)
                    {
                        requests.AddRange(requestForms);
                    }
                }
                return requests;
            }
            catch (Exception ex)
            {
                return requests;
            }
        }

        public async Task<List<PrePayRequestFormViewRequest>?> GetAllPrepayRequestForAccounting(string userid, string currentLoggingRole)
        {
            var requests = new List<PrePayRequestFormViewRequest>();
            try
            {
                // check if current logged in user is not Accounting
                if (currentLoggingRole != Resource.AccountingRoleName)
                {
                    return new List<PrePayRequestFormViewRequest>();
                }
                // get all the campaign that userid is a Accounting
                var campaigns = await campaignMemberRepository.GetAll(cm => cm.UserId == userid
                        && cm.Role.Name.Equals(Resource.AccountingRoleName)
                        && cm.IsActive)
                    .Include(cm => cm.Campaign)
                    .Include(cm => cm.Role)
                    .Select(cm => new Campaign
                    {
                        Id = cm.Campaign.Id,
                    }).ToListAsync();

                foreach (var campaign in campaigns)
                {
                    var requestForms = await requestFormRepository.GetAll(x => x.CampaignId == campaign.Id
                    && x.TypeId == IntConstant.PrePayRequestType
                    && x.ApproveProcesses.Any(x => x.ApproverId.Equals(userid)))
                        .Include(x => x.AttachmentFiles)
                        .Include(x => x.ApproveProcesses)
                        .ThenInclude(x => x.Vouchers)
                        .Include(x => x.User)
                        .Select(rf => new PrePayRequestFormViewRequest
                        {
                            Id = rf.Id,
                            CreateAt = rf.CreateAt,
                            Description = rf.Description,
                            ExpectedMoney = rf.ExpectedMoney,
                            Status = rf.Status,
                            CreatedBy = rf.CreatedBy,
                            CreateByName = rf.User.FirstName + " " + rf.User.LastName,
                            CreateByEmail = rf.User.Email,
                            CampaignId = rf.CampaignId,
                            AttachmentFiles = rf.AttachmentFiles.Select(af => new AttachmentFileRequest
                            {
                                FilePath = af.FilePath,
                                RequestId = af.RequestId
                            }).ToList(),
                            Vouchers = rf.ApproveProcesses.Select(ap => new VoucherRequest
                            {
                                FilePath = ap.Vouchers.FirstOrDefault(vc => vc.Id != null).FilePath,
                            }).Where(cv => cv.FilePath != null).ToList(),
                            ApproveProcessStatus = (rf.ApproveProcesses.FirstOrDefault(x => x.ApproverId.Equals(userid)) == null ? "null" : rf.ApproveProcesses.FirstOrDefault(x => x.ApproverId.Equals(userid)).ApproveStatus)
                        }).ToListAsync();
                    if (requestForms != null && requestForms.Count > 0)
                    {
                        requests.AddRange(requestForms);
                    }
                }
                return requests;
            }
            catch (Exception ex)
            {
                return requests;
            }
        }
        public async Task<List<PrePayRequestFormViewRequest>?> GetAllPaymentRequestForAccounting(string userid, string currentLoggingRole)
        {
            var requests = new List<PrePayRequestFormViewRequest>();
            try
            {
                // check if current logged in user is not Accounting
                if (currentLoggingRole != Resource.AccountingRoleName)
                {
                    return new List<PrePayRequestFormViewRequest>();
                }
                // get all the campaign that userid is a Accounting
                var campaigns = await campaignMemberRepository.GetAll(cm => cm.UserId == userid
                        && cm.Role.Name.Equals(Resource.AccountingRoleName)
                        && cm.IsActive)
                    .Include(cm => cm.Campaign)
                    .Include(cm => cm.Role)
                    .Select(cm => new Campaign
                    {
                        Id = cm.Campaign.Id,
                    }).ToListAsync();

                foreach (var campaign in campaigns)
                {
                    var requestForms = await requestFormRepository.GetAll(x => x.CampaignId == campaign.Id
                    && x.TypeId == IntConstant.PaymentRequestType
                    && x.ApproveProcesses.Any(x => x.ApproverId.Equals(userid)))
                        .Include(x => x.AttachmentFiles)
                        .Include(x => x.ApproveProcesses)
                        .ThenInclude(x => x.Vouchers)
                        .Include(x => x.User)
                        .Select(rf => new PrePayRequestFormViewRequest
                        {
                            Id = rf.Id,
                            CreateAt = rf.CreateAt,
                            Description = rf.Description,
                            ExpectedMoney = rf.ExpectedMoney,
                            Status = rf.Status,
                            CreatedBy = rf.CreatedBy,
                            CreateByName = rf.User.FirstName + " " + rf.User.LastName,
                            CreateByEmail = rf.User.Email,
                            CampaignId = rf.CampaignId,
                            AttachmentFiles = rf.AttachmentFiles.Select(af => new AttachmentFileRequest
                            {
                                FilePath = af.FilePath,
                                RequestId = af.RequestId
                            }).ToList(),
                            Vouchers = rf.ApproveProcesses.Select(ap => new VoucherRequest
                            {
                                FilePath = ap.Vouchers.FirstOrDefault(vc => vc.Id != null).FilePath,
                            }).Where(cv => cv.FilePath != null).ToList(),
                            ApproveProcessStatus = (rf.ApproveProcesses.FirstOrDefault(x => x.ApproverId.Equals(userid)) == null ? "null" : rf.ApproveProcesses.FirstOrDefault(x => x.ApproverId.Equals(userid)).ApproveStatus)
                        }).ToListAsync();
                    if (requestForms != null && requestForms.Count > 0)
                    {
                        requests.AddRange(requestForms);
                    }
                }
                return requests;
            }
            catch (Exception ex)
            {
                return requests;
            }
        }

        public async Task<List<RequestFormViewRequestWithVoucherForPM>?> GetAllPrepayRequestForProjectManager(string userid, string currentLoggingRole)
        {
            var requests = new List<RequestFormViewRequestWithVoucherForPM>();
            try
            {
                // check if current logged in user is not PM
                if (currentLoggingRole != Resource.ProjectManagerRoleName)
                {
                    return new List<RequestFormViewRequestWithVoucherForPM>();
                }
                // get all the campaign that userid is a PM
                var campaigns = await campaignMemberRepository.GetAll(cm => cm.UserId == userid
                        && cm.Role.Name.Equals(Resource.ProjectManagerRoleName)
                        && cm.IsActive)
                    .Include(cm => cm.Campaign)
                    .Include(cm => cm.Role)
                    .Select(cm => new Campaign
                    {
                        Id = cm.Campaign.Id,
                    }).ToListAsync();

                foreach (var campaign in campaigns)
                {
                    var requestForms = await requestFormRepository.GetAll(x => x.CampaignId == campaign.Id
                    && x.TypeId == IntConstant.PrePayRequestType
                    && x.ApproveProcesses.Any(x => x.ApproverId.Equals(userid)))
                        .Include(x => x.User)
                        .Include(x => x.AttachmentFiles)
                        .Include(x => x.ApproveProcesses)
                        .ThenInclude(x => x.Vouchers)
                        .Select(rf => new RequestFormViewRequestWithVoucherForPM
                        {
                            Id = rf.Id,
                            CreateAt = rf.CreateAt,
                            Description = rf.Description,
                            ExpectedMoney = rf.ExpectedMoney,
                            Status = rf.Status,
                            CreatedBy = rf.CreatedBy,
                            CreateByName = rf.User.FirstName + " " + rf.User.LastName,
                            CreateByEmail = rf.User.Email,
                            CampaignId = rf.CampaignId,
                            AttachmentFiles = rf.AttachmentFiles.Select(af => new AttachmentFileRequest
                            {
                                FilePath = af.FilePath,
                                RequestId = af.RequestId
                            }).ToList(),
                            VoucherFiles = rf.ApproveProcesses.SelectMany(ap => ap.Vouchers).ToList(),
                            ApproveProcessStatus = (rf.ApproveProcesses.FirstOrDefault(x => x.ApproverId.Equals(userid)) == null ? "null" : rf.ApproveProcesses.FirstOrDefault(x => x.ApproverId.Equals(userid)).ApproveStatus)
                        }).ToListAsync();
                    if (requestForms != null && requestForms.Count > 0)
                    {
                        requests.AddRange(requestForms);
                    }
                }
                return requests;
            }
            catch (Exception ex)
            {
                return requests;
            }
        }
        public async Task<List<RequestFormViewRequestWithVoucherForPM>?> GetAllPaymentRequestForProjectManager(string userid, string currentLoggingRole)
        {
            var requests = new List<RequestFormViewRequestWithVoucherForPM>();
            try
            {
                // check if current logged in user is not PM
                if (currentLoggingRole != Resource.ProjectManagerRoleName)
                {
                    return new List<RequestFormViewRequestWithVoucherForPM>();
                }
                // get all the campaign that userid is a PM
                var campaigns = await campaignMemberRepository.GetAll(cm => cm.UserId == userid
                        && cm.Role.Name.Equals(Resource.ProjectManagerRoleName)
                        && cm.IsActive)
                    .Include(cm => cm.Campaign)
                    .Include(cm => cm.Role)
                    .Select(cm => new Campaign
                    {
                        Id = cm.Campaign.Id,
                    }).ToListAsync();

                foreach (var campaign in campaigns)
                {
                    var requestForms = await requestFormRepository.GetAll(x => x.CampaignId == campaign.Id
                    && x.TypeId == IntConstant.PaymentRequestType
                    && x.ApproveProcesses.Any(x => x.ApproverId.Equals(userid)))
                        .Include(x => x.User)
                        .Include(x => x.AttachmentFiles)
                        .Include(x => x.ApproveProcesses)
                        .ThenInclude(x => x.Vouchers)
                        .Select(rf => new RequestFormViewRequestWithVoucherForPM
                        {
                            Id = rf.Id,
                            CreateAt = rf.CreateAt,
                            Description = rf.Description,
                            ExpectedMoney = rf.ExpectedMoney,
                            Status = rf.Status,
                            CreatedBy = rf.CreatedBy,
                            CreateByName = rf.User.FirstName + " " + rf.User.LastName,
                            CreateByEmail = rf.User.Email,
                            CampaignId = rf.CampaignId,
                            AttachmentFiles = rf.AttachmentFiles.Select(af => new AttachmentFileRequest
                            {
                                FilePath = af.FilePath,
                                RequestId = af.RequestId
                            }).ToList(),
                            VoucherFiles = rf.ApproveProcesses.SelectMany(ap => ap.Vouchers).ToList(),
                            ApproveProcessStatus = (rf.ApproveProcesses.FirstOrDefault(x => x.ApproverId.Equals(userid)) == null ? "null" : rf.ApproveProcesses.FirstOrDefault(x => x.ApproverId.Equals(userid)).ApproveStatus)
                        }).ToListAsync();
                    if (requestForms != null && requestForms.Count > 0)
                    {
                        requests.AddRange(requestForms);
                    }
                }
                return requests;
            }
            catch (Exception ex)
            {
                return requests;
            }
        }


    }
}
