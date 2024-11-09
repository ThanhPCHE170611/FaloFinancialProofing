//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using FALOFinancialProofing.Models;
//using FALOFinancialProofing.Repository;
//using FALOFinancialProofing.Services;
//using FALOFinancialProofing.Services.CampaignService;
//using Microsoft.Extensions.Logging;
//using Moq;
//using NUnit.Framework;
//using Assert = NUnit.Framework.Assert;

//namespace TestFALOFinancialProofing
//{
//    [TestFixture]
//    public class Nunit_test
//    {
//        private Mock<IRepository<Campaign, int>> _campaignRepositoryMock;
//        private Mock<ILogger<CampaignService>> _loggerMock;
//        private CampaignService _campaignService;

//        [SetUp]
//        public void SetUp()
//        {
//            _campaignRepositoryMock = new Mock<IRepository<Campaign, int>>();
//            _loggerMock = new Mock<ILogger<CampaignService>>();
//            _campaignService = new CampaignService(_campaignRepositoryMock.Object, null, null, _loggerMock.Object);
//        }

//        [Test]
//        public async Task DeleteCampaignByIdAsync_ShouldReturnFalse_WhenCampaignNotFound()
//        {
//            // Arrange
//            int campaignId = 1;
//            _campaignRepositoryMock.Setup(repo => repo.Get(It.IsAny<System.Linq.Expressions.Expression<System.Func<Campaign, bool>>>()))
//                                   .ReturnsAsync((Campaign)null);

//            // Act
//            var result = await _campaignService.DeleteCampaignByIdAsync(campaignId);

//            // Assert
//            Assert.IsFalse(result, "Expected DeleteCampaignByIdAsync to return false when campaign is not found.");
//        }

//        [Test]
//        public async Task DeleteCampaignByIdAsync_ShouldReturnTrue_WhenCampaignIsDeletedSuccessfully()
//        {
//            // Arrange
//            int campaignId = 1;
//            var campaign = new Campaign { Id = campaignId };

//            _campaignRepositoryMock.Setup(repo => repo.Get(It.IsAny<System.Linq.Expressions.Expression<System.Func<Campaign, bool>>>()))
//                                   .ReturnsAsync(campaign);
//            _campaignRepositoryMock.Setup(repo => repo.DeleteAsync(campaign)).ReturnsAsync(true);

//            // Act
//            var result = await _campaignService.DeleteCampaignByIdAsync(campaignId);

//            // Assert
//            Assert.IsTrue(result, "Expected DeleteCampaignByIdAsync to return true when campaign is deleted successfully.");
//        }

//        [Test]
//        public async Task DeleteCampaignByIdAsync_ShouldReturnFalse_WhenExceptionOccurs()
//        {
//            // Arrange
//            int campaignId = 1;

//            _campaignRepositoryMock.Setup(repo => repo.Get(It.IsAny<System.Linq.Expressions.Expression<System.Func<Campaign, bool>>>()))
//                                   .ThrowsAsync(new System.Exception("Database error"));

//            // Act
//            var result = await _campaignService.DeleteCampaignByIdAsync(campaignId);

//            // Assert
//            Assert.IsFalse(result, "Expected DeleteCampaignByIdAsync to return false when an exception occurs.");
//        }
//    }
//}
