namespace FALOFinancialProofing.Models
{
    public class UserRole
    {
        public UserRole()
        {

        }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
    }
}
