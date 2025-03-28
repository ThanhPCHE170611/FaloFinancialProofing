using FALOFinancialProofing.DTOs.CreateQrCodeDTO;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using FALOFinancialProofing.Services;
using FALOFinancialProofing.Services.CampaignService;
using FALOFinancialProofing.Services.CreateQrCodeServices;
using FALOFinancialProofing.Services.EmailService;
using FALOFinancialProofing.Services.SocialNetworkService;
using FALOFinancialProofing.Services.UserSDGServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Moq;
using System.Text;
using Xunit;

public class CreateQrCodeServiceTests
{
    private readonly Mock<IRepository<CreateQrCode, int>> _mockCreateQrCodeRepository;
    private readonly Mock<AuthServices> _mockAuthServices;
    private readonly Mock<ICampaignService> _mockCampaignService;
    private readonly CreateQrCodeService _createQrCodeService;


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

    public CreateQrCodeServiceTests()
    {
        _mockCreateQrCodeRepository = new Mock<IRepository<CreateQrCode, int>>();
        mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
        mockSignInManager = new Mock<SignInManager<User>>(mockUserManager.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<User>>(), null, null, null, null);
        mockOptionsMonitor = new Mock<IOptionsMonitor<AppSetting>>();
        mockRoleManager = new Mock<RoleManager<Role>>(Mock.Of<IRoleStore<Role>>(), null, null, null, null);
        mockEmailService = new Mock<IEmailService>();
        mockLinkGenerator = new Mock<LinkGenerator>();
        mockCampaignMemberRepository = new Mock<IRepository<CampaignMember, int>>();
        mockSocialNetworkService = new Mock<ISocialNetworkService>();
        mockUserSDGService = new Mock<IUserSDGService>();
        mockRoleService = new Mock<RoleService>(mockRoleManager.Object, mockUserManager.Object);

        _mockAuthServices = new Mock<AuthServices>(
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
        _mockCampaignService = new Mock<ICampaignService>();
        _createQrCodeService = new CreateQrCodeService(_mockCreateQrCodeRepository.Object, _mockAuthServices.Object, _mockCampaignService.Object);
    }

    [Fact]
    public async Task CreateQrCodeAsync_ShouldReturnTrue_WhenQrCodeIsCreatedSuccessfully()
    {
        // Arrange
        var createQrCode = new CreateQrCode { Id = 1 };
        _mockCreateQrCodeRepository.Setup(repo => repo.InsertAsync(createQrCode)).ReturnsAsync(createQrCode);

        // Act
        var result = await _createQrCodeService.CreateQrCodeAsync(createQrCode);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CreateQrCodeAsync_ShouldReturnFalse_WhenQrCodeIsNull()
    {
        // Act
        var result = await _createQrCodeService.CreateQrCodeAsync(null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetQrCodeByIdAsync_ShouldReturnQrCode_WhenFound()
    {
        // Arrange
        var createQrCode = new CreateQrCode { Id = 1 };
        _mockCreateQrCodeRepository.Setup(repo => repo.Get(1)).ReturnsAsync(createQrCode);

        // Act
        var result = await _createQrCodeService.GetQrCodeByIdAsync(1);

        // Assert
        Assert.Equal(createQrCode, result);
    }

    [Fact]
    public async Task GetQrCodeByIdAsync_ShouldLogException_WhenQrCodeNotFound()
    {
        // Arrange
        _mockCreateQrCodeRepository.Setup(repo => repo.Get(1)).ThrowsAsync(new Exception("CreateQrCode not found"));

        // Act
        var result = await _createQrCodeService.GetQrCodeByIdAsync(1);

        // Assert
        Assert.Null(result);
    }


    [Fact]
    public async Task ValidateQrCodeCreate_ShouldReturnTrue_WhenQrCodeRequestIsValid()
    {
        // Arrange
        var createQrCodeRequest = new CreateQrCodeRequest { UserId = "user1", CampaignId = 1 };
        var message = new StringBuilder();
        _mockAuthServices.Setup(auth => auth.CheckUserExist("user1", message)).ReturnsAsync(true);
        _mockCampaignService.Setup(service => service.GetCampaignByCampaignIdAsync(1)).ReturnsAsync(new Campaign());

        // Act
        var result = await _createQrCodeService.ValidateQrCodeCreate(createQrCodeRequest, message);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ValidateQrCodeCreate_ShouldReturnFalse_WhenUserDoesNotExist()
    {
        // Arrange
        var createQrCodeRequest = new CreateQrCodeRequest { UserId = "user1", CampaignId = 1 };
        var message = new StringBuilder();
        _mockAuthServices.Setup(auth => auth.CheckUserExist("user1", message)).ReturnsAsync(false);

        // Act
        var result = await _createQrCodeService.ValidateQrCodeCreate(createQrCodeRequest, message);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ValidateQrCodeCreate_ShouldReturnFalse_WhenCampaignNotFound()
    {
        // Arrange
        var createQrCodeRequest = new CreateQrCodeRequest { UserId = "user1", CampaignId = 1 };
        var message = new StringBuilder();
        _mockAuthServices.Setup(auth => auth.CheckUserExist("user1", message)).ReturnsAsync(true);
        _mockCampaignService.Setup(service => service.GetCampaignByCampaignIdAsync(1)).ReturnsAsync((Campaign)null);

        // Act
        var result = await _createQrCodeService.ValidateQrCodeCreate(createQrCodeRequest, message);

        // Assert
        Assert.False(result);
        Assert.Contains("Campaign not found!", message.ToString());
    }
}
