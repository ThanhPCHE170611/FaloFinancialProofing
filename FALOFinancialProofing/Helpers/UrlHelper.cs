namespace FALOFinancialProofing.Helpers
{
    public static class UrlHelper
    {
        public static string GetImageUrl(HttpRequest request, string imagePath, string folderName)
        {
            return imagePath != null ? imagePath.StartsWith(folderName) ? $"{request.Scheme}://{request.Host}/{imagePath}" : imagePath : null;
        }
    }

}
