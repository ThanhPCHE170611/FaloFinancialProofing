using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.Core
{
    public class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        [Key]
        public TPrimaryKey Id { get; set; }
    }
}