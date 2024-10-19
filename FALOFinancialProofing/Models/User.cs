using FALOFinancialProofing.Core;
using Microsoft.AspNetCore.Identity;

namespace FALOFinancialProofing.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateOnly? BirthDate { get; set; }
        public ICollection<SDG> SDGs { get; set; } = new List<SDG>();
        public ICollection<SocialNetwork> SocialNetworks { get; set; } = new List<SocialNetwork>();
        public ICollection<CampaignMember> CampaignMembers { get; set; } = new List<CampaignMember>();
        public ICollection<CreateProjectRequest> SenderCreateProjectRequests { get; set; } = new List<CreateProjectRequest>();
        public ICollection<CreateProjectRequest> ReceiverCreateProjectRequests { get; set; } = new List<CreateProjectRequest>();
        public ICollection<TransactionLog> TransactionLogs { get; set; } = new List<TransactionLog>();

        //public string? Gender { get; set; }
        //public string? Address { get; set; }
        //public string? PhoneNumber { get; set; }
        //public string Email { get; set; }
        //public string SocialAddress { get; set; }
        //public string? Avatar { get; set; }
        //public string Password { get; set; }
        //public int Status { get; set; }
        //public virtual List<Role> Roles { get; set; }
    }
}