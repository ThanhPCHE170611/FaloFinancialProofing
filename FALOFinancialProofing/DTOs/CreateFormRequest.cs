﻿using Humanizer.Localisation;
using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.DTOs
{
    public class CreateFormRequest
    {
        public int? Id { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public string Description { get; set; }

        public double ExpectedMoney { get; set; }

        public string? Status { get; set; } = Resources.GetResource("ProcessStatus");
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public string CampaignId { get; set; }
        public string? TypeId { get; set; }
        [Required]
        public string ApproverId { get; set; }

        public List<IFormFile> UploadFiles { get; set; }

        public bool? IsValidate { get; set; } = false;
    }
}
