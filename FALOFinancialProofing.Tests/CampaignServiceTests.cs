using FALOFinancialProofing.DTOs.CampaignDTO;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using FALOFinancialProofing.Services.BankServices;
using FALOFinancialProofing.Services.ProjectServices;
using FALOFinancialProofing.Services.CampaignService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FALOFinancialProofing.Services;
using MockQueryable;
using FALOFinancialProofing.Services.SocialNetworkService;
using FALOFinancialProofing.Services.UserSDGServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using FALOFinancialProofing.Services.EmailService;
using FALOFinancialProofing.DTOs.BankDTO;

public class CampaignServiceTests
{
    private readonly Mock<IRepository<Campaign, int>> mockCampaignRepository;
    private readonly Mock<UserManager<User>> mockUserManager;
    private readonly Mock<SignInManager<User>> mockSignInManager;
    private readonly Mock<IOptionsMonitor<AppSetting>> mockOptionsMonitor;
    private readonly Mock<RoleManager<Role>> mockRoleManager;
    private readonly Mock<IEmailService> mockEmailService;
    private readonly Mock<LinkGenerator> mockLinkGenerator;
    private readonly Mock<IRepository<CampaignMember, int>> mockCampaignMemberRepository;
    private readonly Mock<ISocialNetworkService> mockSocialNetworkService;
    private readonly Mock<RoleService> mockRoleService;
    private readonly Mock<IUserSDGService> mockUserSDGService;
    private readonly Mock<IProjectService> mockProjectService;
    private readonly Mock<IBankService> mockBankService;
    private readonly Mock<ILogger<CampaignService>> mockLogger;
    private readonly Mock<AuthServices> mockAuthServices;
    private readonly CampaignService campaignService;

    public CampaignServiceTests()
    {
        mockCampaignRepository = new Mock<IRepository<Campaign, int>>();
        mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
        mockSignInManager = new Mock<SignInManager<User>>(mockUserManager.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<User>>(), null, null, null, null);
        mockOptionsMonitor = new Mock<IOptionsMonitor<AppSetting>>();
        mockRoleManager = new Mock<RoleManager<Role>>(Mock.Of<IRoleStore<Role>>(), null, null, null, null);
        mockEmailService = new Mock<IEmailService>();
        mockLinkGenerator = new Mock<LinkGenerator>();
        mockCampaignMemberRepository = new Mock<IRepository<CampaignMember, int>>();
        mockSocialNetworkService = new Mock<ISocialNetworkService>();
        mockUserSDGService = new Mock<IUserSDGService>();
        mockProjectService = new Mock<IProjectService>();
        mockBankService = new Mock<IBankService>();
        mockLogger = new Mock<ILogger<CampaignService>>();

        var mockRoleService = new Mock<RoleService>(mockRoleManager.Object, mockUserManager.Object);

        mockAuthServices = new Mock<AuthServices>(
           mockUserManager.Object,
           mockSignInManager.Object,
           mockOptionsMonitor.Object,
           mockRoleManager.Object,
           mockEmailService.Object,
           mockLinkGenerator.Object,
           mockCampaignMemberRepository.Object,
           mockSocialNetworkService.Object,
           mockRoleService.Object,
           mockUserSDGService.Object
       );

        campaignService = new CampaignService(
            mockCampaignRepository.Object,
            mockAuthServices.Object,
            mockProjectService.Object,
            mockLogger.Object,
            mockBankService.Object
        );
    }

    [Fact]
    public async Task CreateCampaignAsync_ShouldReturnCampaign_WhenSuccess()
    {
        // Arrange
        var createCampaignDTO = new CreateCampaignDTO { Title = "Test Campaign" };
        var campaign = new Campaign { Title = "Test Campaign" };
        mockCampaignRepository.Setup(repo => repo.InsertAsync(It.IsAny<Campaign>())).ReturnsAsync(campaign);

        // Act
        var result = await campaignService.CreateCampaignAsync(createCampaignDTO);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Campaign", result.Title);
    }

