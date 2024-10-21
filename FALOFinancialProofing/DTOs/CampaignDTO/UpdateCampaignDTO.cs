using System.ComponentModel.DataAnnotations;
using FALOFinancialProofing.Core;

namespace FALOFinancialProofing.DTOs.CampaignDTO
{
    public class UpdateCampaignDTO : Entity<int>
    {
        //public int? Id { get; set; }

        //[Required]
        //public string CreateBy { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }

        //[Required]
        //public DateTime DateOfCreation { get; set; }
        [Required]
        public double FundTarget { get; set; }
        public string? Image { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Address { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public int? BankingNumber { get; set; }
        [Required]
        public byte Status { get; set; }
    }
}
