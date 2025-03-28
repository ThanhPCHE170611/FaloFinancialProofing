using FALOFinancialProofing;
using FALOFinancialProofing.DTOs.MoveNextCampaignStatusRequestDTO;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using FALOFinancialProofing.Services;
using FALOFinancialProofing.Services.EmailService;
using FALOFinancialProofing.Services.MoveNextCampaignStatusRequestServices;
using FALOFinancialProofing.Services.SocialNetworkService;
using FALOFinancialProofing.Services.UserSDGServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using MockQueryable;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

public class MoveNextCampaignStatusRequestServiceTests
{
    private readonly Mock<IRepository<MoveNextCampaignStatusRequest, int>> _mockMoveNextCampaignStatusRequestRepository;
    private readonly Mock<IRepository<Campaign, int>> _mockCampaignRepository;
    private readonly Mock<AuthServices> _mockAuthServices;
    private readonly Mock<IRepository<CampaignMember, int>> _mockCampaignMemberRepository;
    private readonly MoveNextCampaignStatusRequestService _service;
    private readonly Mock<MoveNextCampaignStatusRequestService> _MockService;

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

    public MoveNextCampaignStatusRequestServiceTests()
    {

        _mockMoveNextCampaignStatusRequestRepository = new Mock<IRepository<MoveNextCampaignStatusRequest, int>>();
        _mockCampaignRepository = new Mock<IRepository<Campaign, int>>();
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
        _mockCampaignMemberRepository = new Mock<IRepository<CampaignMember, int>>();
        _service = new MoveNextCampaignStatusRequestService(
            _mockMoveNextCampaignStatusRequestRepository.Object,
            _mockCampaignRepository.Object,
            _mockAuthServices.Object,
            _mockCampaignMemberRepository.Object);
        _MockService = new Mock<MoveNextCampaignStatusRequestService>();
    }

