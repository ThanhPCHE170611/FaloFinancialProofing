using FALOFinancialProofing.Attributes;
using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.DTOs.CampaignDTO
{
    public class CreateCampaignClientRequest
    {
        [Required]
        public string CreateBy { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 8, ErrorMessage = "Title must be at least 8 and max 200 characters")]
        public string Title { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Description must be at least 8 characters")]
        public string Description { get; set; }
        [Required]
        public double FundTarget { get; set; }
        //public string? Image { get; set; } // sẽ xóa
        [ImageFile]
        public IFormFile? LogoFile { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Address { get; set; }
        public List<IFormFile>? FormFiles { get; set; }
        //public int? BankId { get; set; }

    }
}