    [Fact]
    public async Task CreateCampaignAsync_ShouldReturnNull_WhenExceptionThrown()
    {
        // Arrange
        var createCampaignDTO = new CreateCampaignDTO { Title = "Test Campaign" };
        mockCampaignRepository.Setup(repo => repo.InsertAsync(It.IsAny<Campaign>())).ThrowsAsync(new Exception());

        // Act
        var result = await campaignService.CreateCampaignAsync(createCampaignDTO);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllCampaignsAsync_ShouldReturnCampaigns_WhenSuccess()
    {
        #region campaignInformation
        // Arrange
        //        var campaigns = new List<CampaignInformation>
        //{
        //            new CampaignInformation
        //            {
        //                FirstName = "John",
        //                LastName = "Doe",
        //                CampaignId = 1,
        //                ProjectId = 101,
        //                ProjectName = "Project Alpha",
        //                CreateBy = "user1",
        //                Title = "Campaign 1",
        //                Description = "Description for Campaign 1",
        //                DateOfCreation = DateTime.Now.AddDays(-10),
        //                FundTarget = 10000,
        //                Image = "image1.jpg",
        //                EndDate = DateTime.Now.AddDays(20),
        //                Address = "123 Main St",
        //                IsActive = true,
        //                BankId = 1,
        //                Status = "Active",
        //                UpdateLog = "Initial creation",
        //                TotalMoneyEarned = 5000
        //            },
        //            new CampaignInformation
        //            {
        //                FirstName = "Jane",
        //                LastName = "Smith",
        //                CampaignId = 2,
        //                ProjectId = 102,
        //                ProjectName = "Project Beta",
        //                CreateBy = "user2",
        //                Title = "Campaign 2",
        //                Description = "Description for Campaign 2",
        //                DateOfCreation = DateTime.Now.AddDays(-5),
        //                FundTarget = 20000,
        //                Image = "image2.jpg",
        //                EndDate = DateTime.Now.AddDays(25),
        //                Address = "456 Elm St",
        //                IsActive = true,
        //                BankId = 2,
        //                Status = "Active",
        //                UpdateLog = "Initial creation",
        //                TotalMoneyEarned = 10000
        //            }
        //        }.AsQueryable().BuildMock(); 
        #endregion

        var campaigns = new List<Campaign>
    {
        new Campaign
        {
            Id = 1,
            User = new User { FirstName = "John", LastName = "Doe" },
            ProjectId = 101,
            Project = new Project { ProjectName = "Project Alpha" },
            CreateBy = "user1",
            Title = "Campaign 1",
            Description = "Description for Campaign 1",
            DateOfCreation = DateTime.Now.AddDays(-10),
            FundTarget = 10000,
            Image = "image1.jpg",
            EndDate = DateTime.Now.AddDays(20),
            Address = "123 Main St",
            IsActive = true,
            BankId = 1,
            Status = RequestStatus.Rejected,
            UpdateLog = "Initial creation",
            TransactionLogs = new List<TransactionLog>
            {
                new TransactionLog { Amount = 5000 }
            }
        },
        new Campaign
        {
            Id = 2,
            User = new User { FirstName = "Jane", LastName = "Smith" },
            ProjectId = 102,
            Project = new Project { ProjectName = "Project Beta" },
            CreateBy = "user2",
            Title = "Campaign 2",
            Description = "Description for Campaign 2",
            DateOfCreation = DateTime.Now.AddDays(-5),
            FundTarget = 20000,
            Image = "image2.jpg",
            EndDate = DateTime.Now.AddDays(25),
            Address = "456 Elm St",
            IsActive = true,
            BankId = 2,
            Status = "Active",
            UpdateLog = "Initial creation",
            TransactionLogs = new List<TransactionLog>
            {
                new TransactionLog { Amount = 10000 }
            }
        }
    }.AsQueryable().BuildMock();
        mockCampaignRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Campaign, bool>>>())).Returns(campaigns);

        // Act
        var result = await campaignService.GetAllCampaignsAsync(new DefaultHttpContext().Request);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Campaign 2", result[0].Title);
    }

    [Fact]
    public async Task GetAllCampaignsAsync_ShouldReturnEmptyList_WhenExceptionThrown()
    {
        // Arrange
        mockCampaignRepository.Setup(repo => repo.GetAll(null)).Throws(new Exception());

        // Act
        var result = await campaignService.GetAllCampaignsAsync(new DefaultHttpContext().Request);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetCampaignByIdAsync_ShouldReturnCampaign_WhenFound()
    {
        // Arrange
        var campaign = new Campaign { Id = 1, Title = "Test Campaign" };
        mockCampaignRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Campaign, bool>>>())).ReturnsAsync(campaign);

        // Act
        var result = await campaignService.GetCampaignByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Campaign", result.Title);
    }

    [Fact]
    public async Task GetCampaignByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        mockCampaignRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Campaign, bool>>>())).ReturnsAsync((Campaign)null);

        // Act
        var result = await campaignService.GetCampaignByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateCampaignAsync_ShouldReturnTrue_WhenSuccess()
    {
        // Arrange
        var updateCampaignDTO = new UpdateCampaignDTO { Id = 1, Title = "Updated Campaign" };
        var campaign = new Campaign { Id = 1, Title = "Test Campaign" };
        mockAuthServices.Setup(auth => auth.CheckRole(It.IsAny<string>(), It.IsAny<string>(), AppRole.Admin, It.IsAny<StringBuilder>())).ReturnsAsync(true);
        //mockAuthServices.Setup(auth => auth.CheckRole(It.IsAny<string>(), It.IsAny<string>(), AppRole.ProjectManagementBoard, It.IsAny<StringBuilder>())).ReturnsAsync(false);

        mockBankService.Setup(bank => bank.GetBankByIdAsync(It.IsAny<int>())).ReturnsAsync(new FALOFinancialProofing.DTOs.BankDTO.BankInformation()
        {
            Id = 1
        });
        mockCampaignRepository.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync(campaign);

        mockCampaignRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Campaign>())).ReturnsAsync(true);

        // Act
        var result = await campaignService.UpdateCampaignAsync(updateCampaignDTO, new StringBuilder());

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateCampaignAsync_ShouldReturnFalse_WhenExceptionThrown()
    {
        // Arrange
        var updateCampaignDTO = new UpdateCampaignDTO { Id = 1, Title = "Updated Campaign" };
        mockCampaignRepository.Setup(repo => repo.Get(It.IsAny<int>())).ThrowsAsync(new Exception());

        // Act
        var result = await campaignService.UpdateCampaignAsync(updateCampaignDTO, new StringBuilder());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteCampaignByIdAsync_ShouldReturnTrue_WhenSuccess()
    {
        // Arrange
        var campaign = new Campaign { Id = 1, Title = "Test Campaign" };
        mockCampaignRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Campaign, bool>>>())).ReturnsAsync(campaign);
        mockCampaignRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Campaign>())).ReturnsAsync(true);

        // Act
        var result = await campaignService.DeleteCampaignByIdAsync(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteCampaignByIdAsync_ShouldReturnFalse_WhenExceptionThrown()
    {
        // Arrange
        mockCampaignRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Campaign, bool>>>())).ThrowsAsync(new Exception());

        // Act
        var result = await campaignService.DeleteCampaignByIdAsync(1);

        // Assert
        Assert.False(result);
    }
}
