using FALOFinancialProofing.Core;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.Models
{
    public class Project : Entity<int>
    {
        public string CreatedBy { get; set; }
        public User User { get; set; }

        [StringLength(200)]
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreation { get; set; }
        public string? Status { get; set; }
        public bool IsActive { get; set; }
        // id có thể null bởi vì thằng khách hàng không cần phải đăng kí với tổ chức( nếu để là 0 thì lỗi khóa ngoại)
        public int? OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public ICollection<Campaign> Campaigns { get; set; }
        public ICollection<CreateProjectRequest> CreateProjectRequests { get; set; }
        [MaxLength(200)]
        public string? Image { get; set; }

    }
}
