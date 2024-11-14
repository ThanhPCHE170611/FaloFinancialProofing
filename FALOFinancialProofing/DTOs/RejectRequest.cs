using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.DTOs
{
    public class RejectRequest
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string CurrentLoggingRole { get; set; }
        [Required]
        public int RequestId { get; set; }
        [Required, MinLength(10)]
        public string Feedback { get; set; }
    }
}
