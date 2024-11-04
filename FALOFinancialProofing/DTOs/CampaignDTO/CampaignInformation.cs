using FALOFinancialProofing.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.DTOs.CampaignDTO
{
    public class CampaignInformation
    {
        public int CampaignId { get; set; }
        public int ProjectId { get; set; }
        public string CreateBy { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime DateOfCreation { get; set; }
        public double FundTarget { get; set; }
        public double TotalMoneyEarned { get; set; }
        public string? Image { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
        public string? BankingNumber { get; set; }
        public string? Status { get; set; }
    }
}
