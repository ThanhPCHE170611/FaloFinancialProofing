using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using FALOFinancialProofing.Services.ProjectServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace TestFALOFinancialProofing
{
    public class ProjectServiceTests
    {
        private readonly ProjectService _projectService;
        private readonly Mock<IRepository<Project, int>> _projectRepositoryMock;

        public ProjectServiceTests()
        {
            _projectRepositoryMock = new Mock<IRepository<Project, int>>();

            // Assuming other dependencies are not used in this method, we can pass in null for them in the constructor
            _projectService = new ProjectService(_projectRepositoryMock.Object, null, null, null, null);
        }

        [Fact]
        public async Task CreateProjectAsync_ValidProject_ReturnsTrue()
        {
            // Arrange
            var project = new Project { CreatedBy = "396b51fc-90eb-4013-b8ba-ffe774cc62ec", ProjectName = "Test Project" , Description = "des",DateOfCreation = DateTime.Now, IsActive = false };
            //_projectRepositoryMock.Setup(repo => repo.InsertAsync(project)).Returns(Task.CompletedTask);
            _projectRepositoryMock.Setup(repo => repo.InsertAsync(project)).Returns(Task.FromResult(project));


            // Act
            var result = await _projectService.CreateProjectAsync(project);

            // Assert
            Assert.True(result);
            _projectRepositoryMock.Verify(repo => repo.InsertAsync(project), Times.Once);
        }

        [Fact]
        public async Task CreateProjectAsync_NullProject_ThrowsExceptionAndReturnsFalse()
        {
            // Arrange
            Project project = null;

            // Act
            var result = await _projectService.CreateProjectAsync(project);

            // Assert
            Assert.False(result);
            _projectRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<Project>()), Times.Never);
        }
        [Fact]
        public async Task CreateProjectReturnEntityAsync_ValidProject_ReturnsProject()
        {
            // Arrange
            var validProject = new Project { /* initialize properties */ };
            _projectRepositoryMock.Setup(repo => repo.InsertAsync(validProject)).Returns(Task.FromResult(validProject));

            // Act
            var result = await _projectService.CreateProjectReturnEntityAsync(validProject);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(validProject, result);
            _projectRepositoryMock.Verify(repo => repo.InsertAsync(validProject), Times.Once);
        }
        [Fact]
        public async Task CreateProjectReturnEntityAsync_RepositoryThrowsException_ReturnsProjectAndLogsError()
        {
            // Arrange
            var validProject = new Project { /* initialize properties */ };
            _projectRepositoryMock.Setup(repo => repo.InsertAsync(validProject)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _projectService.CreateProjectReturnEntityAsync(validProject);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(validProject, result);
            // Verify that an exception was logged (depends on how you log exceptions)
        }
        [Fact]
        public async Task GetProjectByIdAsync_NonExistentId_ReturnsNullAndLogsError()
        {
            // Arrange
            int invalidId = 999;
            _projectRepositoryMock.Setup(repo => repo.Get(invalidId)).ReturnsAsync((Project)null);

            // Act
            var result = await _projectService.GetProjectByIdAsync(invalidId);

            // Assert
            Assert.Null(result);
            _projectRepositoryMock.Verify(repo => repo.Get(invalidId), Times.Once);
            // Optionally, verify the console log or exception handling as needed
        }
        [Fact]
        public async Task GetProjectByIdAsync_ValidId_ReturnsProject()
        {
            // Arrange
            int validId = 1;
            var project = new Project { Id = validId, ProjectName = "Test Project" };
            _projectRepositoryMock.Setup(repo => repo.Get(validId)).ReturnsAsync(project);

            // Act
            var result = await _projectService.GetProjectByIdAsync(validId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(project, result);
            _projectRepositoryMock.Verify(repo => repo.Get(validId), Times.Once);
        }
        [Fact]
        public async Task GetProjectByIdAsync_ExceptionDuringRetrieval_ReturnsNullAndLogsError()
        {
            // Arrange
            int anyId = 1;
            _projectRepositoryMock.Setup(repo => repo.Get(anyId)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _projectService.GetProjectByIdAsync(anyId);

            // Assert
            Assert.Null(result);
            // Verify that an exception was logged (depends on how you log exceptions)
        }
    }
}
