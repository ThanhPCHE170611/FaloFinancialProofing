using FALOFinancialProofing.DTOs.TransactionLogsDTOs;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using FALOFinancialProofing.Services.TransactionLogsServices;
using Microsoft.AspNetCore.Identity;
using MockQueryable;
using Moq;
using System.Linq.Expressions;

namespace FALOFinancialProofing.Tests.Services
{
    public class TransactionLogServiceTests
    {
        private readonly Mock<IRepository<TransactionLog, int>> _transactionLogRepositoryMock;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly TransactionLogService _transactionLogService;

        public TransactionLogServiceTests()
        {
            _transactionLogRepositoryMock = new Mock<IRepository<TransactionLog, int>>();
            _userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _transactionLogService = new TransactionLogService(_transactionLogRepositoryMock.Object, _userManagerMock.Object);
        }

        [Fact]
        public async Task CreateTransactionLogAsync_ShouldReturnTrue_WhenTransactionLogIsCreated()
        {
            // Arrange
            var createTransactionLog = new CreateTransactionLog
            {
                CampaignId = 1,
                TransactionDate = DateTime.Now,
                Amount = 100,
                Description = "Test Description"
            };

            var transactionLog = new TransactionLog
            {
                Id = 1,
                CampaignId = createTransactionLog.CampaignId,
                TransactionDate = createTransactionLog.TransactionDate,
                Amount = createTransactionLog.Amount,
                Description = createTransactionLog.Description
            };

            _transactionLogRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<TransactionLog>())).ReturnsAsync(transactionLog);

            // Act
            var result = await _transactionLogService.CreateTransactionLogAsync(createTransactionLog);

