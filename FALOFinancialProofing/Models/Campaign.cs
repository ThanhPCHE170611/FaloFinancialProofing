﻿using FALOFinancialProofing.Core;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.Models
{
    public class Campaign : Entity<int>
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime DateOfCreation { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public double TargetAmount { get; set; }

        [MaxLength(200)]
        public string? Image { get; set; }

        public DateTime? EndDate { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public byte Status { get; set; }

        public ICollection<CampaignMember> CampaignMembers { get; set; } = new List<CampaignMember>();
        public ICollection<MoveNextCampaignStatusRequest> MoveNextCampaignStatusRequests { get; set; } = new List<MoveNextCampaignStatusRequest>();
        public ICollection<CreateCampaignRequest> CreateCampaignRequests { get; set; } = new List<CreateCampaignRequest>();

    }
}
