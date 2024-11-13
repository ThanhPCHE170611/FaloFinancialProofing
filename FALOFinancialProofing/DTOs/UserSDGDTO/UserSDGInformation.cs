using FALOFinancialProofing.DTOs.SDGDTOs;
using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.DTOs.UserSDGDTO
{
    public class UserSDGInformation
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public SDGInformation sDGInformation { get; set; }
    }
}
