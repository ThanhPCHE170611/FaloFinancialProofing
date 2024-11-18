using FALOFinancialProofing.Attributes;
using Humanizer.Localisation;
using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.DTOs
{
    public class CreateFormRequest
    {
        public int? Id { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;

        [MinLength(8)]
        public string Description { get; set; }

        [MinimumExpectedMoney(0, ErrorMessage = "Expected money should greater than 0")]
        public double ExpectedMoney { get; set; }

        public string? Status { get; set; } = Resources.GetResource("ProcessStatus");
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public string CampaignId { get; set; }
        public string? TypeId { get; set; }
        [Required]
        public string ApproverId { get; set; }

        public IFormFile? UploadFiles { get; set; }

        public IFormFile? VoucherFile { get; set; }

        public bool? IsValidate { get; set; } = false;
    }
}
