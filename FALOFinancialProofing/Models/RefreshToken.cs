using System.ComponentModel.DataAnnotations.Schema;

namespace FALOFinancialProofing.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        [ForeignKey(nameof(UserId))]
        public int UserId { get; set; }
        public User User { get; set; } 
        public string Token { get; set; } 
        public string JwtId { get; set; } 
        public bool IsRevoked { get; set; } 
        public bool IsUsed { get; set; } 
        public DateTime IssuedAt { get; set; } 
        public DateTime ExpiredAt { get; set; } 
    }
}
