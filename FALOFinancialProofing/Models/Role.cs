namespace FALOFinancialProofing.Models
{
    public class Role
    {
        public Role()
        {
            
        }
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
