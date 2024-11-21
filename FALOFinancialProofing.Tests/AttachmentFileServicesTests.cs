using FALOFinancialProofing.Constant;
using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using FALOFinancialProofing.Services.AttachmentFIleServices;
using MockQueryable;
using Moq;
using System.Linq.Expressions;

namespace FALOFinancialProofing.Tests
{
    public class AttachmentFileServicesTests
    {
        private readonly Mock<IRepository<AttachmentFile, int>> mockAttachmentRepo;
        private readonly Mock<IRepository<RequestForm, int>> mockRequestFormRepo;
        private readonly AttachmentFileServices attachmentFileServices;

        public AttachmentFileServicesTests()
        {
            mockAttachmentRepo = new Mock<IRepository<AttachmentFile, int>>();
            mockRequestFormRepo = new Mock<IRepository<RequestForm, int>>();
            attachmentFileServices = new AttachmentFileServices(
                mockAttachmentRepo.Object,
                mockRequestFormRepo.Object
            );
        }

        [Fact]
        public async Task CreateManyAttachmentFileAsync_ReturnTrue_WhenSuccess()
        {
            // Arrange
            var attachmentFileRequests = new List<AttachmentFileRequest>
            {
                new AttachmentFileRequest
                {
                    Id = 1,
                    FilePath = "file1.txt",
                    RequestId = 1
                },
                new AttachmentFileRequest
                {
                    Id = 1,
                    FilePath = "file1.txt",
                    RequestId = 2
                }
            };

            mockAttachmentRepo
                .Setup(repo => repo.InsertManyAsync(It.IsAny<List<AttachmentFile>>()))
                .ReturnsAsync(true);
            // Act
            var result = await attachmentFileServices
                        .CreateManyAttachmentFileAsync(attachmentFileRequests);
            // Result
            Assert.True(result);
            mockAttachmentRepo.Verify(repo => repo.InsertManyAsync(It.IsAny<List<AttachmentFile>>()), Times.Once);
        }

        [Fact]
        public async Task CreateManyAttachmentFileAsync_ReturnTrue_WhenFalse_DupplicateID()
        {
            // Arrange
            var attachmentFileRequests = new List<AttachmentFileRequest>
            {
                new AttachmentFileRequest
                {
                    Id = 1,
                    FilePath = "file1.txt",
                },
                new AttachmentFileRequest
                {
                    Id = 1,
                    FilePath = "file2.txt",
                }
            };

            mockAttachmentRepo
                .Setup(repo => repo.InsertManyAsync(It.IsAny<List<AttachmentFile>>()))
                .ReturnsAsync(null);
            // Act
            var result = await attachmentFileServices
                        .CreateManyAttachmentFileAsync(attachmentFileRequests);
            // Result
            Assert.False(result);
            mockAttachmentRepo.Verify(repo => repo.InsertManyAsync(It.IsAny<List<AttachmentFile>>()), Times.Once);
        }

        [Fact]
        public async Task CreateManyAttachmentFileAsync_ReturnTrue_WhenFalse_RequestIDNull()
        {
            // Arrange
            var attachmentFileRequests = new List<AttachmentFileRequest>
            {
                new AttachmentFileRequest
                {
                    Id = 2,
                    FilePath = "file1.txt",
                },
                new AttachmentFileRequest
                {
                    Id = 1,
                    FilePath = "file1.txt",
                }
            };

            mockAttachmentRepo
                .Setup(repo => repo.InsertManyAsync(It.IsAny<List<AttachmentFile>>()))
                .ReturnsAsync(null);
            // Act
            var result = await attachmentFileServices
                        .CreateManyAttachmentFileAsync(attachmentFileRequests);
            // Result
            Assert.False(result);
            mockAttachmentRepo.Verify(repo => repo.InsertManyAsync(It.IsAny<List<AttachmentFile>>()), Times.Once);
        }

        [Fact]
        public async Task DownloadPrePayAttachmentFileByFileName_FileExists_ReturnsFileData()
        {
            // Arrange
            var fileName = "testfile1.zip";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "PrePayUploads", fileName);
            var fileBytes = new byte[] { 1, 2, 3, 4 };

