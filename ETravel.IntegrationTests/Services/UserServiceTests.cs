using ETravel.BAL;
using ETravel.BAL.Models;
using ETravel.BAL.Services;
using ETravel.DAL;
using ETravel.IntegrationTests.Extensions;
using ETravel.Server.Web.Api.Controllers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace ETravel.IntegrationTests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        private ETravelEntities _context;
        private UsersController _usersController;
        private UserService _userService;
        
        [TestInitialize]
        public void TestInitialize()
        {
            _context = new ETravelEntities();
            
            _usersController = new UsersController();
            
            _userService = new UserService(new UnitOfWork());
        }
        
        [TestMethod]
        public void UpdateUser_WhenCalled_ShouldReturnTrueAndUpdateTheGivenUser()
        {
            // Arrange
            var currentApiUser = _context.Users.First();
            var identity = _usersController.MockCurrentApiUser(currentApiUser.Id.ToString(), currentApiUser.Username);
            
            // Act
            var result = _userService.UpdateUser(new UserModel
            {
                Username = currentApiUser.Username,
                Name = "Updated Name from integration Test",
                UpdatedDateTime = DateTime.Now,
                ShortBio = "Updated ShortBio from integration test",
                AvatarImage = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSODALYDYo2dqN0DG_kPNi2X7EAy1K8SpRRZQWkNv9alC62IHggOw"
            }, identity);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result, true);

            _context.Entry(currentApiUser).Reload();
            currentApiUser.Name.Should().Be("Updated Name from integration Test");
            currentApiUser.ShortBio.Should().Be("Updated ShortBio from integration test");
        }

        [TestMethod]
        public void UpdateUser_WhenUserNotFound_ShouldReturnFalse()
        {
            // Arrange
            var identity = new GenericIdentity("123");

            identity.AddClaim(
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "123")
            );
            identity.AddClaim(
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "-")
            );

            // Act
            var result = _userService.UpdateUser(new UserModel
            {
                Username = "123",
                Name = "Updated Name from integration Test",
                UpdatedDateTime = DateTime.Now,
                ShortBio = "Updated ShortBio from integration test",
                AvatarImage = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSODALYDYo2dqN0DG_kPNi2X7EAy1K8SpRRZQWkNv9alC62IHggOw"
            }, identity);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result, false);
        }

        [TestMethod]
        public void CreateUser_WhenCalled_ShouldCreateNewUserToTheDb()
        {
            // Arrange
            var newUser = new UserModel
            {
                Name = "New Integration Test User",
                Username = "tester@gmail.com",
            };

            // Act
            var noOfUsersBeforeCreation = _context.Users.Count();

            _userService.CreateUser(newUser);
            
            var noOfUsersAfterCreation = _context.Users.Count();

            // Assert
            Assert.AreEqual(noOfUsersBeforeCreation + 1, noOfUsersAfterCreation);
        }
        
    }
}
