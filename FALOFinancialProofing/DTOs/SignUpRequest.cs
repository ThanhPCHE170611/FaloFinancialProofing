namespace FALOFinancialProofing.DTOs
{
    public class SignUpRequest
    {
        public string? FullName { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? SocialAddress { get; set; }
        public string? Avatar { get; set; }
        public string Password { get; set; }
        public int? Status { get; set; }
    }
}
