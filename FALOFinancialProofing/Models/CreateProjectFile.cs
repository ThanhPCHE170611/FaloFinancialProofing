using FALOFinancialProofing.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace FALOFinancialProofing.Models
{
    public class CreateProjectFile : Entity<int>
    {
        [ForeignKey("RequestId")]
        public int RequestId { get; set; }
        public string FilePath { get; set; }
        public CreateProjectRequest ProjectRequest { get; set; }
    }
}
