using Microsoft.Identity.Client;
using NuGet.Packaging.Signing;
using System.IO;

namespace FALOFinancialProofing.Helpers
{

    public class FileHelper
    {
        public const int UserImageMaxFileSize = 5 * 1024 * 1024;
        public const int OrganizationImageMaxFileSize = 5 * 1024 * 1024;
        public const int CampaignImageMaxFileSize = 10 * 1024 * 1024;
        public const int ProjectImageMaxFileSize = 10 * 1024 * 1024;

        public static async Task<string> ConvertIFormFileToStringAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();
                return Convert.ToBase64String(fileBytes);
            }
        }

        // áp dụng cho updateProfile
        public static async Task<string> SaveImageAndReturnShortPathAsync(IFormFile file, string folderName, string fileShortPath)
        {
            //fileShortPath include folderName and filename
            if (file == null || file.Length == 0)
            {
                return null;
            }
            string deleteFilePath = null;
            if (!string.IsNullOrEmpty(fileShortPath))
            {
                deleteFilePath = Path.Combine(Directory.GetCurrentDirectory(), fileShortPath);
                if (File.Exists(deleteFilePath))
                {
                    File.Delete(deleteFilePath);
                }
            }
            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (!Directory.Exists(uploadFolder)) // không cần dùng cũng được khi đã tạo folder trong startup
            {
                Directory.CreateDirectory(uploadFolder);
            }
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            string fileName = $"{Guid.NewGuid()}{extension}";
            string imagePath = Path.Combine(uploadFolder, fileName);
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return $"{folderName}/{fileName}";
        }

        private static string GetContentType(string path)
        {
            var types = new Dictionary<string, string>
            {
                { ".txt", "text/plain" },
                { ".pdf", "application/pdf" },
                { ".doc", "application/vnd.ms-word" },
                { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
                { ".xls", "application/vnd.ms-excel" },
                { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                { ".png", "image/png" },
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".gif", "image/gif" },
                { ".csv", "text/csv" },
                { ".zip", "application/zip" }
            };

            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types.ContainsKey(ext) ? types[ext] : "application/octet-stream";
        }

        public static async Task<(Stream?, string?, string?)> DownLoadFile(string shortPath)
        {
            if (string.IsNullOrEmpty(shortPath))
                return (null, null, null);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), shortPath);
            if (!File.Exists(filePath))
                return (null, null, null);
            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return (memory, GetContentType(filePath), Path.GetFileName(filePath));
        }
    }
}
