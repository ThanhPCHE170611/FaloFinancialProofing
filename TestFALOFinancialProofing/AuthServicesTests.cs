using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services;
using FALOFinancialProofing.Services.EmailService;
using FALOFinancialProofing.Services.ProjectServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestFALOFinancialProofing
{
    public class AuthServicesTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<SignInManager<User>> _signInManagerMock;
        private readonly AuthServices _authService;

        public AuthServicesTests()
        {
            _userManagerMock = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object, null, null, null, null, null, null, null, null);

            _signInManagerMock = new Mock<SignInManager<User>>(
                _userManagerMock.Object,
                new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                null, null, null, null);

            _authService = new AuthServices(_userManagerMock.Object, _signInManagerMock.Object, null, null, null, null);
        }

        [Fact]
        public async Task LoginUser_ShouldReturnUserDto_WhenLoginIsSuccessful()
        {
            // Arrange
            var userLogin = new SignInModel { UserName = "pm1", Password = "Abcd1234!" };
            var user = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                UserName = "pm1"
            };

            _userManagerMock.Setup(x => x.FindByNameAsync(userLogin.UserName)).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, userLogin.Password)).ReturnsAsync(true);
            _signInManagerMock.Setup(x => x.PasswordSignInAsync(userLogin.UserName, userLogin.Password, true, false)).ReturnsAsync(SignInResult.Success);
            _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });

            // Act
            var result = await _authService.LoginUser(userLogin);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.FirstName, result.FirstName);
            Assert.Equal(user.LastName, result.LastName);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.UserName, result.UserName);
            Assert.Contains("User", result.RoleNames);
        }

        [Fact]
        public async Task LoginUser_ShouldReturnNull_WhenUserNotFound()
        {
            // Arrange
            var userLogin = new SignInModel { UserName = "nonexistentuser", Password = "Test@1234" };

            _userManagerMock.Setup(x => x.FindByNameAsync(userLogin.UserName)).ReturnsAsync((User)null);

            // Act
            var result = await _authService.LoginUser(userLogin);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task LoginUser_ShouldReturnNull_WhenPasswordIsIncorrect()
        {
            // Arrange
            var userLogin = new SignInModel { UserName = "testuser", Password = "IncorrectPassword" };
            var user = new User
            {
                Id = "1",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                UserName = "testuser"
            };

            _userManagerMock.Setup(x => x.FindByNameAsync(userLogin.UserName)).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, userLogin.Password)).ReturnsAsync(false);

            // Act
            var result = await _authService.LoginUser(userLogin);

            // Assert
            Assert.Null(result);
        }
    }
}

