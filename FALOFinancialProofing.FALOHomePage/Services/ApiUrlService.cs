namespace FALOFinancialProofing.FALOHomePage.Services
{
    public interface IApiUrlService
    {
        string GetApiBaseUrl();
    }

    public class ApiUrlService : IApiUrlService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiUrlService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetApiBaseUrl()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var scheme = request.Scheme;  
            var host = request.Host.Value;  
            return $"{scheme}://{host}/api/"; 
        }
    }
}
