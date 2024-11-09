using FALOFinancialProofing.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using FALOFinancialProofing.DTOs.RoleDTOs;

namespace FALOFinancialProofing.DTOs.CampaignMemberDTO
{
    public class CampaignMemberInformation
    {
        public int id { get; set; }
        public int CampaignId { get; set; }
        public string CampaignTitle { get; set; }
        public string UserId { get; set; } = null!;
        public string UserName{ get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public double Debt { get; set; } = 0;
        public RoleInformation roleInformation { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
