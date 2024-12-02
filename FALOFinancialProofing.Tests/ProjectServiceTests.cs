//using FALOFinancialProofing.DTOs.CreateProjectFileDTO;
//using FALOFinancialProofing.DTOs.ProjectDTOs;
//using FALOFinancialProofing.Helpers;
//using FALOFinancialProofing.Models;
//using FALOFinancialProofing.Repository;
//using FALOFinancialProofing.Services;
//using FALOFinancialProofing.Services.ProjectServices;
//using Microsoft.AspNetCore.Identity;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//public class ProjectServiceTests
//{
//    private readonly Mock<IRepository<Project, int>> _mockProjectRepository;
//    private readonly Mock<IRepository<Organization, int>> _mockOrganizationRepository;
//    private readonly Mock<AuthServices> _mockAuthServices;
//    private readonly Mock<IRepository<Campaign, int>> _mockCampaignRepository;
//    private readonly ProjectService _projectService;

//    public ProjectServiceTests()
//    {
//        _mockProjectRepository = new Mock<IRepository<Project, int>>();
//        _mockOrganizationRepository = new Mock<IRepository<Organization, int>>();

//        _mockCampaignRepository = new Mock<IRepository<Campaign, int>>();

//        _projectService = new ProjectService(
//            _mockProjectRepository.Object,
//            _mockOrganizationRepository.Object,
//            _mockAuthServices.Object,
//            _mockCampaignRepository.Object
//        );
//    }

//    [Fact]
//    public async Task CreateProjectAsync_ShouldReturnTrue_WhenProjectIsValid()
//    {
//        // Arrange
//        var project = new Project { Id = 1, ProjectName = "Test Project" };
//        _mockProjectRepository.Setup(repo => repo.InsertAsync(project)).Returns(Task.CompletedTask);

//        // Act
//        var result = await _projectService.CreateProjectAsync(project);

//        // Assert
//        Assert.True(result);
//    }

//    [Fact]
//    public async Task CreateProjectAsync_ShouldReturnFalse_WhenProjectIsNull()
//    {
//        // Act
//        var result = await _projectService.CreateProjectAsync(null);

//        // Assert
//        Assert.False(result);
//    }

//    [Fact]
//    public async Task GetProjectByIdAsync_ShouldReturnProject_WhenProjectExists()
//    {
//        // Arrange
//        var project = new Project { Id = 1, ProjectName = "Test Project" };
//        _mockProjectRepository.Setup(repo => repo.Get(1)).ReturnsAsync(project);

//        // Act
//        var result = await _projectService.GetProjectByIdAsync(1);

//        // Assert
//        Assert.Equal(project, result);
//    }

//    [Fact]
//    public async Task GetProjectByIdAsync_ShouldReturnNull_WhenProjectDoesNotExist()
//    {
//        // Arrange
//        _mockProjectRepository.Setup(repo => repo.Get(1)).ReturnsAsync((Project)null);

//        // Act
//        var result = await _projectService.GetProjectByIdAsync(1);

//        // Assert
//        Assert.Null(result);
//    }

//    [Fact]
//    public async Task GetProjectsByUserIdAsync_ShouldReturnProjects_WhenProjectsExist()
//    {
//        // Arrange
//        var projects = new List<Project> { new Project { Id = 1, CreatedBy = "user1" } };
//        _mockProjectRepository.Setup(repo => repo.GetAll()).Returns(projects.AsQueryable().BuildMock().Object);

//        // Act
//        var result = await _projectService.GetProjectsByUserIdAsync("user1");

//        // Assert
//        Assert.Equal(projects, result);
//    }

//    [Fact]
//    public async Task GetProjectsByUserIdAsync_ShouldReturnEmptyList_WhenProjectsDoNotExist()
//    {
//        // Arrange
//        _mockProjectRepository.Setup(repo => repo.GetAll()).Returns(new List<Project>().AsQueryable().BuildMock().Object);

//        // Act
//        var result = await _projectService.GetProjectsByUserIdAsync("user1");

//        // Assert
//        Assert.Empty(result);
//    }

//    [Fact]
//    public async Task CheckProjectByUserIdAndProjectIdAsync_ShouldReturnTrue_WhenProjectExists()
//    {
//        // Arrange
//        var projects = new List<Project> { new Project { Id = 1, CreatedBy = "user1" } };
//        _mockProjectRepository.Setup(repo => repo.GetAll()).Returns(projects.AsQueryable().BuildMock().Object);

//        // Act
//        var result = await _projectService.CheckProjectByUserIdAndProjectIdAsync("user1", 1);

//        // Assert
//        Assert.True(result);
//    }

//    [Fact]
//    public async Task CheckProjectByUserIdAndProjectIdAsync_ShouldReturnFalse_WhenProjectDoesNotExist()
//    {
//        // Arrange
//        var projects = new List<Project> { new Project { Id = 1, CreatedBy = "user2" } };
//        _mockProjectRepository.Setup(repo => repo.GetAll()).Returns(projects.AsQueryable().BuildMock().Object);

//        // Act
//        var result = await _projectService.CheckProjectByUserIdAndProjectIdAsync("user1", 1);

//        // Assert
//        Assert.False(result);
//    }

//    [Fact]
//    public async Task DeleteProjectAsync_ShouldReturnTrue_WhenProjectIsDeleted()
//    {
//        // Arrange
//        var project = new Project { Id = 1, ProjectName = "Test Project" };
//        _mockProjectRepository.Setup(repo => repo.Get(1)).ReturnsAsync(project);
//        _mockProjectRepository.Setup(repo => repo.DeleteAsync(project)).ReturnsAsync(true);

//        // Act
//        var result = await _projectService.DeleteProjectAsync(1);

//        // Assert
//        Assert.True(result);
//    }

//    [Fact]
//    public async Task DeleteProjectAsync_ShouldReturnFalse_WhenProjectDoesNotExist()
//    {
//        // Arrange
//        _mockProjectRepository.Setup(repo => repo.Get(1)).ReturnsAsync((Project)null);

//        // Act
//        var result = await _projectService.DeleteProjectAsync(1);

//        // Assert
//        Assert.False(result);
//    }
//}
