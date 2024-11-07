using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.DTOs
{
    public class CassoApiResponseDTO
    {
        public int Error { get; set; }
        public string Message { get; set; }
        public List<BankAccount> Data { get; set; }
    }
}
