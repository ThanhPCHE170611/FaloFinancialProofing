namespace FALOFinancialProofing.FALOHomePage.Models
{
    public class AttachmentFileDTO
    {
        public class AttachmentFileResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public AttachmentData Data { get; set; }
        }

        public class AttachmentData
        {
            public int TotalRecords { get; set; }
            public int Page { get; set; }
            public List<AttachmentFile> Data { get; set; }
        }

        public class AttachmentFile
        {
            public int Id { get; set; }
            public DateTime CreateAt { get; set; }
            public string Description { get; set; }
            public decimal ExpectedMoney { get; set; }
            public string CreateByName { get; set; }
            public string CreateByEmail { get; set; }
            public string AttachmentFilePath { get; set; }
        }
    }
}
