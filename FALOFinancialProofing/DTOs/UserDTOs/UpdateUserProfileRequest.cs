using FALOFinancialProofing.DTOs.RoleDTOs;
using FALOFinancialProofing.DTOs.SDGDTOs;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FALOFinancialProofing.DTOs.UserDTOs
{
    public class UpdateUserProfileRequest
    {
        public string Id { get; set; }
        //không cho đổi email
        [MaxLength(40)]
        public string FirstName { get; set; } = null!;
        [MaxLength(40)]
        public string LastName { get; set; } = null!;
        public DateOnly? BirthDate { get; set; }
        // không truyền vào logoFile thì không update logo
        public bool? Gender { get; set; }
        public string? Address { get; set; }
        public string? WorkPlace { get; set; }
        public string? Bio { get; set; }
        public IFormFile? LogoFile { get; set; }
        public string? Education { get; set; }
        public string? Skill { get; set; }
        public string? Hobby { get; set; }
        public string? Strength { get; set; }
        public string? VolunteerExperience { get; set; }
        public string? VolunteerGoal { get; set; }
        public string? PhoneNumber { get; set; }
        public string SocialNetworkRequestJsons { get; set; }
        public string SDGUserRequestJsons { get; set; }
        //public List<string> socialNetworkRequests { get; set; } = new List<string>();
        //[Required]
        [JsonIgnore]
        public List<SocialNetworkRequest> SocialNetworkRequests { get; set; } = new List<SocialNetworkRequest>();
        [JsonIgnore]
        public List<SDGUserRequest> sDGUserRequests { get; set; } = new List<SDGUserRequest>();
    }
}
