namespace FALOFinancialProofing.Helpers
{
    public static class UrlHelper
    {
        public static string GetImageUrl(HttpRequest request, string imagePath)
        {

            return imagePath != null ? $"{request.Scheme}://{request.Host}/{imagePath}" : null;
        }
    }

}
