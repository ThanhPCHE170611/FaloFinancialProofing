using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.DTOs.CampaignDTO
{
    public class CreateCampaignClientRequest
    {
        [Required]
        public string CreateBy { get; set; }
        [Required]
        public int ProjectId { get; set; }
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
        //public string? BankingNumber { get; set; }
        public string? Status { get; set; }
        public List<IFormFile>? FormFiles { get; set; }
        public int? BankId { get; set; }
    }
}
