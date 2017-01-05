using ETravel.BAL;
using ETravel.DAL;
using ETravel.Server.Web.Api.Controllers;
using ETravel.Server.Web.Api.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;

namespace ETravel.Server.Web.Api.Tests.Controllers.Api
{
    [TestClass]
    public class UsersControllerTests
    {
        private UsersController _usersController;

        public UsersControllerTests()
        {
            var mockRepository = new Mock<IRepository<User>>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(u => u.UserRepository).Returns(mockRepository.Object);


            _usersController = new UsersController();
            _usersController.MockCurrentUser("1", "user1@domain.com");
        }

        [TestMethod]
        public void GetUser_GivenUserIdIsLessOrEqualToZero_ShouldReturnBadRequest()
        {
            //Arrange
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://etravelwebapi.azurewebsites.net/api/users/0");
            var route = config.Routes.MapHttpRoute("Default", "api/{controller}/{id}");
            var controller = new UsersController
            {
                Request = request,
            };
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

            //Act
            var result = controller.GetUser(0);

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public void GetUser_NoUserWithGivenIdExists_ShouldReturnNotFound()
        {
            //Arrange
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://etravelwebapi.azurewebsites.net/api/users/1337");
            var route = config.Routes.MapHttpRoute("Default", "api/{controller}/{id}");
            var controller = new UsersController
            {
                Request = request,
            };
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

            //Act
            var result = controller.GetUser(155);

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }

        [TestMethod]
        public void GetUser_UserWithGivenIdExists_ShouldReturnOK()
        {
            //Arrange
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://etravelwebapi.azurewebsites.net/api/users/1");
            var route = config.Routes.MapHttpRoute("Default", "api/{controller}/{id}");
            var controller = new UsersController
            {
                Request = request,
            };
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

            //Act
            var result = controller.GetUser(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public void GetAllUsers_AllUsersReturned_ShouldReturnOk()
        {
            //Arrange
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://etravelwebapi.azurewebsites.net/api/users");
            var route = config.Routes.MapHttpRoute("Default", "api/{controller}/{id}");
            var controller = new UsersController
            {
                Request = request,
            };
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

            //Act
            var result = controller.GetAllUsers();

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public void GetLastTenRegisteredUsers_AtMost10MostRecentUsersReturned_ShouldReturnOk()
        {
            //Arrange
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://etravelwebapi.azurewebsites.net/api/users/lastTen");
            var route = config.Routes.MapHttpRoute("Default", "api/{controller}/{id}");
            var controller = new UsersController
            {
                Request = request,
            };
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

            //Act
            var result = controller.GetLastTenRegisteredUsers();

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public void GetAllUsersByName_SearchTermIsNull_ShouldReturnBadRequest()
        {
            //Arrange
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://etravelwebapi.azurewebsites.net/getAllUsersByName/null");
            var route = config.Routes.MapHttpRoute("Default", "api/{controller}/{id}");
            var controller = new UsersController
            {
                Request = request,
            };
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

            //Act
            var result = controller.GetAllUsersByName(null);

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public void GetAllUsersByName_SearchTermNotNull_ShouldReturnOk()
        {
            //Arrange
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://etravelwebapi.azurewebsites.net/getAllUsersByName/or");
            var route = config.Routes.MapHttpRoute("Default", "api/{controller}/{id}");
            var controller = new UsersController
            {
                Request = request,
            };
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

            //Act
            var result = controller.GetAllUsersByName("or");

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
