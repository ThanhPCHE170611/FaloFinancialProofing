using FALOFinancialProofing.DTOs.BankDTO;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using FALOFinancialProofing.Services.BankServices;
using MockQueryable;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

public class BankServiceTests
{
    private readonly Mock<IRepository<Bank, int>> _mockBankRepository;
    private readonly BankService _bankService;

    public BankServiceTests()
    {
        _mockBankRepository = new Mock<IRepository<Bank, int>>();
        _bankService = new BankService(_mockBankRepository.Object);
    }

    [Fact]
    public async Task CreateBankAsync_ValidBank_ReturnsTrue()
    {
        // Arrange
        var bank = new Bank { Id = 1, OwnerName = "Test Owner", AccountNumber = "123456" };
        _mockBankRepository.Setup(repo => repo.InsertAsync(bank)).ReturnsAsync(bank);

        // Act
        var result = await _bankService.CreateBankAsync(bank);

        // Assert
        Assert.True(result);
        _mockBankRepository.Verify(repo => repo.InsertAsync(bank), Times.Once);
    }

    [Fact]
    public async Task CreateBankAsync_NullBank_ReturnsFalse()
    {
        // Act
        var result = await _bankService.CreateBankAsync(null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetBankByIdAsync_ValidId_ReturnsBankInformation()
    {
        // Arrange
        var bank = new Bank { Id = 1, OwnerName = "Test Owner", AccountNumber = "123456" };
        _mockBankRepository.Setup(repo => repo.Get(1)).ReturnsAsync(bank);

        // Act
        var result = await _bankService.GetBankByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(bank.Id, result.Id);
        Assert.Equal(bank.OwnerName, result.OwnerName);
        Assert.Equal(bank.AccountNumber, result.AccountNumber);
    }

    [Fact]
    public async Task GetBankByIdAsync_InvalidId_ReturnsNull()
    {
        // Arrange
        _mockBankRepository.Setup(repo => repo.Get(1)).ReturnsAsync((Bank)null);

        // Act
        var result = await _bankService.GetBankByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllBanksAsync_ReturnsListOfBanks()
    {
        // Arrange
        var banks = new List<Bank>
        {
            new Bank
            {
                Id = 1,
                OwnerName = "Owner 1",
                AccountNumber = "123456",
                BankCodeName = "Code1",
                acqId = 1001,
                CassoAccountID = 1000001,
                Campaigns = new List<Campaign>()
            },
            new Bank
            {
                Id = 2,
                OwnerName = "Owner 2",
                AccountNumber = "654321",
                BankCodeName = "Code2",
                acqId = 1002,
                CassoAccountID = 1000002,
                Campaigns = new List<Campaign>
                {
                    new Campaign { Id = 1, BankId = 2 }
                }
            },
            new Bank
            {
                Id = 3,
                OwnerName = "Owner 3",
                AccountNumber = "789012",
                BankCodeName = "Code3",
                acqId = 1003,
                CassoAccountID = 1000003,
                Campaigns = new List<Campaign>()
            }
        }.Where(bank => !bank.Campaigns.Any(c => c.BankId == bank.Id)).BuildMock();
        //_mockBankRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Bank, bool>>>())
        //.Where(bank => !bank.Campaigns.Any(c => c.BankId == bank.Id)).ToList()).Returns(banks);
        _mockBankRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Bank, bool>>>())).Returns(banks);

        // Act
        var result = await _bankService.GetAllBanksAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal(1, result.First(x => x.Id == 1).Id);
        Assert.Equal(3, result.First(x => x.Id == 3).Id);
    }

    [Fact]
    public async Task UpdateBankAsync_ValidBank_ReturnsTrue()
    {
        // Arrange
        var bank = new Bank { Id = 1, OwnerName = "Test Owner", AccountNumber = "123456" };
        _mockBankRepository.Setup(repo => repo.Get(1)).ReturnsAsync(bank);
        _mockBankRepository.Setup(repo => repo.UpdateAsync(bank)).ReturnsAsync(true);
        // Act
        var result = await _bankService.UpdateBankAsync(bank);

        // Assert
        Assert.True(result);
        _mockBankRepository.Verify(repo => repo.UpdateAsync(bank), Times.Once);
    }

    [Fact]
    public async Task UpdateBankAsync_InvalidBank_ReturnsFalse()
    {
        // Arrange
        var bank = new Bank { Id = 1, OwnerName = "Test Owner", AccountNumber = "123456" };
        _mockBankRepository.Setup(repo => repo.Get(1)).ReturnsAsync((Bank)null);

        // Act
        var result = await _bankService.UpdateBankAsync(bank);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteBankAsync_ValidId_ReturnsTrue()
    {
        // Arrange
        var bank = new Bank { Id = 1, OwnerName = "Test Owner", AccountNumber = "123456" };
        _mockBankRepository.Setup(repo => repo.Get(1)).ReturnsAsync(bank);
        _mockBankRepository.Setup(repo => repo.DeleteAsync(bank)).ReturnsAsync(true);

        // Act
        var result = await _bankService.DeleteBankAsync(1);

        // Assert
        Assert.True(result);
        _mockBankRepository.Verify(repo => repo.DeleteAsync(bank), Times.Once);
    }

    [Fact]
    public async Task DeleteBankAsync_InvalidId_ReturnsFalse()
    {
        // Arrange
        _mockBankRepository.Setup(repo => repo.Get(1)).ReturnsAsync((Bank)null);

        // Act
        var result = await _bankService.DeleteBankAsync(1);

        // Assert
        Assert.False(result);
    }
}
