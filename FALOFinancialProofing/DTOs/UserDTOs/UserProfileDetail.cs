using FALOFinancialProofing.DTOs.UserSDGDTO;

namespace FALOFinancialProofing.DTOs.UserDTOs
{
    public class UserProfileDetail
    {
        public string Id { get; set; }
        //không cho đổi email
        public string Email { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateOnly? BirthDate { get; set; }
        // không truyền vào logoFile thì không update logo
        public bool? Gender { get; set; }
        public string? Address { get; set; }
        public string? WorkPlace { get; set; }
        public string? Bio { get; set; }
        public string? Image { get; set; }
        public string? Education { get; set; }
        public string? Skill { get; set; }
        public string? Hobby { get; set; }
        public string? Strength { get; set; }
        public string? VolunteerExperience { get; set; }
        public string? VolunteerGoal { get; set; }
        public List<SocialNetworkRequest> SocialNetworkRequests { get; set; } = new List<SocialNetworkRequest>();
        public List<UserSDGInformation> userSDGInformation { get; set; } = new List<UserSDGInformation>();
    }
}
