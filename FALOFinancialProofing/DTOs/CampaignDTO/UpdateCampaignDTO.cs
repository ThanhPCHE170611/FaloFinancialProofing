using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FALOFinancialProofing.Core;

namespace FALOFinancialProofing.DTOs.CampaignDTO
{
    public class UpdateCampaignDTO : Entity<int>
    {
        [JsonPropertyOrder(2)]
        public string UserId { get; set; } // của pm (k update)
        [JsonPropertyOrder(3)]
        public int? BankId { get; set; } // check valid
        public string Title { get; set; } // pm
        [MinLength(8, ErrorMessage = "Description must be at least 8 characters")]
        public string Description { get; set; }// pm
        public IFormFile? LogoFile { get; set; } // pm
        [Range(0, double.MaxValue, ErrorMessage = "FundTarget must be a non-negative number")]
        public double? FundTarget { get; set; }
        public string? Address { get; set; } // pm
        public bool? IsActive { get; set; }
        public string? Status { get; set; }


    }
}
