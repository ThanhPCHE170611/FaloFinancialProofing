using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using FALOFinancialProofing.Services.SDGServices;
using FALOFinancialProofing.DTOs;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MockQueryable;

public class SDGServicesTest
{
    private readonly Mock<IRepository<SDG, int>> mockSDGRepository;
    private readonly SDGServices sdgServices;

    public SDGServicesTest()
    {
        mockSDGRepository = new Mock<IRepository<SDG, int>>();
        sdgServices = new SDGServices(mockSDGRepository.Object);
    }




    [Fact]
    public async Task CreateSDGAsync_ValidSDG_ReturnsSDG()
    {
        // Arrange
        var sdgRequest = new SDGRequest { Id = 1, SDGName = "SDG 1" };
        var sdg = new SDG { Id = 1, SDGName = "SDG 1" };
        mockSDGRepository.Setup(repo => repo.InsertAsync(It.IsAny<SDG>())).ReturnsAsync(sdg);

        // Act
        var result = await sdgServices.CreateSDGAsync(sdgRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(sdg.Id, result.Id);
        Assert.Equal(sdg.SDGName, result.SDGName);
    }

    [Fact]
    public async Task CreateSDGAsync_ExceptionThrown_ReturnsNull()
    {
        // Arrange
        var sdgRequest = new SDGRequest { Id = 1, SDGName = "SDG 1" };
        mockSDGRepository.Setup(repo => repo.InsertAsync(It.IsAny<SDG>())).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await sdgServices.CreateSDGAsync(sdgRequest);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateSDGAsync_ShouldReturnTrue_WhenUpdateIsSuccessful()
    {
        // Arrange
        var sdgRequest = new SDGRequest { Id = 1, SDGName = "Test SDG" };
        var sdg = new SDG { Id = 1, SDGName = "Test SDG" };

        mockSDGRepository.Setup(repo => repo.UpdateAsync(It.IsAny<SDG>())).ReturnsAsync(true);

        // Act
        var result = await sdgServices.UpdateSDGAsync(sdgRequest);

        // Assert
        Assert.True(result);
        mockSDGRepository.Verify(repo => repo.UpdateAsync(It.IsAny<SDG>()), Times.Once);
    }

    [Fact]
    public async Task UpdateSDGAsync_ShouldReturnFalse_WhenUpdateFails()
    {
        // Arrange
        var sdgRequest = new SDGRequest { Id = 1, SDGName = "Test SDG" };

        mockSDGRepository.Setup(repo => repo.UpdateAsync(It.IsAny<SDG>())).ReturnsAsync(false);

        // Act
        var result = await sdgServices.UpdateSDGAsync(sdgRequest);

        // Assert
        Assert.False(result);
        mockSDGRepository.Verify(repo => repo.UpdateAsync(It.IsAny<SDG>()), Times.Once);
    }

    [Fact]
    public async Task UpdateSDGAsync_ShouldReturnFalse_WhenExceptionIsThrown()
    {
        // Arrange
        var sdgRequest = new SDGRequest { Id = 1, SDGName = "Test SDG" };

        mockSDGRepository.Setup(repo => repo.UpdateAsync(It.IsAny<SDG>())).ThrowsAsync(new System.Exception());

        // Act
        var result = await sdgServices.UpdateSDGAsync(sdgRequest);

        // Assert
        Assert.False(result);
        mockSDGRepository.Verify(repo => repo.UpdateAsync(It.IsAny<SDG>()), Times.Once);
    }

    [Fact]
    public async Task DeleteSDGAsync_ShouldReturnTrue_WhenDeleteIsSuccessful()
    {
        // Arrange
        var sdgRequest = new SDGRequest { Id = 1 };
        var sdg = new SDG { Id = 1 };

        mockSDGRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<SDG, bool>>>())).ReturnsAsync(sdg);
        mockSDGRepository.Setup(repo => repo.DeleteAsync(It.IsAny<SDG>())).ReturnsAsync(true);

        // Act
        var result = await sdgServices.DeleteSDGAsync(sdgRequest);

        // Assert
        Assert.True(result);
        mockSDGRepository.Verify(repo => repo.Get(It.IsAny<Expression<Func<SDG, bool>>>()), Times.Once);
        mockSDGRepository.Verify(repo => repo.DeleteAsync(It.IsAny<SDG>()), Times.Once);
    }

