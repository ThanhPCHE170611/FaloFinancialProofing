using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.DTOs.UserSDGDTO
{
    public class CreateUserSDG
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int SDGId { get; set; }
        public SDG SDG { get; set; }
    }
}
