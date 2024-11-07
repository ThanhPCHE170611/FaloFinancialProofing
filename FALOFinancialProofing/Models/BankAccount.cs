using FALOFinancialProofing.Core;

namespace FALOFinancialProofing.Models
{
    public class BankAccount : Entity<int>
    {
        public string AccountId { get; set; } // ID của tài khoản từ Casso
        public string AccountNumber { get; set; } // Số tài khoản
        public string AccountName { get; set; } // Tên tài khoản
        public string BankCode { get; set; } // Mã ngân hàng
        public double? Balance { get; set; } // Số dư

    }
}
