using Moq;
using Xunit;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using FALOFinancialProofing.Repository;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.SDGServices;
using FALOFinancialProofing.DTOs;

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
    public async Task DeleteSDGAsync_ValidId_ReturnsTrue()
    {
        // Arrange
        var sdgId = 1;
        mockSDGRepository.Setup(repo => repo.DeleteAsync(sdgId)).ReturnsAsync(true);

        // Act
        var result = await sdgServices.DeleteSDGAsync(sdgId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteSDGAsync_InvalidId_ReturnsFalse()
    {
        // Arrange
        var sdgId = 1;
        mockSDGRepository.Setup(repo => repo.DeleteAsync(sdgId)).ReturnsAsync(false);

        // Act
        var result = await sdgServices.DeleteSDGAsync(sdgId);

        // Assert
        Assert.False(result);
    }
}
