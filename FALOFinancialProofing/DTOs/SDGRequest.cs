using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.DTOs
{
    public class SDGRequest
    {
        public int? Id { get; set; }
        public string? SDGName { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
