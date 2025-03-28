using FALOFinancialProofing.DTOs.CreateProjectFileDTO;
using FALOFinancialProofing.DTOs.ProjectDTOs;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using FALOFinancialProofing.Services;
using FALOFinancialProofing.Services.EmailService;
using FALOFinancialProofing.Services.ProjectServices;
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

public class ProjectServiceTests
{
    private readonly Mock<IRepository<Project, int>> _mockProjectRepository;
    private readonly Mock<IRepository<Organization, int>> _mockOrganizationRepository;
    private readonly Mock<AuthServices> _mockAuthServices;
    private readonly Mock<IRepository<Campaign, int>> _mockCampaignRepository;
    private readonly ProjectService _projectService;


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


    public ProjectServiceTests()
    {
        _mockProjectRepository = new Mock<IRepository<Project, int>>();
        _mockOrganizationRepository = new Mock<IRepository<Organization, int>>();
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

        _projectService = new ProjectService(
            _mockProjectRepository.Object,
            _mockOrganizationRepository.Object,
            _mockAuthServices.Object,
            _mockCampaignRepository.Object
        );
    }

    [Fact]
    public async Task CreateProjectAsync_ShouldReturnTrue_WhenProjectIsValid()
    {
        // Arrange
        var project = new Project { Id = 1, ProjectName = "Test Project" };
        _mockProjectRepository.Setup(repo => repo.InsertAsync(project)).ReturnsAsync(project);

        // Act
        var result = await _projectService.CreateProjectAsync(project);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CreateProjectAsync_ShouldReturnFalse_WhenProjectIsNull()
    {
        // Act
        var result = await _projectService.CreateProjectAsync(null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CreateProjectReturnEntityAsync_ShouldReturnProject_WhenProjectIsValid()
    {
        // Arrange
        var project = new Project { Id = 1, ProjectName = "Test Project" };
        _mockProjectRepository.Setup(repo => repo.InsertAsync(project)).ReturnsAsync(project);

        // Act
        var result = await _projectService.CreateProjectReturnEntityAsync(project);

        // Assert
        Assert.Equal(project, result);
        _mockProjectRepository.Verify(repo => repo.InsertAsync(project), Times.Once);
    }

    [Fact]
    public async Task CreateProjectReturnEntityAsync_ShouldReturnNull_WhenProjectIsNotValid()
    {
        // Arrange

        // Act
        var result = await _projectService.CreateProjectReturnEntityAsync(null);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateProjectReturnEntityAsync_ShouldLogException_WhenInsertFails()
    {
        // Arrange
        var project = new Project { Id = 1, ProjectName = "Test Project" };
        _mockProjectRepository.Setup(repo => repo.InsertAsync(project)).ThrowsAsync(new Exception("Insert failed"));

        // Act
        var result = await _projectService.CreateProjectReturnEntityAsync(project);

        // Assert
        Assert.Equal(project, result);
        _mockProjectRepository.Verify(repo => repo.InsertAsync(project), Times.Once);
    }

    [Fact]
    public async Task GetProjectByIdAsync_ShouldReturnProject_WhenProjectExists()
    {
        // Arrange
        var project = new Project { Id = 1, ProjectName = "Test Project" };
        _mockProjectRepository.Setup(repo => repo.Get(1)).ReturnsAsync(project);

        // Act
        var result = await _projectService.GetProjectByIdAsync(1);

        // Assert
        Assert.Equal(project, result);
    }

    [Fact]
    public async Task GetProjectByIdAsync_ShouldReturnNull_WhenProjectDoesNotExist()
    {
        // Arrange
        _mockProjectRepository.Setup(repo => repo.Get(1)).ReturnsAsync((Project)null);

        // Act
        var result = await _projectService.GetProjectByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetProjectsByUserIdAsync_ShouldReturnProjects_WhenProjectsExist()
    {
        // Arrange
        var projects = new List<Project> { new Project { Id = 1, CreatedBy = "user1" } };
        _mockProjectRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Project, bool>>>())).Returns(projects.AsQueryable().BuildMock());

        // Act
        var result = await _projectService.GetProjectsByUserIdAsync("user1");

        // Assert
        Assert.Equal(projects, result);
    }

    [Fact]
    public async Task GetProjectsByUserIdAsync_ShouldReturnEmptyList_WhenProjectsDoNotExist()
    {
        // Arrange
        _mockProjectRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Project, bool>>>())).Returns(new List<Project>().AsQueryable().BuildMock());

        // Act
        var result = await _projectService.GetProjectsByUserIdAsync("user1");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task CheckProjectByUserIdAndProjectIdAsync_ShouldReturnTrue_WhenProjectExists()
    {
        // Arrange
        var projects = new List<Project> { new Project { Id = 1, CreatedBy = "user1" } };
        _mockProjectRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Project, bool>>>())).Returns(projects.AsQueryable().BuildMock());

        // Act
        var result = await _projectService.CheckProjectByUserIdAndProjectIdAsync("user1", 1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CheckProjectByUserIdAndProjectIdAsync_ShouldReturnFalse_WhenProjectDoesNotExist()
    {
        // Arrange
        var projects = new List<Project> { new Project { Id = 1, CreatedBy = "user2" } };
        _mockProjectRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Project, bool>>>())).Returns(projects.AsQueryable().BuildMock());

        // Act
        var result = await _projectService.CheckProjectByUserIdAndProjectIdAsync("user1", 1);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetProjectByUserIdAndProjectIdAsync_ShouldReturnProject_WhenProjectExists()
    {
        // Arrange
        var pmUserId = "user123";
        var projectId = 1;
        var project = new Project { Id = projectId, CreatedBy = pmUserId, ProjectName = "Test Project" };
        _mockProjectRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Project, bool>>>()))
            .ReturnsAsync(project);

