using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using FALOFinancialProofing.Services.SocialNetworkService;
using Microsoft.AspNetCore.Identity;
using MockQueryable;
using Moq;
using System.Linq.Expressions;

public class SocialNetworkServiceTests
{
    private readonly Mock<IRepository<SocialNetwork, int>> mockSocialNetworksRepository;
    private readonly Mock<UserManager<User>> mockUserManager;
    private readonly SocialNetworkService socialNetworkService;

    public SocialNetworkServiceTests()
    {
        mockSocialNetworksRepository = new Mock<IRepository<SocialNetwork, int>>();
        mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
        socialNetworkService = new SocialNetworkService(mockSocialNetworksRepository.Object, mockUserManager.Object);
    }

    [Fact]
    public async Task CreateSocialNetworkAsync_ShouldReturnSocialNetwork_WhenSuccess()
    {
        var request = new SocialNetworkRequest { SocialNetworksLink = "http://example.com", UserId = "1" };
        var socialNetwork = new SocialNetwork { Id = 1, SocialNetworksLink = "http://example.com", UserId = "1" };

        mockSocialNetworksRepository.Setup(repo => repo.InsertAsync(It.IsAny<SocialNetwork>())).ReturnsAsync(socialNetwork);

        var result = await socialNetworkService.CreateSocialNetworkAsync(request);

        Assert.NotNull(result);
        Assert.Equal(socialNetwork.Id, result.Id);
    }

    [Fact]
    public async Task CreateSocialNetworkAsync_ShouldReturnNull_WhenExceptionThrown()
    {
        var request = new SocialNetworkRequest { SocialNetworksLink = "http://example.com", UserId = "1" };

        mockSocialNetworksRepository.Setup(repo => repo.InsertAsync(It.IsAny<SocialNetwork>())).ThrowsAsync(new Exception());

        var result = await socialNetworkService.CreateSocialNetworkAsync(request);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteSocialNetworkByIdAsync_ShouldReturnTrue_WhenSuccess()
    {
        var socialNetwork = new SocialNetwork { Id = 1 };

        mockSocialNetworksRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<SocialNetwork, bool>>>())).ReturnsAsync(socialNetwork);
        mockSocialNetworksRepository.Setup(repo => repo.DeleteAsync(It.IsAny<SocialNetwork>())).ReturnsAsync(true);

        var result = await socialNetworkService.DeleteSocialNetworkByIdAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteSocialNetworkByIdAsync_ShouldReturnFalse_WhenNotFound()
    {
        mockSocialNetworksRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<SocialNetwork, bool>>>())).ReturnsAsync((SocialNetwork)null);

        var result = await socialNetworkService.DeleteSocialNetworkByIdAsync(1);

        Assert.False(result);
    }
    [Fact]
    public async Task DeleteSocialNetworkByIdAsync_ShouldReturnFalse_WhenExceptionThrown()
    {
        mockSocialNetworksRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<SocialNetwork, bool>>>())).
            ThrowsAsync(new Exception());

        var result = await socialNetworkService.DeleteSocialNetworkByIdAsync(1);

        Assert.False(result);
    }

    [Fact]
    public async Task GetAllSocialNetworksAsync_ShouldReturnListOfSocialNetworks()
    {
        var socialNetworks = new List<SocialNetwork> { new SocialNetwork { Id = 1 }, new SocialNetwork { Id = 2 } };

        mockSocialNetworksRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<SocialNetwork, bool>>>())).Returns(socialNetworks.AsQueryable().BuildMock());

        var result = await socialNetworkService.GetAllSocialNetworksAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAllSocialNetworksAsync_ShouldReturnEmptyList_WhenExceptionThrown()
    {
        mockSocialNetworksRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<SocialNetwork, bool>>>())).Throws(new Exception());

        var result = await socialNetworkService.GetAllSocialNetworksAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetSocialNetworkByIdAsync_ShouldReturnSocialNetwork_WhenFound()
    {
        var socialNetwork = new SocialNetwork { Id = 1 };

        mockSocialNetworksRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<SocialNetwork, bool>>>())).ReturnsAsync(socialNetwork);

        var result = await socialNetworkService.GetSocialNetworkByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(socialNetwork.Id, result.Id);
    }

    [Fact]
    public async Task GetSocialNetworkByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        mockSocialNetworksRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<SocialNetwork, bool>>>())).ReturnsAsync((SocialNetwork)null);

        var result = await socialNetworkService.GetSocialNetworkByIdAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateSocialNetworkAsync_ShouldReturnTrue_WhenSuccess()
    {
        var request = new SocialNetworkRequest { Id = 1, SocialNetworksLink = "http://example.com", UserId = "1" };
        var socialNetwork = new SocialNetwork { Id = 1, SocialNetworksLink = "http://example.com", UserId = "1" };

        mockSocialNetworksRepository.Setup(repo => repo.UpdateAsync(It.IsAny<SocialNetwork>())).ReturnsAsync(true);

        var result = await socialNetworkService.UpdateSocialNetworkAsync(request);

        Assert.True(result);
    }

    [Fact]
    public async Task UpdateSocialNetworkAsync_ShouldReturnFalse_WhenExceptionThrown()
    {
        var request = new SocialNetworkRequest { Id = 1, SocialNetworksLink = "http://example.com", UserId = "1" };

        mockSocialNetworksRepository.Setup(repo => repo.UpdateAsync(It.IsAny<SocialNetwork>())).ThrowsAsync(new Exception());

        var result = await socialNetworkService.UpdateSocialNetworkAsync(request);

        Assert.False(result);
    }


    [Fact]
    public async Task UpdateSocialNetworkAsync_ShouldReturnTrue_WhenSuccess1()
    {
        var socialNetwork = new SocialNetwork { Id = 1, SocialNetworksLink = "http://example.com", UserId = "1" };

        mockSocialNetworksRepository.Setup(repo => repo.UpdateAsync(It.IsAny<SocialNetwork>())).ReturnsAsync(true);

        var result = await socialNetworkService.UpdateSocialNetworkAsync(socialNetwork);

        Assert.True(result);
    }

    [Fact]
    public async Task UpdateSocialNetworkAsync_ShouldReturnFalse_WhenExceptionThrown1()
    {
        var socialNetwork = new SocialNetwork { Id = 1, SocialNetworksLink = "http://example.com", UserId = "1" };

        mockSocialNetworksRepository.Setup(repo => repo.UpdateAsync(It.IsAny<SocialNetwork>())).ThrowsAsync(new Exception());

        var result = await socialNetworkService.UpdateSocialNetworkAsync(socialNetwork);

        Assert.False(result);
    }
}
