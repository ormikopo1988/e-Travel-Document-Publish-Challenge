using ETravel.BAL;
using ETravel.BAL.Models;
using ETravel.DAL;
using ETravel.Server.Web.Api.Controllers;
using ETravel.Server.Web.Api.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Hosting;

namespace ETravel.Server.Web.Api.Tests.Controllers.Api
{
    [TestClass]
    public class UsersControllerTests
    {
        private UsersController _usersController;
        private Mock<IRepository<User>> _mockRepository;
        private string _currentUserId;

        private UsersController controller;

        private void InitUserController(string requestUri, HttpMethod method)
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(method, requestUri);
            var route = config.Routes.MapHttpRoute("Default", "api/{controller}/{id}");

            controller = new UsersController
            {
                Request = request,
            };
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

        }
        
        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IRepository<User>>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(u => u.UserRepository).Returns(_mockRepository.Object);

            _usersController = new UsersController();
            
            _currentUserId = "1";
            _usersController.MockCurrentUser(_currentUserId, "user1@domain.com");
        }

        [TestMethod]
        public void GetUser_GivenUserIdIsLessOrEqualToZero_ShouldReturnBadRequest()
        {
            //Arrange
            InitUserController("http://etravelwebapi.azurewebsites.net/api/users/0", HttpMethod.Get);
            
            //Act
            var result = controller.GetUser(0);

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public void GetUser_NoUserWithGivenIdExists_ShouldReturnNotFound()
        {
            //Arrange
            InitUserController("http://etravelwebapi.azurewebsites.net/api/users/1337", HttpMethod.Get);
            
            //Act
            var result = controller.GetUser(155);

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }

        [TestMethod]
        public void GetUser_UserWithGivenIdExists_ShouldReturnOK()
        {
            //Arrange
            InitUserController("http://etravelwebapi.azurewebsites.net/api/users/1", HttpMethod.Get);
            
            //Act
            var result = controller.GetUser(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public void GetAllUsers_AllUsersReturned_ShouldReturnOk()
        {
            //Arrange
            InitUserController("http://etravelwebapi.azurewebsites.net/api/users", HttpMethod.Get);

            //Act
            var result = controller.GetAllUsers();

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public void GetLastTenRegisteredUsers_AtMost10MostRecentUsersReturned_ShouldReturnOk()
        {
            //Arrange
            InitUserController("http://etravelwebapi.azurewebsites.net/api/users/lastTen", HttpMethod.Get);
            
            //Act
            var result = controller.GetLastTenRegisteredUsers();

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public void GetAllUsersByName_SearchTermIsNull_ShouldReturnBadRequest()
        {
            //Arrange
            InitUserController("http://etravelwebapi.azurewebsites.net/api/users/getAllUsersByName/null", HttpMethod.Get);
            
            //Act
            var result = controller.GetAllUsersByName(null);

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public void GetAllUsersByName_SearchTermNotNull_ShouldReturnOk()
        {
            //Arrange
            InitUserController("http://etravelwebapi.azurewebsites.net/api/users/getAllUsersByName/or", HttpMethod.Get);
            
            //Act
            var result = controller.GetAllUsersByName("or");

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public void GetCurrentUserInfo_CurrentUserNotFound_ShouldReturnNotFound()
        {
            //Arrange
            InitUserController("http://etravelwebapi.azurewebsites.net/api/users/getCurrentUserInfo", HttpMethod.Get);

            var identity = new GenericIdentity("testUser");
            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);
            
            //Act
            var result = controller.GetCurrentUserInfo();

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }

        [TestMethod]
        public void GetCurrentUserInfo_CurrentUserFound_ShouldReturnOk()
        {
            //Arrange
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://etravelwebapi.azurewebsites.net/api/users/getCurrentUserInfo");
            var route = config.Routes.MapHttpRoute("Default", "api/{controller}/{id}");

            var identity = new GenericIdentity("orme@ait.gr");
            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);

            var controller = new UsersController
            {
                Request = request,
            };
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

            //Act
            var result = controller.GetCurrentUserInfo();

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public void UpdateUserMainInfo_UserModelGivenNotValid_ShouldReturnBadRequest()
        {
            //Arrange
            InitUserController("http://etravelwebapi.azurewebsites.net/api/users", HttpMethod.Put);
            
            var identity = new GenericIdentity("orme@ait.gr");
            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);
            
            var userModel = new UserModel();

            //request = userModel;

            //Act
            var result = controller.GetCurrentUserInfo();

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

    }
}
