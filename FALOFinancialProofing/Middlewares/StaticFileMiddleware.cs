using FALOFinancialProofing.Helpers;
using Microsoft.Extensions.FileProviders;

namespace FALOFinancialProofing.Middlewares
{
    public class StaticFileMiddleware
    {
        private readonly RequestDelegate _next;

        public StaticFileMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var staticFileDirectories = new List<(string Directory, string RequestPath)>
            {
                ($"{FolderImage.UserImageUpload}", $"/{FolderImage.UserImageUpload}"),
                ($"{FolderImage.ProjectImageUpload}", $"/{FolderImage.ProjectImageUpload}"),
                ($"{FolderImage.CampaignImageUpload}", $"/{FolderImage.CampaignImageUpload}"),
            };

            foreach (var (directory, requestPath) in staticFileDirectories)
            {
                context.Response.OnStarting(() =>
                {
                    context.RequestServices.GetRequiredService<IApplicationBuilder>().UseStaticFiles(new StaticFileOptions
                    {
                        FileProvider = new PhysicalFileProvider(
                            Path.Combine(Directory.GetCurrentDirectory(), directory)),
                        RequestPath = requestPath
                    });
                    return Task.CompletedTask;
                });
            }

            await _next(context);
        }
    }
}