            // Assert
            Assert.True(result);
            _transactionLogRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<TransactionLog>()), Times.Once);
        }


        [Fact]
        public async Task CreateTransactionLogAsync_ShouldReturnFalse_WhenExceptionIsThrown()
        {
            // Arrange
            var createTransactionLog = new CreateTransactionLog
            {
                CampaignId = 1,
                TransactionDate = DateTime.Now,
                Amount = 100,
                Description = "Test Description"
            };

            _transactionLogRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<TransactionLog>())).ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _transactionLogService.CreateTransactionLogAsync(createTransactionLog);

            // Assert
            Assert.False(result);
            _transactionLogRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<TransactionLog>()), Times.Once);
        }

        [Fact]
        public async Task GetTransactionLogByIdAsync_ShouldReturnTransactionLog_WhenTransactionLogExists()
        {
            // Arrange
            var transactionLog = new TransactionLog { Id = 1, CampaignId = 1, Amount = 100 };
            _transactionLogRepositoryMock.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync(transactionLog);

            // Act
            var result = await _transactionLogService.GetTransactionLogByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            _transactionLogRepositoryMock.Verify(repo => repo.Get(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetTransactionLogByIdAsync_ShouldReturnNull_WhenTransactionLogDoesNotExist()
        {
            // Arrange
            _transactionLogRepositoryMock.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync((TransactionLog)null);

            // Act
            var result = await _transactionLogService.GetTransactionLogByIdAsync(1);

            // Assert
            Assert.Null(result);
            _transactionLogRepositoryMock.Verify(repo => repo.Get(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetAllTransactionLogsAsync_ShouldReturnAllTransactionLogs()
        {
            // Arrange
            var transactionLogs = new List<TransactionLog>
            {
                new TransactionLog { Id = 1, CampaignId = 1, Amount = 100 },
                new TransactionLog { Id = 2, CampaignId = 2, Amount = 200 }
            };
            _transactionLogRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<TransactionLog, bool>>>())).Returns(transactionLogs.AsQueryable().BuildMock());

            // Act
            var result = await _transactionLogService.GetAllTransactionLogsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _transactionLogRepositoryMock.Verify(repo => repo.GetAll(It.IsAny<Expression<Func<TransactionLog, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetAllTransactionLogsAsync_ShouldThrowException_WhenRepositoryThrowsException()
        {
            // Arrange
            _transactionLogRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<TransactionLog, bool>>>())).Throws(new Exception("Test Exception"));

            // Act & Assert
            var result = await _transactionLogService.GetAllTransactionLogsAsync();
            Assert.Null(result);
            _transactionLogRepositoryMock.Verify(repo => repo.GetAll(It.IsAny<Expression<Func<TransactionLog, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetUserTransactionsByCampaignIdAsync_ShouldReturnTransactions_WhenTransactionsExist()
        {
            // Arrange
            var campaignId = 1;
            var transactionLogs = new List<TransactionLog>
            {
                new TransactionLog { CampaignId = campaignId, Amount = 100, CreateQrCode = new CreateQrCode { UserId = "user1", Id = 1, IsPaid = true }, Campaign = new Campaign { Title = "Campaign 1" }, Description = "Test", TransactionDate = DateTime.Now, tid = "tid1" },
                new TransactionLog { CampaignId = campaignId, Amount = 200, CreateQrCode = new CreateQrCode { UserId = "user2", Id = 2, IsPaid = false }, Campaign = new Campaign { Title = "Campaign 1" }, Description = "Test", TransactionDate = DateTime.Now, tid = "tid2" }
            }.AsQueryable().BuildMock();

            _transactionLogRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<TransactionLog, bool>>>())).Returns(transactionLogs);

            // Act
            var result = await _transactionLogService.GetUserTransactionsByCampaignIdAsync(campaignId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _transactionLogRepositoryMock.Verify(repo => repo.GetAll(It.IsAny<Expression<Func<TransactionLog, bool>>>()), Times.Once);
        }


        [Fact]
        public async Task GetUserTransactionsByCampaignIdAsync_ShouldReturnEmptyList_WhenNoTransactionsExist()
        {
            // Arrange
            var campaignId = 1;
            var transactionLogs = new List<TransactionLog>().AsQueryable().BuildMock();

            _transactionLogRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<TransactionLog, bool>>>())).Returns(transactionLogs);

            // Act
            var result = await _transactionLogService.GetUserTransactionsByCampaignIdAsync(campaignId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _transactionLogRepositoryMock.Verify(repo => repo.GetAll(It.IsAny<Expression<Func<TransactionLog, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetUserTransactionsByCampaignIdAsync_ShouldThrowException_WhenRepositoryThrowsException()
        {
            // Arrange
            var campaignId = 1;
            _transactionLogRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<TransactionLog, bool>>>())).Throws(new Exception("Test Exception"));

            // Act & Assert
            var result = await _transactionLogService.GetUserTransactionsByCampaignIdAsync(campaignId);

            Assert.Null(result);
            _transactionLogRepositoryMock.Verify(repo => repo.GetAll(It.IsAny<Expression<Func<TransactionLog, bool>>>()), Times.Once);
        }


        [Fact]
        public async Task GetMoneyOutTransactionsByCampaignIdAsync_ShouldReturnTransactions_WhenTransactionsExist()
        {
            // Arrange
            var campaignId = 1;
            var transactionLogs = new List<TransactionLog>
            {
                new TransactionLog { CampaignId = campaignId, Amount = -100, Campaign = new Campaign { Title = "Campaign 1" }, Description = "Test", TransactionDate = DateTime.Now, tid = "tid1" },
                new TransactionLog { CampaignId = campaignId, Amount = -200, Campaign = new Campaign { Title = "Campaign 1" }, Description = "Test", TransactionDate = DateTime.Now, tid = "tid2" }
            }.AsQueryable().BuildMock();

            _transactionLogRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<TransactionLog, bool>>>())).Returns(transactionLogs);

            // Act
            var result = await _transactionLogService.GetMoneyOutTransactionsByCampaignIdAsync(campaignId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _transactionLogRepositoryMock.Verify(repo => repo.GetAll(It.IsAny<Expression<Func<TransactionLog, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetUserTransactionsByUserIdAsync_ShouldReturnTransactions_WhenTransactionsExist()
        {
            // Arrange
            var userId = "user1";
            var transactionLogs = new List<TransactionLog>
            {
                new TransactionLog { Amount = 100, CreateQrCode = new CreateQrCode { UserId = userId, Id = 1, IsPaid = true }, Campaign = new Campaign { Title = "Campaign 1" }, Description = "Test", TransactionDate = DateTime.Now, tid = "tid1" },
                new TransactionLog { Amount = 200, CreateQrCode = new CreateQrCode { UserId = userId, Id = 2, IsPaid = false }, Campaign = new Campaign { Title = "Campaign 1" }, Description = "Test", TransactionDate = DateTime.Now, tid = "tid2" }
            }.AsQueryable().BuildMock();

            _transactionLogRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<TransactionLog, bool>>>())).Returns(transactionLogs);

            // Act
            var result = await _transactionLogService.GetUserTransactionsByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _transactionLogRepositoryMock.Verify(repo => repo.GetAll(It.IsAny<Expression<Func<TransactionLog, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetTotalMoneyOutByCampaignId_ShouldReturnTotalMoneyOut_WhenTransactionsExist()
        {
            // Arrange
            var campaignId = 1;
            var transactionLogs = new List<TransactionLog>
            {
                new TransactionLog { CampaignId = campaignId, Amount = -100 },
                new TransactionLog { CampaignId = campaignId, Amount = -200 }
            }.AsQueryable().BuildMock();

            _transactionLogRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<TransactionLog, bool>>>())).Returns(transactionLogs);

            // Act
            var result = await _transactionLogService.GetTotalMoneyOutByCampaignId(campaignId);

            // Assert
            Assert.Equal(300, result);
            _transactionLogRepositoryMock.Verify(repo => repo.GetAll(It.IsAny<Expression<Func<TransactionLog, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetTotalMoneyOutByCampaignId_ShouldReturnZero_WhenNoTransactionsExist()
        {
            // Arrange
            var campaignId = 1;
            var transactionLogs = new List<TransactionLog>().AsQueryable().BuildMock();

            _transactionLogRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<TransactionLog, bool>>>())).Returns(transactionLogs);

            // Act
            var result = await _transactionLogService.GetTotalMoneyOutByCampaignId(campaignId);

            // Assert
            Assert.Equal(0, result);
            _transactionLogRepositoryMock.Verify(repo => repo.GetAll(It.IsAny<Expression<Func<TransactionLog, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetTotalMoneyOutByCampaignId_ShouldReturnZero_WhenRepositoryThrowsException()
        {
            // Arrange
            var campaignId = 1;
            _transactionLogRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<TransactionLog, bool>>>())).Throws(new Exception("Test Exception"));

            // Act
            var result = await _transactionLogService.GetTotalMoneyOutByCampaignId(campaignId);

            // Assert
            Assert.Equal(0, result);
            _transactionLogRepositoryMock.Verify(repo => repo.GetAll(It.IsAny<Expression<Func<TransactionLog, bool>>>()), Times.Once);
        }


    }
}
