using FALOFinancialProofing.Attributes;
using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.DTOs.OrganizationDTO
{
    public class CreateOrganization
    {
        [Required]
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Main_office { get; set; }
        public int Representative { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        [ImageFile]
        public IFormFile? LogoFile { get; set; }
        //public string Logo { get; set; }
        public string Description { get; set; }
        public string Vision { get; set; }
        public string Mission { get; set; }
        public string CoreValue { get; set; }
        public string MainActivity { get; set; }
        public string Interests { get; set; }
        public string VolunteerExperience { get; set; }
        public string VolunteerObjectives { get; set; }
        //public string Attachments { get; set; }
        public string Bio { get; set; }

    }
}