    [Fact]
    public async Task DeleteSDGAsync_ShouldReturnFalse_WhenSDGNotFound()
    {
        // Arrange
        var sdgRequest = new SDGRequest { Id = 1 };

        mockSDGRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<SDG, bool>>>())).ReturnsAsync((SDG)null);

        // Act
        var result = await sdgServices.DeleteSDGAsync(sdgRequest);

        // Assert
        Assert.False(result);
        mockSDGRepository.Verify(repo => repo.Get(It.IsAny<Expression<Func<SDG, bool>>>()), Times.Once);
        mockSDGRepository.Verify(repo => repo.DeleteAsync(It.IsAny<SDG>()), Times.Once);
    }

    [Fact]
    public async Task DeleteSDGAsync_ShouldReturnFalse_WhenDeleteFails()
    {
        // Arrange
        var sdgRequest = new SDGRequest { Id = 1 };
        var sdg = new SDG { Id = 1 };

        mockSDGRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<SDG, bool>>>())).ReturnsAsync(sdg);
        mockSDGRepository.Setup(repo => repo.DeleteAsync(It.IsAny<SDG>())).ReturnsAsync(false);

        // Act
        var result = await sdgServices.DeleteSDGAsync(sdgRequest);

        // Assert
        Assert.False(result);
        mockSDGRepository.Verify(repo => repo.Get(It.IsAny<Expression<Func<SDG, bool>>>()), Times.Once);
        mockSDGRepository.Verify(repo => repo.DeleteAsync(It.IsAny<SDG>()), Times.Once);
    }

    [Fact]
    public async Task DeleteSDGAsync_ShouldReturnFalse_WhenExceptionIsThrown()
    {
        // Arrange
        var sdgRequest = new SDGRequest { Id = 1 };

        mockSDGRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<SDG, bool>>>())).ThrowsAsync(new System.Exception());

        // Act
        var result = await sdgServices.DeleteSDGAsync(sdgRequest);

        // Assert
        Assert.False(result);
        mockSDGRepository.Verify(repo => repo.Get(It.IsAny<Expression<Func<SDG, bool>>>()), Times.Once);
        mockSDGRepository.Verify(repo => repo.DeleteAsync(It.IsAny<SDG>()), Times.Never);
    }
    [Fact]
    public async Task GetAllSDGsAsync_ShouldReturnListOfSDGInformation_WhenDataExists()
    {
        // Arrange
        var sdgList = new List<SDG>
        {
            new SDG { Id = 1, SDGName = "SDG 1" },
            new SDG { Id = 2, SDGName = "SDG 2" }
        }.AsQueryable().BuildMock();

        var mockDbSet = new Mock<DbSet<SDG>>();
        mockDbSet.As<IQueryable<SDG>>().Setup(m => m.Provider).Returns(sdgList.Provider);
        mockDbSet.As<IQueryable<SDG>>().Setup(m => m.Expression).Returns(sdgList.Expression);
        mockDbSet.As<IQueryable<SDG>>().Setup(m => m.ElementType).Returns(sdgList.ElementType);
        mockDbSet.As<IQueryable<SDG>>().Setup(m => m.GetEnumerator()).Returns(sdgList.GetEnumerator());

        mockSDGRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<SDG, bool>>>())).Returns(mockDbSet.Object);

        // Act
        var result = await sdgServices.GetAllSDGsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("SDG 1", result[0].SDGName);
        Assert.Equal("SDG 2", result[1].SDGName);
    }

    [Fact]
    public async Task GetAllSDGsAsync_ShouldReturnNull_WhenExceptionIsThrown()
    {
        // Arrange
        mockSDGRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<SDG, bool>>>())).Throws(new System.Exception("Test exception"));

        // Act
        var result = await sdgServices.GetAllSDGsAsync();

        // Assert
        Assert.Null(result);
    }



    [Fact]
    public async Task DeleteSDGByIdAsync_ShouldReturnTrue_WhenDeleteByIdIsSuccessful()
    {
        // Arrange
        var sdgRequest = new SDGRequest { Id = 1 };
        var sdg = new SDG { Id = 1 };

        mockSDGRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<SDG, bool>>>())).ReturnsAsync(sdg);
        mockSDGRepository.Setup(repo => repo.DeleteAsync(It.IsAny<SDG>())).ReturnsAsync(true);

        // Act
        var result = await sdgServices.DeleteSDGByIdAsync(sdgRequest.Id.Value);

        // Assert
        Assert.True(result);
        mockSDGRepository.Verify(repo => repo.Get(It.IsAny<Expression<Func<SDG, bool>>>()), Times.Once);
        mockSDGRepository.Verify(repo => repo.DeleteAsync(It.IsAny<SDG>()), Times.Once);
    }

    [Fact]
    public async Task DeleteSDGByIdAsync_ShouldReturnFalse_WhenSDGNotFound()
    {
        // Arrange
        var sdgRequest = new SDGRequest { Id = 1 };

        mockSDGRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<SDG, bool>>>())).ReturnsAsync((SDG)null);

        // Act
        var result = await sdgServices.DeleteSDGByIdAsync(sdgRequest.Id.Value);

        // Assert
        Assert.False(result);
        mockSDGRepository.Verify(repo => repo.Get(It.IsAny<Expression<Func<SDG, bool>>>()), Times.Once);
        mockSDGRepository.Verify(repo => repo.DeleteAsync(It.IsAny<SDG>()), Times.Never);
    }

    [Fact]
    public async Task DeleteSDGByIdAsync_ShouldReturnFalse_WhenDeleteFails()
    {
        // Arrange
        var sdgRequest = new SDGRequest { Id = 1 };
        var sdg = new SDG { Id = 1 };

        mockSDGRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<SDG, bool>>>())).ReturnsAsync(sdg);
        mockSDGRepository.Setup(repo => repo.DeleteAsync(It.IsAny<SDG>())).ReturnsAsync(false);

        // Act
        var result = await sdgServices.DeleteSDGByIdAsync(sdgRequest.Id.Value);

        // Assert
        Assert.False(result);
        mockSDGRepository.Verify(repo => repo.Get(It.IsAny<Expression<Func<SDG, bool>>>()), Times.Once);
        mockSDGRepository.Verify(repo => repo.DeleteAsync(It.IsAny<SDG>()), Times.Once);
    }

    [Fact]
    public async Task DeleteSDGByIdAsync_ShouldReturnFalse_WhenExceptionIsThrown()
    {
        // Arrange
        var sdgRequest = new SDGRequest { Id = 1 };

        mockSDGRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<SDG, bool>>>())).ThrowsAsync(new System.Exception());

        // Act
        var result = await sdgServices.DeleteSDGByIdAsync(sdgRequest.Id.Value);

        // Assert
        Assert.False(result);
        mockSDGRepository.Verify(repo => repo.Get(It.IsAny<Expression<Func<SDG, bool>>>()), Times.Once);
        mockSDGRepository.Verify(repo => repo.DeleteAsync(It.IsAny<SDG>()), Times.Never);
    }
}
