using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.DTOs
{
    public class CreateAccountingBookRequest
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string CurrentRole { get; set;}
        [Required]
        public string UserId { get; set; }

        [Required]
        public string CampaignId { get; set; }

    }
}
