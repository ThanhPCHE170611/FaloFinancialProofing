using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.DTOs
{
    public class SocialNetworkRequest
    {
        public int? Id { get; set; }
        public string? SocialNetworksLink { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
