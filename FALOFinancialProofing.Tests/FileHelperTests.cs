using FALOFinancialProofing.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FALOFinancialProofing.Tests
{
    public class FileHelperTests
    {
        [Fact]
        public async Task DownLoadFile_FileExists_ReturnsFileData()
        {
            // Arrange
            var shortPath = "testfile.txt";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), shortPath);
            var fileContent = "Test content";
            await File.WriteAllTextAsync(filePath, fileContent);

            // Act
            var result = await FileHelper.DownLoadFile(shortPath);

            // Assert
            Assert.NotNull(result.Item1);
            Assert.Equal("text/plain", result.Item2);
            Assert.Equal("testfile.txt", result.Item3);

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public async Task DownLoadFile_FileNotExists_ReturnsNull()
        {
            // Arrange
            var shortPath = "nonexistentfile.txt";

            // Act
            var result = await FileHelper.DownLoadFile(shortPath);

            // Assert
            Assert.Null(result.Item1);
            Assert.Null(result.Item2);
            Assert.Null(result.Item3);
        }

        [Fact]
        public async Task DownLoadFile_ShortPathIsNullOrEmpty_ReturnsNull()
        {
            // Act
            var resultNull = await FileHelper.DownLoadFile(null);
            var resultEmpty = await FileHelper.DownLoadFile(string.Empty);

            // Assert
            Assert.Null(resultNull.Item1);
            Assert.Null(resultNull.Item2);
            Assert.Null(resultNull.Item3);

            Assert.Null(resultEmpty.Item1);
            Assert.Null(resultEmpty.Item2);
            Assert.Null(resultEmpty.Item3);
        }
    }
}
