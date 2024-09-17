using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.Models
{
    public class User
    {
        public User() { }
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