        // Act
        var result = await _projectService.GetProjectByUserIdAndProjectIdAsync(pmUserId, projectId);

        // Assert
        Assert.Equal(project, result);
        _mockProjectRepository.Verify(repo => repo.Get(It.IsAny<Expression<Func<Project, bool>>>()), Times.Once);
    }


    [Fact]
    public async Task GetProjectByUserIdAndProjectIdAsync_ShouldReturnNull_WhenGetNull()
    {
        // Arrange
        var pmUserId = "user123";
        var projectId = 1;
        _mockProjectRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Project, bool>>>()))
            .ReturnsAsync((Project)null);

        // Act
        var result = await _projectService.GetProjectByUserIdAndProjectIdAsync(pmUserId, projectId);

        // Assert
        Assert.Null(result);
        _mockProjectRepository.Verify(repo => repo.Get(It.IsAny<Expression<Func<Project, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetAllProjectsAsync_ShouldReturnProjectInformationList_WhenProjectsExist()
    {
        // Arrange
        var projects = new List<Project>
        {
            new Project
            {
                Id = 1,
                User = new User { Image = "user1.jpg", FirstName = "John", LastName = "Doe" },
                CreatedBy = "user1",
                ProjectName = "Project 1",
                Description = "Description 1",
                DateOfCreation = DateTime.Now,
                Status = "Active",
                IsActive = true,
                OrganizationId = 1,
                Organization = new Organization { Name = "Org 1" }
            }
        };
        _mockProjectRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Project, bool>>>())).Returns(projects.AsQueryable().BuildMock());

        // Act
        var result = await _projectService.GetAllProjectsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        var projectInfo = result.First();
        Assert.Equal(1, projectInfo.id);
        Assert.Equal("user1.jpg", projectInfo.UserImage);
        Assert.Equal("user1", projectInfo.CreatedBy);
        Assert.Equal("John", projectInfo.FirstName);
        Assert.Equal("Doe", projectInfo.LastName);
        Assert.Equal("Project 1", projectInfo.ProjectName);
        Assert.Equal("Description 1", projectInfo.Description);
        Assert.Equal("Active", projectInfo.Status);
        Assert.True(projectInfo.IsActive);
        Assert.Equal(1, projectInfo.OrganizationId);
        Assert.Equal("Org 1", projectInfo.OrganizationName);
    }

    [Fact]
    public async Task GetAllProjectsAsync_ShouldReturnEmptyList_WhenNoProjectsExist()
    {
        // Arrange
        _mockProjectRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Project, bool>>>())).Returns(new List<Project>().AsQueryable().BuildMock());

        // Act
        var result = await _projectService.GetAllProjectsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllProjectsAsync_ShouldReturnNull_WhenExceptionIsThrown()
    {
        // Arrange
        _mockProjectRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Project, bool>>>())).Throws(new Exception("Database error"));

        // Act
        var result = await _projectService.GetAllProjectsAsync();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllProjectsByUserIdAsync_ShouldReturnProjectInformationList_WhenProjectsExist()
    {
        // Arrange
        var userId = "user1";
        var request = new Mock<HttpRequest>().Object;
        var projects = new List<Project>
    {
        new Project
        {
            Id = 1,
            User = new User { Image = "user1.jpg", FirstName = "John", LastName = "Doe" },
            CreatedBy = userId,
            ProjectName = "Project 1",
            Description = "Description 1",
            DateOfCreation = DateTime.Now,
            Status = "Active",
            IsActive = true,
            OrganizationId = 1,
            Organization = new Organization { Name = "Org 1" },
            Image = "project1.jpg"
        }
    };
        _mockProjectRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Project, bool>>>())).Returns(projects.AsQueryable().BuildMock());

        // Act
        var result = await _projectService.GetAllProjectsByUserIdAsync(userId, request);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        var projectInfo = result.First();
        Assert.Equal(1, projectInfo.id);
        Assert.Equal("user1.jpg", projectInfo.UserImage);
        Assert.Equal(userId, projectInfo.CreatedBy);
        Assert.Equal("John", projectInfo.FirstName);
        Assert.Equal("Doe", projectInfo.LastName);
        Assert.Equal("Project 1", projectInfo.ProjectName);
        Assert.Equal("Description 1", projectInfo.Description);
        Assert.Equal("Active", projectInfo.Status);
        Assert.True(projectInfo.IsActive);
        Assert.Equal(1, projectInfo.OrganizationId);
        Assert.Equal("Org 1", projectInfo.OrganizationName);
        Assert.Equal("project1.jpg", projectInfo.Image);
    }

    [Fact]
    public async Task GetAllProjectsByUserIdAsync_ShouldReturnEmptyList_WhenNoProjectsExist()
    {
        // Arrange
        var userId = "user1";
        var request = new Mock<HttpRequest>().Object;
        _mockProjectRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Project, bool>>>())).Returns(new List<Project>().AsQueryable().BuildMock());

        // Act
        var result = await _projectService.GetAllProjectsByUserIdAsync(userId, request);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllProjectsByUserIdAsync_ShouldLogException_WhenExceptionIsThrown()
    {
        // Arrange
        var userId = "user1";
        var request = new Mock<HttpRequest>().Object;
        _mockProjectRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Project, bool>>>())).Throws(new Exception("Database error"));

        // Act
        var result = await _projectService.GetAllProjectsByUserIdAsync(userId, request);

        // Assert
        Assert.Null(result);
    }
    [Fact]
    public async Task GetAllProjectInSystemAsync_ShouldReturnProjectInformationList_WhenProjectsExist()
    {
        // Arrange
        var request = new Mock<HttpRequest>().Object;
        var projects = new List<Project>
    {
        new Project { Id = 1, CreatedBy = "User1", User = new User { Image = "image1.jpg", FirstName = "John", LastName = "Doe" }, ProjectName = "Project1", Description = "Description1", DateOfCreation = DateTime.Now, Status = "Active", IsActive = true, OrganizationId = 1, Organization = new Organization { Name = "Org1" }, Image = "project1.jpg" },
        new Project { Id = 2, CreatedBy = "User2", User = new User { Image = "image2.jpg", FirstName = "Jane", LastName = "Doe" }, ProjectName = "Project2", Description = "Description2", DateOfCreation = DateTime.Now, Status = "Active", IsActive = true, OrganizationId = 2, Organization = new Organization { Name = "Org2" }, Image = "project2.jpg" }
    }.AsQueryable().BuildMock();

        _mockProjectRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Project, bool>>>())).Returns(projects);

        // Act
        var result = await _projectService.GetAllProjectInSystemAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAllProjectInSystemAsync_ShouldReturnEmptyList_WhenNoProjectsExist()
    {
        // Arrange
        var request = new Mock<HttpRequest>().Object;
        var projects = new List<Project>().AsQueryable().BuildMock();

        _mockProjectRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Project, bool>>>())).Returns(projects);

        // Act
        var result = await _projectService.GetAllProjectInSystemAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllProjectInSystemAsync_ShouldLogException_WhenExceptionIsThrown()
    {
        // Arrange
        var request = new Mock<HttpRequest>().Object;
        _mockProjectRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Project, bool>>>())).Throws(new Exception("Test exception"));

        // Act
        var result = await _projectService.GetAllProjectInSystemAsync(request);

        // Assert
        Assert.Null(result);
    }
    [Fact]
    public async Task GetProjectDetailsByProjectId_ShouldReturnProjectInformation_WhenProjectExists()
    {
        // Arrange
        int projectId = 1;
        var request = new Mock<HttpRequest>();
        var project = new Project
        {
            Id = projectId,
            CreatedBy = "user1",
            ProjectName = "Test Project",
            Description = "Test Description",
            DateOfCreation = DateTime.Now,
            Image = "test_image.png",
            OrganizationId = 1,
            User = new User { FirstName = "John", LastName = "Doe", Image = "user_image.png" },
            Organization = new Organization { Name = "Test Org", Logo = "org_logo.png" },
            CreateProjectRequests = new List<CreateProjectRequest>
        {
            new CreateProjectRequest
            {
                CreateProjectFiles = new List<CreateProjectFile>
                {
                    new CreateProjectFile { Id = 1, RequestId = 1, FilePath = "file_path_1" }
                }
            }
        }
        };
        var projects = new List<Project> { project }.AsQueryable().BuildMock();
        _mockProjectRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Project, bool>>>())).Returns(projects);

        // Act
        var result = await _projectService.GetProjectDetailsByProjectId(projectId, request.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(projectId, result.id);
        Assert.Equal("user_image.png", result.UserImage);
        Assert.Equal("John", result.FirstName);
        Assert.Equal("Doe", result.LastName);
        Assert.Equal("Test Project", result.ProjectName);
        Assert.Equal("Test Description", result.Description);
        Assert.Equal(project.DateOfCreation, result.DateOfCreation);
        Assert.Equal(project.Status, result.Status);
        Assert.Equal(project.IsActive, result.IsActive);
        Assert.Equal(1, result.OrganizationId);
        Assert.Equal("Test Org", result.OrganizationName);
        Assert.Equal("test_image.png", result.Image);
        Assert.Equal("org_logo.png", result.Logo);
        Assert.Single(result.CreateProjectFiles);
        Assert.Equal(1, result.CreateProjectFiles.First().Id);
        Assert.Equal(1, result.CreateProjectFiles.First().RequestId);
        Assert.Equal("file_path_1", result.CreateProjectFiles.First().FilePath);
    }

    [Fact]
    public async Task GetProjectDetailsByProjectId_ShouldReturnNull_WhenProjectDoesNotExist()
    {
        // Arrange
        int projectId = 1;
        var request = new Mock<HttpRequest>();
        var projects = new List<Project>().AsQueryable().BuildMock();
        _mockProjectRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Project, bool>>>())).Returns(projects);

        // Act
        var result = await _projectService.GetProjectDetailsByProjectId(projectId, request.Object);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetProjectDetailsByProjectId_ShouldLogException_WhenExceptionIsThrown()
    {
        // Arrange
        int projectId = 1;
        var request = new Mock<HttpRequest>();
        _mockProjectRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Project, bool>>>())).Throws(new Exception("Test Exception"));

        // Act
        var result = await _projectService.GetProjectDetailsByProjectId(projectId, request.Object);

        // Assert
        Assert.Null(result);
        // Verify that the exception was logged
        // (You can use a logging framework or mock the Console.Out.WriteLineAsync method to verify the log)
    }

    [Fact]
    public async Task ValidateProjectCreateAsync_ShouldReturnTrue_WhenProjectIsValid()
    {
        // Arrange
        var createProject = new CreateProject { CreatedBy = "user1", OrganizationId = 1, LogoFile = null };
        var message = new StringBuilder();
        _mockAuthServices.Setup(x => x.CheckUserInRole(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<StringBuilder>())).ReturnsAsync(true);
        _mockOrganizationRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(new Organization());

        // Act
        var result = await _projectService.ValidateProjectCreateAsync(createProject, message);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ValidateProjectCreateAsync_ShouldReturnFalse_WhenUserIsNotInProjectManagerRole()
    {
        // Arrange
        var createProject = new CreateProject { CreatedBy = "user1", OrganizationId = 1, LogoFile = null };
        var message = new StringBuilder();
        _mockAuthServices.Setup(x => x.CheckUserInRole(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<StringBuilder>())).ReturnsAsync(false);

        // Act
        var result = await _projectService.ValidateProjectCreateAsync(createProject, message);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ValidateProjectCreateAsync_ShouldReturnFalse_WhenOrganizationDoesNotExist()
    {
        // Arrange
        var createProject = new CreateProject { CreatedBy = "user1", OrganizationId = 1, LogoFile = null };
        var message = new StringBuilder();
        _mockAuthServices.Setup(x => x.CheckUserInRole(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<StringBuilder>())).ReturnsAsync(true);
        _mockOrganizationRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync((Organization)null);

        // Act
        var result = await _projectService.ValidateProjectCreateAsync(createProject, message);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ValidateProjectCreateAsync_ShouldReturnFalse_WhenLogoFileIsTooLarge()
    {
        // Arrange
        var createProject = new CreateProject { CreatedBy = "user1", OrganizationId = 1, LogoFile = new FormFile(null, 0, FileHelper.ProjectImageMaxFileSize + 1, null, null) };
        var message = new StringBuilder();
        _mockAuthServices.Setup(x => x.CheckUserInRole(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<StringBuilder>())).ReturnsAsync(true);
        _mockOrganizationRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(new Organization());


        // Act
        var result = await _projectService.ValidateProjectCreateAsync(createProject, message);

        // Assert
        Assert.False(result);
        Assert.Equal("Logo is too large", message.ToString());
    }

    [Fact]
    public async Task UpdateProjectAsync_ShouldReturnTrue_WhenUpdateIsSuccessful()
    {
        // Arrange
        var updateProjectRequest = new UpdateProjectRequest
        {
            ProjectId = 1,
            ProjectName = "Updated Project",
            Description = "Updated Description",
            LogoFile = null,
            UserId = "user1",
            RoleId = "role1",
            IsActive = false,
            Status = RequestStatus.Close
        };
        var project = new Project
        {
            Id = 1,
            ProjectName = "Old Project",
            Description = "Old Description",
            Image = "old_image.png",
            IsActive = true,
            Status = RequestStatus.Close
        };
        var campaigns = new List<Campaign>
    {
        new Campaign { Id = 1, ProjectId = 1, IsActive = true, Status = RequestStatus.Close }
    };
        var message = new StringBuilder();

        _mockProjectRepository.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync(project);
        _mockAuthServices.Setup(auth => auth.CheckRole(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<StringBuilder>())).ReturnsAsync(true);
        _mockProjectRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Project>())).ReturnsAsync(true);
        _mockCampaignRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Campaign, bool>>>())).Returns(campaigns.AsQueryable().BuildMock());
        _mockCampaignRepository.Setup(repo => repo.UpdateManyAsync(It.IsAny<IEnumerable<Campaign>>())).ReturnsAsync(true);

        // Act
        var result = await _projectService.UpdateProjectAsync(updateProjectRequest, message);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateProjectAsync_ShouldReturnFalse_WhenProjectNotFound()
    {
        // Arrange
        var updateProjectRequest = new UpdateProjectRequest
        {
            ProjectId = 1,
            ProjectName = "Updated Project",
            Description = "Updated Description",
            LogoFile = null,
            UserId = "user1",
            RoleId = "role1",
            IsActive = true,
            Status = RequestStatus.Close
        };
        var message = new StringBuilder();

        _mockProjectRepository.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync((Project)null);

        // Act
        var result = await _projectService.UpdateProjectAsync(updateProjectRequest, message);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateProjectAsync_ShouldLogException_WhenUpdateFails()
    {
        // Arrange
        var updateProjectRequest = new UpdateProjectRequest
        {
            ProjectId = 1,
            ProjectName = "Updated Project",
            Description = "Updated Description",
            LogoFile = null,
            UserId = "user1",
            RoleId = "role1",
            IsActive = true,
            Status = RequestStatus.Close
        };
        var project = new Project
        {
            Id = 1,
            ProjectName = "Old Project",
            Description = "Old Description",
            Image = "old_image.png",
            IsActive = true,
            Status = RequestStatus.Close
        };
        var message = new StringBuilder();

        _mockProjectRepository.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync(project);
        _mockAuthServices.Setup(auth => auth.CheckRole(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<StringBuilder>())).ReturnsAsync(true);
        _mockProjectRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Project>())).ThrowsAsync(new Exception("Update failed"));

        // Act
        var result = await _projectService.UpdateProjectAsync(updateProjectRequest, message);

        // Assert
        Assert.False(result);
        Assert.Contains("Update failed", message.ToString());
    }

    [Fact]
    public async Task CheckProjectIsActiveAsync_ShouldReturnTrue_WhenProjectIsActive()
    {
        // Arrange
        var projectId = 1;
        var project = new Project { Id = projectId, IsActive = true };
        _mockProjectRepository.Setup(repo => repo.Get(projectId)).ReturnsAsync(project);

        // Act
        var result = await _projectService.CheckProjectIsActiveAsync(projectId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CheckProjectIsActiveAsync_ShouldReturnFalse_WhenProjectIsNotActive()
    {
        // Arrange
        var projectId = 1;
        var project = new Project { Id = projectId, IsActive = false };
        _mockProjectRepository.Setup(repo => repo.Get(projectId)).ReturnsAsync(project);

        // Act
        var result = await _projectService.CheckProjectIsActiveAsync(projectId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CheckProjectIsActiveAsync_ShouldLogException_WhenProjectNotFound()
    {
        // Arrange
        var projectId = 1;
        _mockProjectRepository.Setup(repo => repo.Get(projectId)).ThrowsAsync(new Exception("Project not found"));

        // Act
        var result = await _projectService.CheckProjectIsActiveAsync(projectId);

        // Assert
        Assert.False(result);
        _mockProjectRepository.Verify(repo => repo.Get(projectId), Times.Once);
    }


}
