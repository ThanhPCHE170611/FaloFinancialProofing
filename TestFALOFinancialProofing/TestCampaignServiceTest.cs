using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FALOFinancialProofing.DTOs.CampaignDTO;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using FALOFinancialProofing.Services.CampaignService;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestFALOFinancialProofing
{
    public class TestCampaignServiceTest
    {
        private readonly Mock<IRepository<Campaign, int>> _mockCampaignRepository;
        private readonly Mock<ILogger<CampaignService>> _mockLogger;
        private readonly CampaignService _campaignService;

        public TestCampaignServiceTest()
        {
            _mockCampaignRepository = new Mock<IRepository<Campaign, int>>();
            _mockLogger = new Mock<ILogger<CampaignService>>();
            _campaignService = new CampaignService(_mockCampaignRepository.Object, null, null, _mockLogger.Object);
        }

        [Fact]
        public async Task CreateCampaignAsync_ValidInput_ReturnsNewCampaign()
        {
            // Arrange
            var createCampaignDTO = new CreateCampaignDTO
            {
                CreateBy = "user123",
                Title = "New Campaign",
                Description = "Campaign Description",
                DateOfCreation = DateTime.Now,
                FundTarget = 10000,
                Image = "image.jpg",
                EndDate = DateTime.Now.AddMonths(1),
                Address = "Campaign Address",
                IsActive = false,
                BankingNumber = "123456789",
                Status = "Pending"
            };

            var newCampaign = new Campaign
            {
                CreateBy = createCampaignDTO.CreateBy,
                Title = createCampaignDTO.Title,
                Description = createCampaignDTO.Description,
                DateOfCreation = createCampaignDTO.DateOfCreation,
                FundTarget = createCampaignDTO.FundTarget,
                Image = createCampaignDTO.Image,
                EndDate = createCampaignDTO.EndDate,
                Address = createCampaignDTO.Address,
                IsActive = createCampaignDTO.IsActive,
                BankingNumber = createCampaignDTO.BankingNumber,
                Status = createCampaignDTO.Status
            };

            _mockCampaignRepository
                .Setup(repo => repo.InsertAsync(It.IsAny<Campaign>()))
                .ReturnsAsync(newCampaign);

            // Act
            var result = await _campaignService.CreateCampaignAsync(createCampaignDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createCampaignDTO.Title, result?.Title);
            Assert.Equal(createCampaignDTO.Description, result?.Description);
            _mockCampaignRepository.Verify(repo => repo.InsertAsync(It.IsAny<Campaign>()), Times.Once);
        }

        [Fact]
        public async Task CreateCampaignAsync_ExceptionThrown_ReturnsNull()
        {
            // Arrange
            var createCampaignDTO = new CreateCampaignDTO
            {
                CreateBy = "user123",
                Title = "New Campaign",
                Description = "Campaign Description",
                DateOfCreation = DateTime.Now,
                FundTarget = 10000,
                Image = "image.jpg",
                EndDate = DateTime.Now.AddMonths(1),
                Address = "Campaign Address",
                IsActive = false,
                BankingNumber = "123456789",
                Status = "Pending"
            };

            _mockCampaignRepository
                .Setup(repo => repo.InsertAsync(It.IsAny<Campaign>()))
                .ThrowsAsync(new System.Exception("Database error"));

            // Act
            var result = await _campaignService.CreateCampaignAsync(createCampaignDTO);

            // Assert
            Assert.Null(result);
            _mockCampaignRepository.Verify(repo => repo.InsertAsync(It.IsAny<Campaign>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCampaignByIdAsync_CampaignExists_ReturnsTrue()
        {
            // Arrange
            var campaignId = 1;
            var campaign = new Campaign { Id = campaignId };

            _mockCampaignRepository
                .Setup(repo => repo.Get(It.IsAny<int>()))
                .ReturnsAsync(campaign);

            _mockCampaignRepository
                .Setup(repo => repo.DeleteAsync(It.IsAny<Campaign>()))
                .ReturnsAsync(true);

            // Act
            var result = await _campaignService.DeleteCampaignByIdAsync(campaignId);

            // Assert
            Assert.True(result);
            _mockCampaignRepository.Verify(repo => repo.Get(It.IsAny<int>()), Times.Once);
            _mockCampaignRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Campaign>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCampaignByIdAsync_CampaignDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var campaignId = 1;

            _mockCampaignRepository
                .Setup(repo => repo.Get(It.IsAny<int>()))
                .ReturnsAsync((Campaign)null);

            // Act
            var result = await _campaignService.DeleteCampaignByIdAsync(campaignId);

            // Assert
            Assert.False(result);
            _mockCampaignRepository.Verify(repo => repo.Get(It.IsAny<int>()), Times.Once);
            _mockCampaignRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Campaign>()), Times.Never);
        }

        [Fact]
        public async Task DeleteCampaignByIdAsync_ExceptionThrown_ReturnsFalse()
        {
            // Arrange
            var campaignId = 1;

            _mockCampaignRepository
                .Setup(repo => repo.Get(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _campaignService.DeleteCampaignByIdAsync(campaignId);

            // Assert
            Assert.False(result);
            _mockCampaignRepository.Verify(repo => repo.Get(It.IsAny<int>()), Times.Once);
            _mockCampaignRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Campaign>()), Times.Never);
        }
    }
}
