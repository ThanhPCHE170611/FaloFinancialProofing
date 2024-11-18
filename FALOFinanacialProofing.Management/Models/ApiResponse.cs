namespace FALOFinanacialProofing.Management.Models
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public TokenModel Data { get; set; }
    }
    public class TokenModel
    {
        public string AccessToken { get; set; }
        public string RefeshToken { get; set; }
    }
}
