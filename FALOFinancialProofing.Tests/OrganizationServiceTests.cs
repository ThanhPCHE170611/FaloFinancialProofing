using FALOFinancialProofing.DTOs.OrganizationDTO;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using FALOFinancialProofing.Services;
using FALOFinancialProofing.Services.BankServices;
using FALOFinancialProofing.Services.CampaignService;
using FALOFinancialProofing.Services.EmailService;
using FALOFinancialProofing.Services.OrganizationMemberServices;
using FALOFinancialProofing.Services.OrganizationServices;
using FALOFinancialProofing.Services.ProjectServices;
using FALOFinancialProofing.Services.SocialNetworkService;
using FALOFinancialProofing.Services.UserSDGServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockQueryable;
using Moq;
using System.Linq.Expressions;
using System.Text;

public class OrganizationServiceTests
{
    private readonly Mock<IRepository<Organization, int>> _mockOrganizationRepository;
    private readonly Mock<AuthServices> _mockAuthServices;
    private readonly Mock<IOrganizationMemberService> _mockOrganizationMemberService;
    private readonly OrganizationService _organizationService;



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

    public OrganizationServiceTests()
    {
        _mockOrganizationRepository = new Mock<IRepository<Organization, int>>();
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
        _mockOrganizationMemberService = new Mock<IOrganizationMemberService>();

        _organizationService = new OrganizationService(
            _mockOrganizationRepository.Object,
            _mockAuthServices.Object,
            _mockOrganizationMemberService.Object
        );
    }

    [Fact]
    public async Task CreateOrganizationAsync_ShouldReturnOrganization_WhenSuccess()
    {
        // Arrange
        var createOrganization = new CreateOrganization { Name = "Test Organization" };
        var message = new StringBuilder();
        var organization = new Organization { Name = "Test Organization" };

        _mockOrganizationRepository.Setup(repo => repo.InsertAsync(It.IsAny<Organization>())).ReturnsAsync(organization);

        // Act
        var result = await _organizationService.CreateOrganizationAsync(createOrganization, message);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Organization", result.Name);
        Assert.Contains("Create Organization Successfully!", message.ToString());
    }

    [Fact]
    public async Task CreateOrganizationAsync_ShouldReturnNull_WhenInsertFails()
    {
        // Arrange
        var createOrganization = new CreateOrganization { Name = "Test Organization" };
        var message = new StringBuilder();

        _mockOrganizationRepository.Setup(repo => repo.InsertAsync(It.IsAny<Organization>())).ThrowsAsync(new Exception("Insert failed"));
        //Act
        var result = await _organizationService.CreateOrganizationAsync(createOrganization, message);

        // Assert
        Assert.Null(result);
        Assert.Contains("Create Organization Failed!", message.ToString());
    }


