using FALOFinancialProofing.Core;
using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.Models
{
    public class Project : Entity<int>
    {
        public string CreatedBy { get; set; }
        [StringLength(200)]
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreation { get; set; } 
        public bool Status { get; set; }
    }
}
