namespace FALOFinancialProofing.FALOHomePage.Models
{
    public class LoginDTO
    {
        public class ApiResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public ApiData Data { get; set; }
        }

        public class ApiData
        {
            public string AccessToken { get; set; }
            public int UserId { get; set; }
        }
    }
}
