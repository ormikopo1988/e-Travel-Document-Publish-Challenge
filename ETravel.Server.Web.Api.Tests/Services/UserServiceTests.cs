using ETravel.BAL;
using ETravel.BAL.Services;
using ETravel.DAL;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data.Entity;

namespace ETravel.Server.Web.Api.Tests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        private UserService _userService;

        private Mock<DbSet<User>> _mockUsers;

        private Mock<IEtravelEntities> _mockContext;
        
        [TestInitialize]
        public void TestInitialize()
        {
            _mockUsers = new Mock<DbSet<User>>();

            _mockContext = new Mock<IEtravelEntities>();

            //init this _mockContext - that would return our mock DbSet of Users
            _mockContext.SetupGet(c => c.Users).Returns(_mockUsers.Object);
            
            _userService = new UserService(new UnitOfWork());
        }

        [TestMethod]
        public void GetAllUsers_RequestForAllUsers_ShouldReturnAllRegisteredUsers()
        {
            //Arrange

            //Act
            var users = _userService.GetAllUsers();

            users.Should().NotBeEmpty();
        }

        [TestMethod]
        public void GetLastTenRegisteredUsers_RequestForLastTenRegisteredUsers_ShouldReturnAtMost10LastRegisteredUsers()
        {
            //Arrange

            //Act
            var users = _userService.GetLastTenRegisteredUsers();

            users.Should().NotBeEmpty();
            users.Count.Should().BeLessOrEqualTo(10);
        }

        [TestMethod]
        public void GetUser_UserIdNotExists_ShouldReturnNull()
        {
            //Arrange

            //Act
            var user = _userService.GetUser(0);

            user.Should().BeNull();
        }

        [TestMethod]
        public void GetUser_UserExists_ShouldReturnASingleUser()
        {
            //Arrange

            //Act
            var user = _userService.GetUser(1);

            user.Should().NotBeNull();
        }

        [TestMethod]
        public void GetUserByUsername_UsernameNotExists_ShouldReturnNull()
        {
            //Arrange

            //Act
            var user = _userService.GetUserByUsername("cdsd");

            user.Should().BeNull();
        }

        [TestMethod]
        public void GetUserByUsername_UsernameExists_ShouldReturnASingleUser()
        {
            //Arrange

            //Act
            var user = _userService.GetUserByUsername("orme@ait.gr");

            user.Should().NotBeNull();
        }

        [TestMethod]
        public void GetByName_ΝameNotExists_ShouldReturnEmptyList()
        {
            //Arrange

            //Act
            var users = _userService.GetByName("111111111");

            users.Should().NotBeNull().And.BeEmpty();
        }

        [TestMethod]
        public void GetByName_NameExists_ShouldReturnOneOrMoreUsers()
        {
            //Arrange

            //Act
            var users = _userService.GetByName("or");

            users.Should().NotBeNull().And.NotBeEmpty();
            users.Should().HaveCount(c => c >= 1).And.OnlyHaveUniqueItems();
        }
    }
}
