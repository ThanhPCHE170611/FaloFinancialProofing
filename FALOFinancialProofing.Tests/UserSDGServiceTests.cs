using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using FALOFinancialProofing.Services.UserSDGServices;
using MockQueryable;
using Moq;
using System.Linq.Expressions;

public class UserSDGServiceTests
{
    private readonly Mock<IRepository<UserSDG, int>> _mockUserSDGRepository;
    private readonly UserSDGService _userSDGService;

    public UserSDGServiceTests()
    {
        _mockUserSDGRepository = new Mock<IRepository<UserSDG, int>>();
        _userSDGService = new UserSDGService(_mockUserSDGRepository.Object);
    }

    [Fact]
    public async Task CreateUserSDGAsync_ShouldReturnUserSDG_WhenSuccess()
    {
        // Arrange
        var userSDG = new UserSDG { Id = 1, UserId = "user1", SDGId = 1 };
        _mockUserSDGRepository.Setup(repo => repo.InsertAsync(It.IsAny<UserSDG>())).ReturnsAsync(userSDG);

        // Act
        var result = await _userSDGService.CreateUserSDGAsync(userSDG);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userSDG.Id, result.Id);
    }

    [Fact]
    public async Task CreateUserSDGAsync_ShouldReturnNull_WhenExceptionIsThrown()
    {
        // Arrange
        var userSdg = new UserSDG { Id = 1, UserId = "user1", SDGId = 1 };
        _mockUserSDGRepository.Setup(repo => repo.InsertAsync(userSdg)).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _userSDGService.CreateUserSDGAsync(userSdg);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteUserSDGAsync_ShouldReturnTrue_WhenSuccess()
    {
        // Arrange
        var userSDG = new UserSDG { Id = 1, UserId = "user1", SDGId = 1 };
        _mockUserSDGRepository.Setup(repo => repo.DeleteAsync(It.IsAny<UserSDG>())).ReturnsAsync(true);

        // Act
        var result = await _userSDGService.DeleteUserSDGAsync(userSDG);

        // Assert
        Assert.True(result);
    }


    [Fact]
    public async Task DeleteUserSDGAsync_ShouldReturnFalse_WhenFailure()
    {
        // Arrange
        var userSDG = new UserSDG { Id = 1, UserId = "user1", SDGId = 1 };
        _mockUserSDGRepository.Setup(repo => repo.DeleteAsync(It.IsAny<UserSDG>())).ReturnsAsync(false);

        // Act
        var result = await _userSDGService.DeleteUserSDGAsync(userSDG);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteUserSDGAsync_ShouldHandleException()
    {
        // Arrange
        var userSDG = new UserSDG { Id = 1, UserId = "user1", SDGId = 1 };
        _mockUserSDGRepository.Setup(repo => repo.DeleteAsync(It.IsAny<UserSDG>())).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _userSDGService.DeleteUserSDGAsync(userSDG);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetUserSDGByUserIdAndSdgIdAsync_ShouldReturnUserSDG_WhenFound()
    {
        // Arrange
        var userSDG = new UserSDG { Id = 1, UserId = "user1", SDGId = 1 };
        _mockUserSDGRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<UserSDG, bool>>>())).ReturnsAsync(userSDG);

        // Act
        var result = await _userSDGService.GetUserSDGByUserIdAndSdgIdAsync("user1", 1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userSDG.Id, result.Id);
    }

    [Fact]
    public async Task GetUserSDGByUserIdAndSdgIdAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        _mockUserSDGRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<UserSDG, bool>>>())).ReturnsAsync((UserSDG)null);

        // Act
        var result = await _userSDGService.GetUserSDGByUserIdAndSdgIdAsync("user1", 1);

        // Assert
        Assert.Null(result);
    }
    [Fact]
    public async Task GetUserSDGsByUserIdAsync_ShouldReturnUserSDGInformationList_WhenFound()
    {
        // Arrange
        var userSDGs = new List<UserSDG>
        {
            new UserSDG { Id = 1, UserId = "user1", SDGId = 1, SDG = new SDG { Id = 1, SDGName = "SDG1" } },
            new UserSDG { Id = 2, UserId = "user1", SDGId = 2, SDG = new SDG { Id = 2, SDGName = "SDG2" } }
        }.AsQueryable().BuildMock();

        _mockUserSDGRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<UserSDG, bool>>>())).Returns(userSDGs);

        // Act
        var result = await _userSDGService.GetUserSDGsByUserIdAsync("user1");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("SDG1", result[0].sDGInformation.SDGName);
        Assert.Equal("SDG2", result[1].sDGInformation.SDGName);
    }

    [Fact]
    public async Task GetUserSDGsByUserIdAsync_ShouldReturnEmptyList_WhenNotFound()
    {
        // Arrange
        var userSDGs = new List<UserSDG>().AsQueryable().BuildMock();

        _mockUserSDGRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<UserSDG, bool>>>())).Returns(userSDGs);

        // Act
        var result = await _userSDGService.GetUserSDGsByUserIdAsync("user1");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUserSDGsByUserIdAsync_ShouldHandleException()
    {
        // Arrange
        _mockUserSDGRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<UserSDG, bool>>>())).Throws(new Exception("Database error"));

        // Act
        var result = await _userSDGService.GetUserSDGsByUserIdAsync("user1");

        // Assert
        Assert.Null(result);
    }


}