    [Fact]
    public async Task ValidateCreateOrganizationAsync_ShouldReturnTrue_WhenUserExists()
    {
        // Arrange
        var createOrganization = new CreateOrganization { UserId = "user1" };
        var message = new StringBuilder();

        _mockAuthServices.Setup(auth => auth.CheckUserExist(It.IsAny<string>(), It.IsAny<StringBuilder>())).ReturnsAsync(true);

        // Act
        var result = await _organizationService.ValidateCreateOrganizationAsync(createOrganization, message);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ValidateCreateOrganizationAsync_ShouldReturnFalse_WhenUserDoesNotExist()
    {
        // Arrange
        var createOrganization = new CreateOrganization { UserId = "user1" };
        var message = new StringBuilder();

        _mockAuthServices.Setup(auth => auth.CheckUserExist(It.IsAny<string>(), It.IsAny<StringBuilder>())).ReturnsAsync(false);

        // Act
        var result = await _organizationService.ValidateCreateOrganizationAsync(createOrganization, message);

        // Assert
        Assert.False(result);
        Assert.Contains("Exception", message.ToString());
    }

    [Fact]
    public async Task ValidateCreateOrganizationAsync_ShouldReturnFalse_WhenExceptionThrown()
    {
        // Arrange
        var createOrganization = new CreateOrganization { UserId = "user1" };
        var message = new StringBuilder();

        _mockAuthServices.Setup(auth => auth.CheckUserExist(It.IsAny<string>(), It.IsAny<StringBuilder>())).ThrowsAsync(new Exception("Some error"));

        // Act
        var result = await _organizationService.ValidateCreateOrganizationAsync(createOrganization, message);

        // Assert
        Assert.False(result);
        Assert.Contains("Some error", message.ToString());
    }

    [Fact]
    public async Task GetOrganizationByIdAsync_ShouldReturnOrganization_WhenFound()
    {
        // Arrange
        var organization = new Organization { Id = 1, Name = "Test Organization" };

        _mockOrganizationRepository.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync(organization);

        // Act
        var result = await _organizationService.GetOrganizationByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetOrganizationByIdAsync_ReturnNull_WhenThrowException()
    {
        // Arrange
        var organizationId = 1;
        var expectedExceptionMessage = "Repository error";
        _mockOrganizationRepository.Setup(repo => repo.Get(organizationId)).ThrowsAsync(new Exception(expectedExceptionMessage));

        // Act
        var result = await _organizationService.GetOrganizationByIdAsync(organizationId);

        // Assert
        Assert.Null(result);
        // You can also verify that the exception message was logged if you have a logging mechanism
    }
    [Fact]
    public async Task GetOrganizationByIdAsync_ReturnNull_WhenGetReturnNull()
    {
        // Arrange
        var organizationId = 1;
        var expectedExceptionMessage = "Repository error";
        _mockOrganizationRepository.Setup(repo => repo.Get(organizationId)).ReturnsAsync((Organization)null);

        // Act
        var result = await _organizationService.GetOrganizationByIdAsync(organizationId);

        // Assert
        Assert.Null(result);
        // You can also verify that the exception message was logged if you have a logging mechanism
    }
    [Fact]
    public async Task GetAllOrganizationsAsync_ShouldReturnOrganizations()
    {
        // Arrange
        var organizations = new List<Organization>
        {
            new Organization { Id = 1, Name = "Org1" },
            new Organization { Id = 2, Name = "Org2" }
        }.AsQueryable().BuildMock();

        _mockOrganizationRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Organization, bool>>>())).Returns(organizations);

        // Act
        var result = await _organizationService.GetAllOrganizationsAsync(new DefaultHttpContext().Request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAllOrganizationsAsync_ReturnNull_WhenThrowException()
    {
        // Arrange
        var organizations = new List<Organization>
        {
            new Organization { Id = 1, Name = "Org1" },
            new Organization { Id = 2, Name = "Org2" }
        }.AsQueryable().BuildMock();

        _mockOrganizationRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Organization, bool>>>())).Throws(new Exception("Error"));

        // Act
        var result = await _organizationService.GetAllOrganizationsAsync(new DefaultHttpContext().Request);

        // Assert
        Assert.Null(result);
    }
    [Fact]
    public async Task UpdateOrganizationAsync_ShouldReturnTrue_WhenSuccess()
    {
        // Arrange
        var organization = new Organization { Id = 1, Name = "Updated Organization" };

        _mockOrganizationRepository.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync(organization);
        _mockOrganizationRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Organization>())).ReturnsAsync(true);
        // Act
        var result = await _organizationService.UpdateOrganizationAsync(organization);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateOrganizationAsync_ShouldReturnFalse_WhenNotFound()
    {
        // Arrange
        var organization = new Organization { Id = 1, Name = "Updated Organization" };

        _mockOrganizationRepository.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync((Organization)null);

        // Act
        var result = await _organizationService.UpdateOrganizationAsync(organization);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteOrganizationAsync_ShouldReturnTrue_WhenSuccess()
    {
        // Arrange
        var organization = new Organization { Id = 1, Name = "Test Organization" };

        _mockOrganizationRepository.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync(organization);
        _mockOrganizationRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Organization>())).ReturnsAsync(true);

        // Act
        var result = await _organizationService.DeleteOrganizationAsync(1);

        // Assert
        Assert.True(result);
    }
    [Fact]
    public async Task DeleteOrganizationAsync_ShouldReturnFalse_WhenGetNull()
    {
        // Arrange
        var organization = new Organization { Id = 1, Name = "Test Organization" };

        _mockOrganizationRepository.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync((Organization)null);
        // Act
        var result = await _organizationService.DeleteOrganizationAsync(1);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ValidateOrganizationUpdateAsync_ShouldReturnFalse_WhenOrganizationNotFound()
    {
        // Arrange
        var updateOrganization = new UpdateOrganization { Id = 1, UserId = "user1" };
        var message = new StringBuilder();

        _mockOrganizationRepository.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync((Organization)null);

        // Act
        var result = await _organizationService.ValidateOrganizationUpdateAsync(updateOrganization, message);

        // Assert
        Assert.False(result);
        Assert.Contains("Organization not found!", message.ToString());
    }
    [Fact]
    public async Task ValidateOrganizationUpdateAsync_ShouldReturnFalse_WhenUserHasNoPermission()
    {
        // Arrange
        var updateOrganization = new UpdateOrganization { Id = 1, UserId = "user1" };
        var message = new StringBuilder();
        var organization = new Organization { Id = 1 };

        _mockOrganizationRepository.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync(organization);
        _mockOrganizationMemberService.Setup(service => service.GetOrganizationMemberByUserIdAndOrganizationIdAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync((OrganizationMember)null);
        _mockAuthServices.Setup(auth => auth.CheckUserInRole(It.IsAny<string>(), AppRole.Admin, It.IsAny<StringBuilder>())).ReturnsAsync(false);
        _mockAuthServices.Setup(auth => auth.CheckUserInRole(It.IsAny<string>(), AppRole.ProjectManagementBoard, It.IsAny<StringBuilder>())).ReturnsAsync(false);

        // Act
        var result = await _organizationService.ValidateOrganizationUpdateAsync(updateOrganization, message);

        // Assert
        Assert.False(result);
        Assert.Contains("You don't have permission to update organization!", message.ToString());
    }

    [Fact]
    public async Task ValidateOrganizationUpdateAsync_ShouldReturnFalse_WhenLogoIsTooLarge()
    {
        // Arrange
        var updateOrganization = new UpdateOrganization { Id = 1, UserId = "user1", LogoFile = new FormFile(null, 0, FileHelper.OrganizationImageMaxFileSize + 1, null, null) };
        var message = new StringBuilder();
        var organization = new Organization { Id = 1 };

        _mockOrganizationRepository.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync(organization);
        _mockOrganizationMemberService.Setup(service => service.GetOrganizationMemberByUserIdAndOrganizationIdAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(new OrganizationMember());
        _mockAuthServices.Setup(auth => auth.CheckUserInRole(It.IsAny<string>(), AppRole.Admin, It.IsAny<StringBuilder>())).ReturnsAsync(true);
        _mockAuthServices.Setup(auth => auth.CheckUserInRole(It.IsAny<string>(), AppRole.ProjectManagementBoard, It.IsAny<StringBuilder>())).ReturnsAsync(false);

        // Act
        var result = await _organizationService.ValidateOrganizationUpdateAsync(updateOrganization, message);

        // Assert
        Assert.False(result);
        Assert.Contains("Logo is too large", message.ToString());
    }

    [Fact]
    public async Task UpdateOrganizationAsync_ShouldReturnTrue_WhenSuccess1()
    {
        // Arrange
        var updateOrganization = new UpdateOrganization
        {
            Id = 1,
            Name = "New Name",
            Main_office = "New Office",
            Representative = "New Representative",
            PhoneNumber = "123456789",
            Email = "newemail@example.com",
            LogoFile = null,
            Description = "New Description",
            Vision = "New Vision",
            Mission = "New Mission",
            CoreValue = "New Core Value",
            MainActivity = "New Main Activity",
            Interests = "New Interests",
            VolunteerExperience = "New Volunteer Experience",
            VolunteerObjectives = "New Volunteer Objectives",
            Bio = "New Bio"
        };
        var message = new StringBuilder();
        var organization = new Organization { Id = 1 };

        _mockOrganizationRepository.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync(organization);
        _mockOrganizationRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Organization>())).ReturnsAsync(true);
        // Act
        var result = await _organizationService.UpdateOrganizationAsync(updateOrganization, message);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateOrganizationAsync_ShouldReturnFalse_WhenNotFound1()
    {
        // Arrange
        var updateOrganization = new UpdateOrganization { Id = 1 };
        var message = new StringBuilder();

        _mockOrganizationRepository.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync((Organization)null);

        // Act
        var result = await _organizationService.UpdateOrganizationAsync(updateOrganization, message);

        // Assert
        Assert.False(result);
        Assert.Contains("Object reference not set to an instance of an object.", message.ToString());
    }

    [Fact]
    public async Task UpdateOrganizationAsync_ShouldReturnFalse_WhenExceptionThrown()
    {
        // Arrange
        var updateOrganization = new UpdateOrganization { Id = 1 };
        var message = new StringBuilder();
        var organization = new Organization { Id = 1 };

        _mockOrganizationRepository.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync(organization);
        _mockOrganizationRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Organization>())).ThrowsAsync(new Exception("Update failed"));

        // Act
        var result = await _organizationService.UpdateOrganizationAsync(updateOrganization, message);

        // Assert
        Assert.False(result);
        Assert.Contains("Update failed", message.ToString());
    }
}
