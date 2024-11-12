using Microsoft.Identity.Client;
using NuGet.Packaging.Signing;

namespace FALOFinancialProofing.Helpers
{
    public class FileHelper
    {
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
            if (!Directory.Exists(uploadFolder))
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
            //using (var memoryStream = new MemoryStream())
            //{
            //    await file.CopyToAsync(memoryStream);
            //    byte[] fileBytes = memoryStream.ToArray();
            //    return Convert.ToBase64String(fileBytes);
            //}
        }

        //public static async Task<string> RemovePrefixStringUrlAsync(string imagePath)
        //{
        //    string currentPath = Directory.GetCurrentDirectory();
        //    imagePath = imagePath.Substring(currentPath.Length);
        //    //if ()
        //    //{

        //    //}


        //}

        //public static async Task<string> AddPrefixStringUrlAsync(string dbImagePath)
        //{
        //    string currentPath = Directory.GetCurrentDirectory();
        //    imagePath = imagePath.Substring(currentPath.Length);
        //    if ()
        //    {

        //    }


        //}
    }
}
