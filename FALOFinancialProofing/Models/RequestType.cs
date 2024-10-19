using FALOFinancialProofing.Core;

namespace FALOFinancialProofing.Models
{
    public class RequestType : Entity<int>
    { 
        public string TypeName { get; set; }
        public ICollection<RequestForm> RequestForms { get; set; } = new List<RequestForm>();
    }
}
