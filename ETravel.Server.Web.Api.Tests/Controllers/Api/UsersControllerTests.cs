using ETravel.BAL;
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

        private UsersController _controller;
        private HttpRequestMessage _request;

        private void InitUserController(string requestUri, HttpMethod method, bool allowAnonymous = true, string username = null)
        {
            var config = new HttpConfiguration();
            _request = new HttpRequestMessage(method, requestUri);
            var route = config.Routes.MapHttpRoute("Default", "api/{controller}/{id}");

            if(!allowAnonymous)
            {
                var identity = new GenericIdentity(username);
                Thread.CurrentPrincipal = new GenericPrincipal(identity, null);
            }

            _controller = new UsersController
            {
                Request = _request,
            };
            _controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

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
            var result = _controller.GetUser(0);

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public void GetUser_NoUserWithGivenIdExists_ShouldReturnNotFound()
        {
            //Arrange
            InitUserController("http://etravelwebapi.azurewebsites.net/api/users/1337", HttpMethod.Get);
            
            //Act
            var result = _controller.GetUser(155);

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }

        [TestMethod]
        public void GetUser_UserWithGivenIdExists_ShouldReturnOK()
        {
            //Arrange
            InitUserController("http://etravelwebapi.azurewebsites.net/api/users/1", HttpMethod.Get);
            
            //Act
            var result = _controller.GetUser(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public void GetAllUsers_AllUsersReturned_ShouldReturnOk()
        {
            //Arrange
            InitUserController("http://etravelwebapi.azurewebsites.net/api/users", HttpMethod.Get);

            //Act
            var result = _controller.GetAllUsers();

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public void GetLastTenRegisteredUsers_AtMost10MostRecentUsersReturned_ShouldReturnOk()
        {
            //Arrange
            InitUserController("http://etravelwebapi.azurewebsites.net/api/users/lastTen", HttpMethod.Get);
            
            //Act
            var result = _controller.GetLastTenRegisteredUsers();

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public void GetAllUsersByName_SearchTermIsNull_ShouldReturnBadRequest()
        {
            //Arrange
            InitUserController("http://etravelwebapi.azurewebsites.net/api/users/getAllUsersByName/null", HttpMethod.Get);
            
            //Act
            var result = _controller.GetAllUsersByName(null);

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public void GetAllUsersByName_SearchTermNotNull_ShouldReturnOk()
        {
            //Arrange
            InitUserController("http://etravelwebapi.azurewebsites.net/api/users/getAllUsersByName/or", HttpMethod.Get);
            
            //Act
            var result = _controller.GetAllUsersByName("or");

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public void GetCurrentUserInfo_CurrentUserNotFound_ShouldReturnNotFound()
        {
            //Arrange
            InitUserController("http://etravelwebapi.azurewebsites.net/api/users/getCurrentUserInfo", HttpMethod.Get, false, "testUser");
            
            //Act
            var result = _controller.GetCurrentUserInfo();

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }

        [TestMethod]
        public void GetCurrentUserInfo_CurrentUserFound_ShouldReturnOk()
        {
            //Arrange
            InitUserController("http://etravelwebapi.azurewebsites.net/api/users/getCurrentUserInfo", HttpMethod.Get, false, "orme@ait.gr");
            
            //Act
            var result = _controller.GetCurrentUserInfo();

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
        
    }
}
