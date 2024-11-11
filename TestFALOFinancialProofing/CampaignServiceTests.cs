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
    public class CampaignServiceTests
    {
        private readonly Mock<IRepository<Campaign, int>> _mockCampaignRepository;
        private readonly Mock<ILogger<CampaignService>> _mockLogger;
        private readonly CampaignService _campaignService;

        public CampaignServiceTests()
        {
            _mockCampaignRepository = new Mock<IRepository<Campaign, int>>();
            _mockLogger = new Mock<ILogger<CampaignService>>();
            _campaignService = new CampaignService(_mockCampaignRepository.Object,null ,null , _mockLogger.Object);
        }

        [Fact]
        public async Task UpdateCampaignAsync_NonExistentCampaignId_ReturnsFalseAndLogsError()
        {
            // Arrange
            var updateCampaignDTO = new UpdateCampaignDTO { Id = 99, Title = "Updated Title" };

            _mockCampaignRepository.Setup(repo => repo.Get(updateCampaignDTO.Id))
                .ReturnsAsync((Campaign)null);

            // Act
            var result = await _campaignService.UpdateCampaignAsync(updateCampaignDTO);

            // Assert
            Assert.False(result);
            _mockCampaignRepository.Verify(repo => repo.Get(updateCampaignDTO.Id), Times.Once);
            _mockCampaignRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Campaign>()), Times.Never);

            _mockLogger.Verify(
                log => log.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Campaign not found!")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once); // Verify that the log message was logged once
        }
    }
}