            mockAttachmentRepo.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<AttachmentFile, bool>>>()))
                .Returns(new List<AttachmentFile>
                {
                    new AttachmentFile
                    {
                        Id = 1,
                        FilePath = filePath,
                        RequestId = 1
                    }
                }.AsQueryable());

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            await File.WriteAllBytesAsync(filePath, fileBytes);

            // Act
            var result = await attachmentFileServices.DownloadPrePayAttachmentFileByFileName(fileName);

            // Assert
            Assert.NotNull(result.fileBytes);
            Assert.Equal("application/zip", result.contentType);
            Assert.Equal(fileName, result.fileName);

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public async Task DownloadPrePayAttachmentFileByFileName_FileNotExists_ReturnsNull()
        {
            // Arrange
            var fileName = "testfile.zip";

            // Act
            var result = await attachmentFileServices.DownloadPrePayAttachmentFileByFileName(fileName);

            // Assert
            Assert.Null(result.fileBytes);
            Assert.Null(result.contentType);
            Assert.Null(result.fileName);

        }
        [Fact]
        public async Task DownloadPrePayAttachmentFileByFileNameRAR_FileExists_ReturnsFileData()
        {
            // Arrange
            var fileName = "testfile.rar";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "PrePayUploads", fileName);
            var fileBytes = new byte[] { 1, 2, 3, 4 };

            mockAttachmentRepo.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<AttachmentFile, bool>>>()))
                .Returns(new List<AttachmentFile>
                {
                    new AttachmentFile
                    {
                        Id = 1,
                        FilePath = filePath,
                        RequestId = 1
                    }
                }.AsQueryable());

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            await File.WriteAllBytesAsync(filePath, fileBytes);

            // Act
            var result = await attachmentFileServices.DownloadPrePayAttachmentFileByFileName(fileName);

            // Assert
            Assert.NotNull(result.fileBytes);
            Assert.Equal("application/x-rar-compressed", result.contentType);
            Assert.Equal(fileName, result.fileName);

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public async Task DownloadPrePayAttachmentFileByFileNameOther_FileExists_ReturnsFileData()
        {
            // Arrange
            var fileName = "testfile.txt";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "PrePayUploads", fileName);
            var fileBytes = new byte[] { 1, 2, 3, 4 };

            mockAttachmentRepo.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<AttachmentFile, bool>>>()))
                .Returns(new List<AttachmentFile>
                {
                    new AttachmentFile
                    {
                        Id = 1,
                        FilePath = filePath,
                        RequestId = 1
                    }
                }.AsQueryable());

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            await File.WriteAllBytesAsync(filePath, fileBytes);

            // Act
            var result = await attachmentFileServices.DownloadPrePayAttachmentFileByFileName(fileName);

            // Assert
            Assert.NotNull(result.fileBytes);
            Assert.Equal("application/octet-stream", result.contentType);
            Assert.Equal(fileName, result.fileName);

            // Cleanup
            File.Delete(filePath);
        }
        [Fact]
        public async Task DownloadPaymentAttachmentFileByFileName_FileExists_ReturnsFileData()
        {
            // Arrange
            var fileName = "testfile.zip";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "PaymentUploads", fileName);
            var fileBytes = new byte[] { 1, 2, 3, 4 };

            var data = new List<AttachmentFile>
        {
            new AttachmentFile
            {
                Id = 1,
                FilePath = filePath,
                RequestId = 1,
                RequestForm = new RequestForm { Id = 1 } // Simulating a valid RequestForm
            }
        }.AsQueryable().BuildMock();

            mockAttachmentRepo.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<AttachmentFile, bool>>>()))
                .Returns(data);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            await File.WriteAllBytesAsync(filePath, fileBytes);

            // Act
            var result = await attachmentFileServices.DownloadPaymentAttachmentFileByFileName(fileName);

            // Assert
            Assert.NotNull(result.fileBytes);
            Assert.Equal("application/zip", result.contentType);
            Assert.Equal(fileName, result.fileName);

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public async Task DownloadPaymentAttachmentFileByFileName_FileNotExists_ReturnsNull()
        {
            // Arrange
            var fileName = "testfile.zip";

            // Act
            var result = await attachmentFileServices.DownloadPaymentAttachmentFileByFileName(fileName);

            // Assert
            Assert.Null(result.fileBytes);
            Assert.Null(result.contentType);
            Assert.Null(result.fileName);

        }
        [Fact]
        public async Task DownloadPaymentAttachmentFileByFileNameRAR_FileExists_ReturnsFileData()
        {
            // Arrange
            var fileName = "testfile.rar";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "PaymentUploads", fileName);
            var fileBytes = new byte[] { 1, 2, 3, 4 };
            var data = new List<AttachmentFile>
        {
            new AttachmentFile
            {
                Id = 1,
                FilePath = filePath,
                RequestId = 1,
                RequestForm = new RequestForm { Id = 1 } // Simulating a valid RequestForm
            }
        }.AsQueryable().BuildMock();

            mockAttachmentRepo.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<AttachmentFile, bool>>>()))
                .Returns(data);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            await File.WriteAllBytesAsync(filePath, fileBytes);

            // Act
            var result = await attachmentFileServices.DownloadPaymentAttachmentFileByFileName(fileName);

            // Assert
            Assert.NotNull(result.fileBytes);
            Assert.Equal("application/x-rar-compressed", result.contentType);
            Assert.Equal(fileName, result.fileName);

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public async Task DownloadPaymentAttachmentFileByFileNameOther_FileExists_ReturnsFileData()
        {
            // Arrange
            var fileName = "testfile.txt";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "PaymentUploads", fileName);
            var fileBytes = new byte[] { 1, 2, 3, 4 };

            var data = new List<AttachmentFile>
            {
                new AttachmentFile
                {
                    Id = 1,
                    FilePath = filePath,
                    RequestId = 1,
                    RequestForm = new RequestForm { Id = 1 } // Simulating a valid RequestForm
                }
            }.AsQueryable().BuildMock();

            mockAttachmentRepo.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<AttachmentFile, bool>>>()))
                .Returns(data);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            await File.WriteAllBytesAsync(filePath, fileBytes);

            // Act
            var result = await attachmentFileServices.DownloadPaymentAttachmentFileByFileName(fileName);

            // Assert
            Assert.NotNull(result.fileBytes);
            Assert.Equal("application/octet-stream", result.contentType);
            Assert.Equal(fileName, result.fileName);

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public async Task GetAllCurrentAttachmentInCampaign_ReturnsAttachmentFiles()
        {
            // Arrange
            var campaignId = 1;
            var data = new List<AttachmentFile>
            {
                new AttachmentFile
                {
                    Id = 1,
                    FilePath = "file1.txt",
                    RequestId = 1,
                    RequestForm = new RequestForm { Id = 1, CampaignId = campaignId }
                },
                new AttachmentFile
                {
                    Id = 2,
                    FilePath = "file2.txt",
                    RequestId = 2,
                    RequestForm = new RequestForm { Id = 2, CampaignId = campaignId }
                }
            }.AsQueryable().BuildMock();

            mockAttachmentRepo.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<AttachmentFile, bool>>>()))
                .Returns(data);

            // Act
            var result = await attachmentFileServices.GetAllCurrentAttachmentInCampaign(campaignId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].Id);
            Assert.Equal(2, result[1].Id);
        }

        [Fact]
        public async Task GetAllCurrentAttachmentInCampaign_Exeption()
        {
            // Arrange
            var campaignId = 1;


            mockAttachmentRepo.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<AttachmentFile, bool>>>()))
                .Throws(new Exception());

            // Act
            var result = await attachmentFileServices.GetAllCurrentAttachmentInCampaign(campaignId);

            // Assert
            Assert.Empty(result);

        }

        [Fact]
        public async Task GetAllAttachmentInCampaignByRequest_ReturnsRequestForms()
        {
            // Arrange
            var campaignId = 1;
            var data = new List<RequestForm>
            {
                new RequestForm
                {
                    Id = 1,
                    CampaignId = campaignId
                },
                new RequestForm
                {
                    Id = 2,
                    CampaignId = campaignId
                }
            }.AsQueryable().BuildMock();

            mockRequestFormRepo.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<RequestForm, bool>>>()))
                .Returns(data);

            // Act
            var result = await attachmentFileServices.GetAllAttachmentInCampaignByRequest(campaignId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].Id);
            Assert.Equal(2, result[1].Id);
        }

        [Fact]
        public async Task DownloadAttachmentFileWithNoTypeByFileName_FileExists_ReturnsFileDataZIP()
        {
            // Arrange
            var fileName = "testfile.zip";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "PrepayUploads", fileName);
            var fileBytes = new byte[] { 1, 2, 3, 4 };

            var campaignId = 1;
            var data = new List<AttachmentFile>
            {
                new AttachmentFile
                {
                    Id = 1,
                    FilePath = "file1.txt",
                    RequestId = 1,
                    RequestForm = new RequestForm { Id = 1, CampaignId = campaignId, TypeId = 1 }
                },

            }.AsQueryable().BuildMock();

            mockAttachmentRepo.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<AttachmentFile, bool>>>()))
                .Returns(data);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            await File.WriteAllBytesAsync(filePath, fileBytes);

            // Act
            var result = await attachmentFileServices.DownloadAttachmentFileWithNoTypeByFileName(fileName);

            // Assert
            Assert.NotNull(result.fileBytes);
            Assert.Equal("application/zip", result.contentType);
            Assert.Equal(fileName, result.fileName);

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public async Task DownloadAttachmentFileWithNoTypeByFileName_FileExists_ReturnsFileDataRAR()
        {
            // Arrange
            var fileName = "testfile.rar";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "PaymentUploads", fileName);
            var fileBytes = new byte[] { 1, 2, 3, 4 };

            var campaignId = 1;
            var data = new List<AttachmentFile>
            {
                new AttachmentFile
                {
                    Id = 1,
                    FilePath = "file1.txt",
                    RequestId = 1,
                    RequestForm = new RequestForm { Id = 1, CampaignId = campaignId, TypeId = 2 }
                },

            }.AsQueryable().BuildMock();

            mockAttachmentRepo.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<AttachmentFile, bool>>>()))
                .Returns(data);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            await File.WriteAllBytesAsync(filePath, fileBytes);

            // Act
            var result = await attachmentFileServices.DownloadAttachmentFileWithNoTypeByFileName(fileName);


            // Assert
            Assert.NotNull(result.fileBytes);
            Assert.Equal("application/x-rar-compressed", result.contentType);
            Assert.Equal(fileName, result.fileName);

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public async Task DownloadAttachmentFileWithNoTypeByFileName_FileExists_ReturnsFileDataOther()
        {
            // Arrange
            var fileName = "testfile.txt";
            // Arrange
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "PrepayUploads", fileName);
            var fileBytes = new byte[] { 1, 2, 3, 4 };

            var campaignId = 1;
            var data = new List<AttachmentFile>
            {
                new AttachmentFile
                {
                    Id = 1,
                    FilePath = "file1.txt",
                    RequestId = 1,
                    RequestForm = new RequestForm { Id = 1, CampaignId = campaignId, TypeId = 1 }
                },

            }.AsQueryable().BuildMock();

            mockAttachmentRepo.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<AttachmentFile, bool>>>()))
                .Returns(data);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            await File.WriteAllBytesAsync(filePath, fileBytes);

            // Act
            var result = await attachmentFileServices.DownloadAttachmentFileWithNoTypeByFileName(fileName);

            // Assert
            Assert.NotNull(result.fileBytes);
            Assert.Equal("application/octet-stream", result.contentType);
            Assert.Equal(fileName, result.fileName);

            // Cleanup
            File.Delete(filePath);

        }

        [Fact]
        public async Task GetAllPaymentAttachmentInCampaignWithRequest_ShouldReturnRequestsWithAttachments()
        {
            // Arrange
            int campaignId = 1;

            var mockData = new List<RequestForm>
        {
            new RequestForm
            {
                Id = 1,
                CampaignId = campaignId,
                Status = "Approved",
                TypeId = IntConstant.PaymentRequestType,
                CreateAt = DateTime.UtcNow,
                Description = "Payment Request 1",
                ExpectedMoney = 1000,
                User = new User
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com"
                },
                AttachmentFiles = new List<AttachmentFile>
                {
                    new AttachmentFile { FilePath = "path/to/file1" }
                }
            },
            new RequestForm
            {
                Id = 2,
                CampaignId = campaignId,
                Status = "Approved",
                TypeId = IntConstant.PaymentRequestType,
                CreateAt = DateTime.UtcNow,
                Description = "Payment Request 2",
                ExpectedMoney = 2000,
                User = new User
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com"
                },
                AttachmentFiles = new List<AttachmentFile>()
            }
        }.AsQueryable().BuildMock();

            mockRequestFormRepo.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<RequestForm, bool>>>()))
                                .Returns(mockData);

            // Act
            var result = await attachmentFileServices.GetAllPaymentAttachmentInCampaignWithRequest(campaignId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.Equal("Payment Request 1", result[0].Description);
            Assert.Equal("John Doe", result[0].CreateByName);
            Assert.Equal("path/to/file1", result[0].AttachmentFilePath);

            Assert.Equal("Payment Request 2", result[1].Description);
            Assert.Equal("Jane Smith", result[1].CreateByName);
            Assert.Null(result[1].AttachmentFilePath);

            mockRequestFormRepo.Verify(repo => repo.GetAll(It.IsAny<Expression<Func<RequestForm, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetAllPaymentAttachmentInCampaignWithRequest_ShouldReturnEmptyList_WhenNoDataFound()
        {
            // Arrange
            int campaignId = 1;

            var mockData = new List<RequestForm>().AsQueryable().BuildMock();
            mockRequestFormRepo.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<RequestForm, bool>>>()))
                                .Returns(mockData);

            // Act
            var result = await attachmentFileServices.GetAllPaymentAttachmentInCampaignWithRequest(campaignId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            mockRequestFormRepo.Verify(repo => repo.GetAll(It.IsAny<Expression<Func<RequestForm, bool>>>()), Times.Once);
        }
    }
}
