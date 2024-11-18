using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.DTOs.SDGDTOs
{
    public class SDGUserRequest
    {
        public string UserId { get; set; }
        public int SDGId { get; set; }
        public bool IsActive { get; set; }

    }
}
