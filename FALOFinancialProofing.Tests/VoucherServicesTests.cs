using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using FALOFinancialProofing.Services.VoucherServices;
using MockQueryable;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

public class VoucherServicesTests
{
    private readonly Mock<IRepository<Voucher, int>> mockVoucherRepo;
    private readonly VoucherServices voucherServices;

    public VoucherServicesTests()
    {
        mockVoucherRepo = new Mock<IRepository<Voucher, int>>();
        voucherServices = new VoucherServices(mockVoucherRepo.Object);
    }

    [Fact]
    public async Task CreateManyVoucherAsync_ReturnsTrue_WhenSuccess()
    {
        // Arrange
        var dtos = new List<VoucherRequest> { new VoucherRequest { Id = 1, FilePath = "path", ApproveId = 1 } };
        mockVoucherRepo.Setup(repo => repo.InsertManyAsync(It.IsAny<List<Voucher>>())).ReturnsAsync(true);

        // Act
        var result = await voucherServices.CreateManyVoucherAsync(dtos);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CreateManyVoucherAsync_ReturnsFalse_WhenExceptionThrown()
    {
        // Arrange
        var dtos = new List<VoucherRequest> { new VoucherRequest { Id = 1, FilePath = "path", ApproveId = 1 } };
        mockVoucherRepo.Setup(repo => repo.InsertManyAsync(It.IsAny<List<Voucher>>())).ThrowsAsync(new System.Exception());

        // Act
        var result = await voucherServices.CreateManyVoucherAsync(dtos);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetVouchersByApproveIdAsync_ReturnsVouchers_WhenFound()
    {
        // Arrange
        var approveId = 1;
        var vouchers = new List<Voucher>
    {
        new Voucher
        {
            Id = 1,
            FilePath = "path",
            Status = "Approved",
            ApproveProcess = new ApproveProcess { Id = approveId }
        }
    }.AsQueryable().BuildMock();

        mockVoucherRepo.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Voucher, bool>>>()))
            .Returns(vouchers);

        // Act
        var result = await voucherServices.GetVouchersByApproveIdAsync(approveId);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(approveId, result.First().ApproveId);
        Assert.Equal("path", result.First().FilePath);
        Assert.Equal("Approved", result.First().Status);
    }

    [Fact]
    public async Task GetVouchersByApproveIdAsync_ReturnsNull_WhenExceptionThrown()
    {
        // Arrange
        var approveId = 1;
        mockVoucherRepo.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Voucher, bool>>>())).Throws(new System.Exception());

        // Act
        var result = await voucherServices.GetVouchersByApproveIdAsync(approveId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DownloadPrePayAttachmentFileByFileName_FileExists_ReturnsFileData()
    {
        // Arrange
        var fileName = "test.zip";
        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Pre-PayVouchers");
        var filePath = Path.Combine(directoryPath, fileName);
        var fileBytes = new byte[] { 1, 2, 3 };

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        await System.IO.File.WriteAllBytesAsync(filePath, fileBytes);

        // Act
        var result = await voucherServices.DownloadPrePayAttachmentFileByFileName(fileName);

        // Assert
        Assert.NotNull(result.fileBytes);
        Assert.Equal("application/zip", result.contentType);
        Assert.Equal(fileName, result.downloadFileName);

        // Cleanup
        System.IO.File.Delete(filePath);
        Directory.Delete(directoryPath);
    }

    [Fact]
    public async Task DownloadPrePayAttachmentFileByFileName_FileNotExists_ReturnsNull()
    {
        // Arrange
        var fileName = "nonexistent.zip";

        // Act
        var result = await voucherServices.DownloadPrePayAttachmentFileByFileName(fileName);

        // Assert
        Assert.Null(result.fileBytes);
        Assert.Null(result.contentType);
        Assert.Null(result.downloadFileName);
    }

    [Fact]
    public async Task DownloadPaymentAttachmentFileByFileName_FileExists_ReturnsFileData()
    {
        // Arrange
        var fileName = "test.zip";
        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "PaymentVouchers");
        var filePath = Path.Combine(directoryPath, fileName);
        var fileBytes = new byte[] { 1, 2, 3 };

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        await System.IO.File.WriteAllBytesAsync(filePath, fileBytes);

        // Act
        var result = await voucherServices.DownloadPaymentAttachmentFileByFileName(fileName);

        // Assert
        Assert.NotNull(result.fileBytes);
        Assert.Equal("application/zip", result.contentType);
        Assert.Equal(fileName, result.downloadFileName);

        // Cleanup
        System.IO.File.Delete(filePath);
        Directory.Delete(directoryPath);
    }

    [Fact]
    public async Task DownloadPaymentAttachmentFileByFileName_FileNotExists_ReturnsNull()
    {
        // Arrange
        var fileName = "nonexistent.zip";

        // Act
        var result = await voucherServices.DownloadPaymentAttachmentFileByFileName(fileName);

        // Assert
        Assert.Null(result.fileBytes);
        Assert.Null(result.contentType);
        Assert.Null(result.downloadFileName);
    }
}
