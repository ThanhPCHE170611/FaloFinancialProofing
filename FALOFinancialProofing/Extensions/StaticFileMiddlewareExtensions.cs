using FALOFinancialProofing.Helpers;
using Microsoft.Extensions.FileProviders;

namespace FALOFinancialProofing.Extensions
{
    public static class StaticFileMiddlewareExtensions
    {
        public static IApplicationBuilder CustomStaticFiles(this IApplicationBuilder app)
        {
            var staticFileDirectories = new List<(string Directory, string RequestPath)>
            {
                ($"{FolderImage.UserImageUpload}", $"/{FolderImage.UserImageUpload}"),
                ($"{FolderImage.ProjectImageUpload}", $"/{FolderImage.ProjectImageUpload}"),
                ($"{FolderImage.CampaignImageUpload}", $"/{FolderImage.CampaignImageUpload}"),
            };
            foreach (var (directory, requestPath) in staticFileDirectories)
            {
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), directory);
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), directory)),
                    RequestPath = requestPath
                });
            }

            return app;
        }
    }
}
