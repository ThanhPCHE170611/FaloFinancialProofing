using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace FALOFinancialProofing.Services.AccountingBookServices
{
    public class AccountingBookServices : IAcccountingBookServices
    {
        private readonly IRepository<AccountingBook, int> repository;
        private readonly IRepository<Campaign, int> campaignRepository;
        private readonly IRepository<CampaignMember, int> campaignMemberRepository;

        public AccountingBookServices(IRepository<AccountingBook, int> repository, IRepository<Campaign, int> campaignRepository,
            IRepository<CampaignMember, int> campaignMemberRepository)
        {
            this.repository = repository;
            this.campaignRepository = campaignRepository;
            this.campaignMemberRepository = campaignMemberRepository;
        }

        public async Task<AccountingBook?> CreateAccountingRequest(CreateAccountingBookRequest request, StringBuilder msg)
        {
            try
            {
                var isRequestValidated = await ValidateRequest(request, msg);
                var campaignIdInt = TryToParseInt(request.CampaignId);
                if (!isRequestValidated)
                {
                    return null;
                }

                var campaignWithAccountingBook = await campaignRepository.GetAll(c => c.Id == campaignIdInt && c.IsActive)
                    .Include(c => c.AccountingBook).FirstOrDefaultAsync();
                // check if campaign already have accounting book => update // create new
                // save new accounting book
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), Resource.AccountingBookFolderName);

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                var uniqueFileName = $"{Guid.NewGuid()}_{request.File.FileName}";

                var filePath = Path.Combine(uploadFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.File.CopyToAsync(stream);
                }
                if (campaignWithAccountingBook.AccountingBook != null)
                {
                    var updateAccountingBook = new AccountingBook
                    {
                        Id = campaignWithAccountingBook.AccountingBook.Id,
                        CampaignId = campaignWithAccountingBook.AccountingBook.CampaignId,
                        FilePath = uniqueFileName,
                    }; 
                    var canUpdateAccounting = await repository.UpdateAsync(updateAccountingBook);
                    if(canUpdateAccounting)
                    {
                        return updateAccountingBook;
                    }
                } 
                else
                {
                    var newAccountingBook = new AccountingBook
                    {
                        CampaignId = campaignWithAccountingBook.Id,
                        FilePath = uniqueFileName,
                    };
                    var canCreateAccounting = await repository.InsertAsync(newAccountingBook);
                    if (canCreateAccounting != null)
                    {
                        return newAccountingBook;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<bool> ValidateRequest(CreateAccountingBookRequest request, StringBuilder msg)
        {
            var campaignIdInt = TryToParseInt(request.CampaignId);
            if(request == null)
            {
                msg.Append("Request is null");
                return false;
            }

            if(campaignIdInt == 0)
            {
                msg.Append("CampaignId is invalid");
                return false;
            }
            
            // check if UserId and role is valid with the campaign ID
            var accountanceValid = await campaignMemberRepository.GetAll(x => x.IsActive 
                                                && x.UserId.Equals(request.UserId) 
                                                && x.Role.Name.Equals(Resource.AccountingRoleName)
                                                && x.CampaignId == campaignIdInt)
                .Include(x => x.Role)
                .FirstOrDefaultAsync();
            if(accountanceValid == null)
            {
                msg.Append("UserID, CurrentRole is not valid or in wrong campaign");
                return false;
            }

            return true;
        }

        private int TryToParseInt(string campaignId)
        {
            try
            {
                return Convert.ToInt32(campaignId);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<AccountingBook?> GetAccountingBookInCampaign(int campaignId)
        {
            try
            {
                var accountingBook = await repository.GetAll(x => x.CampaignId == campaignId)
                    .FirstOrDefaultAsync();
                return accountingBook;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<(byte[] fileBytes, string contentType, string downloadFileName)> DownloadAccountingBookFileByFileName(string fileName)
        {
            try
            {
                // Đường dẫn đầy đủ đến file trong server
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), Resource.AccountingBookFolderName, fileName);

                // Kiểm tra xem file có tồn tại không
                if (!System.IO.File.Exists(filePath))
                {
                    return (null, null, null);
                }

                // Đọc file vào stream
                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

                // Kiểm tra định dạng file và đặt Content-Type tương ứng
                string contentType;
                if (fileName.EndsWith(".rar"))
                {
                    contentType = "application/x-rar-compressed";  // MIME type cho file .rar
                }
                else if (fileName.EndsWith(".zip"))
                {
                    contentType = "application/zip";  // MIME type cho file .zip
                }
                else
                {
                    contentType = "application/octet-stream";  // Định dạng mặc định
                }

                // Trả về thông tin file
                return (fileBytes, contentType, fileName);
            }
            catch (Exception ex)
            {
                return (null, null, null);
            }
        }

        public async Task<List<AccountingBook>> GetAllAccountingBookInProject(int projectId)
        {
            var accountingBooks = new List<AccountingBook>();
            try
            {
                accountingBooks = await repository.GetAll(x => x.Campaign.ProjectId == projectId)
                    .Include(x => x.Campaign)
                    .ToListAsync();
                return accountingBooks;
            }
            catch (Exception ex)
            {
                return accountingBooks;
            }
        }

        public async Task<bool> DeleteAccountingBook(string fileName, StringBuilder message)
        {
            try
            {
                // Đường dẫn đầy đủ đến file trong server
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), Resource.AccountingBookFolderName, fileName);

                // Kiểm tra xem file có tồn tại không
                if (!System.IO.File.Exists(filePath))
                {
                    message.Append("Cannot find Accounting Book file in storage, please try again later!");
                    return false;
                }
                var accountingBook = await repository.Get(x => x.FilePath.Equals(fileName));
                if (accountingBook == null)
                {
                    message.Append("Cannot find Accounting Book file in database, please try again later!");
                    return false;
                }
                // Xóa file
                System.IO.File.Delete(filePath);
                var canDelete = await repository.DeleteAsync(accountingBook.Id);

                return true;
            }
            catch (Exception ex)
            {
                message.Append("Cannot delete Accounting Book file, please try again later!");
                return false;
            }

        }
    }
}
