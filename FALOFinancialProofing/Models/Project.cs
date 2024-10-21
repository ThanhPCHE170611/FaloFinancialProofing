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
        public bool Status { get; set; }
        public ICollection<Campaign> Campaigns { get; set; }
        public ICollection<CreateProjectRequest> CreateProjectRequests { get; set; }

    }
}
