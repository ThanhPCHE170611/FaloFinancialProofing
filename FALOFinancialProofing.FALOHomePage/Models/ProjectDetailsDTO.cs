namespace FALOFinancialProofing.FALOHomePage.Models
{
    public class ProjectDetailsDTO
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }  
        public string Description { get; set; }  
        public DateTime DateOfCreation { get; set; } 
        public bool IsActive { get; set; } 
        public string OrganizationName { get; set; }  
        public string Status { get; set; }  
        public string CreatedBy { get; set; }  
        public string UserImage { get; set; }  
        public int? OrganizationId { get; set; }  
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Image { get; set; }
        public string? Logo { get; set; }

        //public ProjectDetailsDTO(int id, string name, string description, DateTime d, bool isActive, string oname, string status, string create, string image, int? oid)
        //{
        //    Id = id;
        //    ProjectName = name;
        //    Description = description;
        //    DateOfCreation = d;
        //    IsActive = isActive;
        //    OrganizationName = oname;
        //    Status = status;
        //    CreatedBy = create;
        //    UserImage = image;
        //    OrganizationId = oid;
        //}

        //public ProjectDetailsDTO()
        //{
        //}
    }

    public class ApiResponseProject
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ProjectDetailsDTO Data { get; set; }
    }
}