    [Fact]
    public async Task GetAllMoveNextCampaignStatusRequestAsync_ReturnsList()
    {
        // Arrange
        var requests = new List<MoveNextCampaignStatusRequest> { new MoveNextCampaignStatusRequest() };
        _mockMoveNextCampaignStatusRequestRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<MoveNextCampaignStatusRequest, bool>>>())).Returns(requests.AsQueryable().BuildMock());

        // Act
        var result = await _service.GetAllMoveNextCampaignStatusRequestAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetMoveNextCampaignStatusRequestByIdAsync1_ReturnsRequest()
    {
        // Arrange
        var request = new MoveNextCampaignStatusRequest { Id = 1 };
        _mockMoveNextCampaignStatusRequestRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<MoveNextCampaignStatusRequest, bool>>>())).ReturnsAsync(request);

        // Act
        var result = await _service.GetMoveNextCampaignStatusRequestByIdAsync1(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }


    [Fact]
    public async Task CreateMoveNextCampaignStatusRequestAsync_ReturnsRequest()
    {
        // Arrange
        var requestDto = new CreateMoveNextCampaignStatusRequestDTO { CampaignID = 1, Title = "Test", SenderId = "1", Description = "Test" };
        var campaign = new Campaign { Id = 1, Status = "Fund-Raising" };
        var message = new StringBuilder();

        // Tạo một MoveNextCampaignStatusRequest để trả về từ mock
        var nextStatus = "Implement"; // Giả sử nextStatus là "Implement"
        var expectedRequest = new MoveNextCampaignStatusRequest
        {
            CampaignID = requestDto.CampaignID,
            StatusOfCampaign = nextStatus,
            Status = "Pending",
            Title = requestDto.Title,
            SenderId = requestDto.SenderId,
            CreatedAt = new DateTime(2002, 05, 16), // Hoặc bạn có thể set một giá trị DateTime cụ thể
            Description = requestDto.Description
        };

        _mockCampaignRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Campaign, bool>>>())).ReturnsAsync(campaign);
        _MockService.Setup(service => service.CheckRequestHasBeenCreated(requestDto.CampaignID)).ReturnsAsync(true);
        _MockService.Setup(service => service.CheckMoneyOfCampaignAsync(requestDto.CampaignID)).ReturnsAsync(true);

        // Thiết lập mock để trả về expectedRequest
        _mockMoveNextCampaignStatusRequestRepository.Setup(repo => repo.InsertAsync(It.IsAny<MoveNextCampaignStatusRequest>()))
            .ReturnsAsync(expectedRequest);

        // Act
        var result = await _service.CreateMoveNextCampaignStatusRequestAsync(requestDto, message);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test", result.Title);
    }

    [Fact]
    public async Task ValidateCampaignCreateAsync_ValidRequest_ReturnsTrue()
    {
        // Arrange
        var requestDto = new CreateMoveNextCampaignStatusRequestDTO
        {
            CampaignID = 1,
            SenderId = "1",
            CreatedAt = DateTime.Now
        };
        var message = new StringBuilder();
        var campaign = new Campaign { Id = 1, IsActive = true };
        var campaignMember = new CampaignMember { UserId = "1", CampaignId = 1 };

        _mockAuthServices.Setup(auth => auth.CheckUserInRole(requestDto.SenderId, AppRole.ProjectManager, message)).ReturnsAsync(true);
        _mockCampaignRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Campaign, bool>>>())).ReturnsAsync(campaign);
        _mockCampaignMemberRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<CampaignMember, bool>>>())).ReturnsAsync(campaignMember);

        // Act
        var result = await _service.ValidateCampaignCreateAsync(requestDto, message);

        // Assert
        Assert.True(result);
    }
    [Fact]
    public async Task ValidateCampaignCreateAsync_InvalidUser_ReturnsFalse()
    {
        // Arrange
        var requestDto = new CreateMoveNextCampaignStatusRequestDTO { SenderId = "1" };
        var message = new StringBuilder();

        _mockAuthServices.Setup(auth => auth.CheckUserInRole(requestDto.SenderId, AppRole.ProjectManager, message)).ReturnsAsync(false);

        // Act
        var result = await _service.ValidateCampaignCreateAsync(requestDto, message);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ValidateCampaignCreateAsync_CampaignNotFound_ThrowsException()
    {
        // Arrange
        var requestDto = new CreateMoveNextCampaignStatusRequestDTO { CampaignID = 1, SenderId = "1" };
        var message = new StringBuilder();

        _mockAuthServices.Setup(auth => auth.CheckUserInRole(requestDto.SenderId, AppRole.ProjectManager, message)).ReturnsAsync(true);
        _mockCampaignRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Campaign, bool>>>())).ReturnsAsync((Campaign)null);

        // Act
        var result = await _service.ValidateCampaignCreateAsync(requestDto, message);

        // Assert
        Assert.False(result); // Kiểm tra kết quả trả về là false
        Assert.NotEmpty(message.ToString()); // Kiểm tra message có chứa thông báo lỗi
    }

    [Fact]
    public async Task ValidateCampaignCreateAsync_CampaignInactive_ThrowsException()
    {
        // Arrange
        var requestDto = new CreateMoveNextCampaignStatusRequestDTO { CampaignID = 1, SenderId = "1" };
        var message = new StringBuilder();
        var campaign = new Campaign { Id = 1, IsActive = false };

        _mockAuthServices.Setup(auth => auth.CheckUserInRole(requestDto.SenderId, AppRole.ProjectManager, message)).ReturnsAsync(true);
        _mockCampaignRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Campaign, bool>>>())).ReturnsAsync(campaign);

        // Act
        var result = await _service.ValidateCampaignCreateAsync(requestDto, message);

        // Assert
        Assert.False(result); // Kiểm tra kết quả trả về là false
        Assert.NotEmpty(message.ToString()); // Kiểm tra message có chứa thông báo lỗi
    }

    [Fact]
    public async Task ValidateCampaignCreateAsync_FutureCreatedAt_ThrowsException()
    {
        // Arrange
        var requestDto = new CreateMoveNextCampaignStatusRequestDTO { CampaignID = 1, SenderId = "1", CreatedAt = DateTime.Now.AddDays(1) };
        var message = new StringBuilder();
        var campaign = new Campaign { Id = 1, IsActive = true };

        _mockAuthServices.Setup(auth => auth.CheckUserInRole(requestDto.SenderId, AppRole.ProjectManager, message)).ReturnsAsync(true);
        _mockCampaignRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Campaign, bool>>>())).ReturnsAsync(campaign);

        // Act
        var result = await _service.ValidateCampaignCreateAsync(requestDto, message);

        // Assert
        Assert.False(result); // Kiểm tra kết quả trả về là false
        Assert.NotEmpty(message.ToString()); // Kiểm tra message có chứa thông báo lỗi
    }

    [Fact]
    public async Task ValidateCampaignCreateAsync_UserNotAssociatedWithCampaign_ThrowsException()
    {
        // Arrange
        var requestDto = new CreateMoveNextCampaignStatusRequestDTO { CampaignID = 1, SenderId = "1", CreatedAt = DateTime.Now };
        var message = new StringBuilder();
        var campaign = new Campaign { Id = 1, IsActive = true };

        _mockAuthServices.Setup(auth => auth.CheckUserInRole(requestDto.SenderId, AppRole.ProjectManager, message)).ReturnsAsync(true);
        _mockCampaignRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Campaign, bool>>>())).ReturnsAsync(campaign);
        _mockCampaignMemberRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<CampaignMember, bool>>>())).ReturnsAsync((CampaignMember)null);

        // Act
        var result = await _service.ValidateCampaignCreateAsync(requestDto, message);

        // Assert
        Assert.False(result); // Kiểm tra kết quả trả về là false
        Assert.NotEmpty(message.ToString()); // Kiểm tra message có chứa thông báo lỗi
    }
    [Fact]
    public async Task CancelMoveNextCampaignStatusRequestAsync_CancelsRequest()
    {
        // Arrange
        var request = new MoveNextCampaignStatusRequest { Id = 1, SenderId = "1", Status = "Pending" };
        var message = new StringBuilder();
        _mockMoveNextCampaignStatusRequestRepository.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync(request);
        //request.Status = "Cancel";
        _mockMoveNextCampaignStatusRequestRepository.Setup(repo => repo.UpdateAsync(It.IsAny<MoveNextCampaignStatusRequest>())).ReturnsAsync(true);

        // Act
        var result = await _service.CancelMoveNextCampaignStatusRequestAsync(1, "1", message);

        // Assert
        Assert.True(result);
        Assert.Equal("Cancel", request.Status);
    }
    //[Fact]
    //public async Task CancelMoveNextCampaignStatusRequestAsync_CancelsRequest()
    //{
    //    // Arrange
    //    var request = new MoveNextCampaignStatusRequest { Id = 1, SenderId = "1", Status = "Pending" };
    //    var message = new StringBuilder();
    //    _mockMoveNextCampaignStatusRequestRepository.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync(request);
    //    _mockMoveNextCampaignStatusRequestRepository.Setup(repo => repo.UpdateAsync(It.IsAny<MoveNextCampaignStatusRequest>())).ReturnsAsync(true);

    //    // Act
    //    var result = await _service.CancelMoveNextCampaignStatusRequestAsync(1, "1", message);

    //    // Assert
    //    Assert.True(result);
    //    Assert.Equal("Cancel", request.Status);
    //    Assert.Equal("Request cancelled successfully.", message.ToString());
    //}

    [Fact]
    public async Task CancelMoveNextCampaignStatusRequestAsync_RequestNotFound_ReturnsFalse()
    {
        // Arrange
        var requestId = 1;
        var senderId = "1";
        var message = new StringBuilder();

        _mockMoveNextCampaignStatusRequestRepository.Setup(repo => repo.Get(requestId)).ReturnsAsync((MoveNextCampaignStatusRequest)null);

        // Act
        var result = await _service.CancelMoveNextCampaignStatusRequestAsync(requestId, senderId, message);

        // Assert
        Assert.False(result);
        Assert.Equal($"MoveNextCampaignStatusRequest with ID = {requestId} not found.", message.ToString());
    }

    [Fact]
    public async Task CancelMoveNextCampaignStatusRequestAsync_UnauthorizedUser_ReturnsFalse()
    {
        // Arrange
        var request = new MoveNextCampaignStatusRequest { Id = 1, SenderId = "2", Status = "Pending" };
        var message = new StringBuilder();

        _mockMoveNextCampaignStatusRequestRepository.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync(request);

        // Act
        var result = await _service.CancelMoveNextCampaignStatusRequestAsync(1, "1", message);

        // Assert
        Assert.False(result);
        Assert.Equal("You are not authorized to cancel this request.", message.ToString());
    }

    [Fact]
    public async Task CancelMoveNextCampaignStatusRequestAsync_RequestAccepted_ReturnsFalse()
    {
        // Arrange
        var request = new MoveNextCampaignStatusRequest { Id = 1, SenderId = "1", Status = "Accepted" };
        var message = new StringBuilder();

        _mockMoveNextCampaignStatusRequestRepository.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync(request);

        // Act
        var result = await _service.CancelMoveNextCampaignStatusRequestAsync(1, "1", message);

        // Assert
        Assert.False(result);
        Assert.Equal("Your request has been accepted.", message.ToString());
    }

    [Fact]
    public async Task CancelMoveNextCampaignStatusRequestAsync_RequestRejected_ReturnsFalse()
    {
        // Arrange
        var request = new MoveNextCampaignStatusRequest { Id = 1, SenderId = "1", Status = "Rejected" };
        var message = new StringBuilder();

        _mockMoveNextCampaignStatusRequestRepository.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync(request);

        // Act
        var result = await _service.CancelMoveNextCampaignStatusRequestAsync(1, "1", message);

        // Assert
        Assert.False(result);
        Assert.Equal("Your request has been rejected.", message.ToString());
    }

    [Fact]
    public async Task CancelMoveNextCampaignStatusRequestAsync_RequestAlreadyCancelled_ReturnsFalse()
    {
        // Arrange
        var request = new MoveNextCampaignStatusRequest { Id = 1, SenderId = "1", Status = "Cancel" };
        var message = new StringBuilder();

        _mockMoveNextCampaignStatusRequestRepository.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync(request);

        // Act
        var result = await _service.CancelMoveNextCampaignStatusRequestAsync(1, "1", message);

        // Assert
        Assert.False(result);
        Assert.Equal("Your request has been canceled. Cancellation is no longer possible.", message.ToString());
    }

    [Fact]
    public async Task CancelMoveNextCampaignStatusRequestAsync_ExceptionThrown_ReturnsFalse()
    {
        // Arrange
        var requestId = 1;
        var senderId = "1";
        var message = new StringBuilder();

        _mockMoveNextCampaignStatusRequestRepository.Setup(repo => repo.Get(requestId)).ThrowsAsync(new Exception("Some error"));

        // Act
        var result = await _service.CancelMoveNextCampaignStatusRequestAsync(requestId, senderId, message);

        // Assert
        Assert.False(result);
        Assert.NotEmpty(message.ToString()); // Kiểm tra message có chứa thông báo lỗi
    }


    [Fact]
    public async Task CreateMoveNextCampaignStatusRequestAsync_CampaignNotFound_ThrowsException()
    {
        // Arrange
        var requestDto = new CreateMoveNextCampaignStatusRequestDTO { CampaignID = 1 };
        var message = new StringBuilder();

        _mockCampaignRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Campaign, bool>>>())).ReturnsAsync((Campaign)null);

        // Act & Assert
        var result = await _service.CreateMoveNextCampaignStatusRequestAsync(requestDto, message);
        Assert.Null(result);
        Assert.NotEmpty(message.ToString());
    }

    [Fact]
    public async Task CreateMoveNextCampaignStatusRequestAsync_RequestAlreadyCreated_ThrowsException()
    {
        // Arrange
        var requestDto = new CreateMoveNextCampaignStatusRequestDTO { CampaignID = 1 };
        var campaign = new Campaign { Id = 1 };
        var message = new StringBuilder();

        _mockCampaignRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Campaign, bool>>>())).ReturnsAsync(campaign);
        _MockService.Setup(service => service.CheckRequestHasBeenCreated(requestDto.CampaignID)).ReturnsAsync(false);

        // Act & Assert
        var result = await _service.CreateMoveNextCampaignStatusRequestAsync(requestDto, message);
        Assert.Null(result);
        Assert.NotEmpty(message.ToString());
    }

    [Fact]
    public async Task CreateMoveNextCampaignStatusRequestAsync_ClosedCampaign_ThrowsException()
    {
        // Arrange
        var requestDto = new CreateMoveNextCampaignStatusRequestDTO { CampaignID = 1 };
        var campaign = new Campaign { Id = 1, Status = Resource.CampaignStatus_Close };
        var message = new StringBuilder();

        _mockCampaignRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Campaign, bool>>>())).ReturnsAsync(campaign);
        _MockService.Setup(service => service.CheckRequestHasBeenCreated(requestDto.CampaignID)).ReturnsAsync(true);

        // Act & Assert
        var result = await _service.CreateMoveNextCampaignStatusRequestAsync(requestDto, message);
        Assert.Null(result);
        Assert.NotEmpty(message.ToString());
    }
}
