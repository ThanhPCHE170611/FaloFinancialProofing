using FALOFinancialProofing.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FALOFinancialProofing.DTOs.CreateCampaignFileDTO;

namespace FALOFinancialProofing.DTOs.CampaignDTO
{
    public class CampaignInformation
    {
        public int CampaignId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string CreateBy { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime DateOfCreation { get; set; }
        public double FundTarget { get; set; }
        public double TotalMoneyEarned { get; set; }
        public string? Image { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
        public string? Status { get; set; }
        public int? BankId { get; set; }
        public string? AccountNumber { get; set; }
        public string? UpdateLog { get; set; }
        public ICollection<CreateCampaignFileInformation> CreateCampaignFiles { get; set; }
    }
}
