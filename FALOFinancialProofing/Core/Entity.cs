using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FALOFinancialProofing.Core
{
    public class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        [Key]
        [JsonPropertyOrder(1)]
        public TPrimaryKey Id { get; set; }
    }
}