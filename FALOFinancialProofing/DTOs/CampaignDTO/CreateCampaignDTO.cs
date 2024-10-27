using System.ComponentModel.DataAnnotations;
using FALOFinancialProofing.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace FALOFinancialProofing.DTOs.CampaignDTO
{
    public class CreateCampaignDTO
    {
        //public int? Id { get; set; }

        public int ProjectId { get; set; }
        [Required]
        public string CreateBy { get; set; } 
        public string Title { get; set; }
        public string? Description { get; set; }

        [Required]
        public DateTime DateOfCreation { get; set; }
        [Required]
        public double FundTarget { get; set; }
        public string? Image { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Address { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public int? BankingNumber { get; set; }
        [DefaultValue(CampaignStatus.FundRaising)]
        public string Status { get; set; }

    }
}
