using FALOFinancialProofing.Core;

namespace FALOFinancialProofing.Models
{
    public class OrganizationMember : Entity<int>
    {
        public string UserId { get; set; }
        public int OrganizationId { get; set; }
        public Organization? Organization { get; set; }
        public User? User { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
