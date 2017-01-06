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
    public class AttachmentsControllerTests
    {
        private AttachmentsController _attachmentsController;
        private Mock<IRepository<Attachment>> _mockRepository;
        private string _currentUserId;

        private AttachmentsController _controller;
        private HttpRequestMessage _request;

        private void InitAttachmentController(string requestUri, HttpMethod method, bool allowAnonymous = true, string username = null)
        {
            var config = new HttpConfiguration();
            _request = new HttpRequestMessage(method, requestUri);
            var route = config.Routes.MapHttpRoute("Default", "api/{controller}/{id}");

            if (!allowAnonymous)
            {
                var identity = new GenericIdentity(username);
                Thread.CurrentPrincipal = new GenericPrincipal(identity, null);
            }

            _controller = new AttachmentsController
            {
                Request = _request,
            };
            _controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

        }

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IRepository<Attachment>>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(u => u.AttachmentRepository).Returns(_mockRepository.Object);

            _attachmentsController = new AttachmentsController();

            _currentUserId = "1";
            _attachmentsController.MockCurrentUser(_currentUserId, "user1@domain.com");
        }

        [TestMethod]
        public void GetCurrentLoggedInUserAttachments_CurrentUserIdentityGiven_ShouldReturnOk()
        {
            //Arrange
            InitAttachmentController("http://etravelwebapi.azurewebsites.net/api/attachments", HttpMethod.Get, false,"orme@ait.gr");

            //Act
            var result = _controller.GetCurrentLoggedInUserAttachments();

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public void IsRequestorAttachmentCreator_AttachmentIdLessOrEqualToZeroGiven_ShouldReturnBadRequest()
        {
            //Arrange
            InitAttachmentController("http://etravelwebapi.azurewebsites.net/api/attachments/0/isCurrentUserAttachmentCreator", HttpMethod.Get, false, "orme@ait.gr");

            //Act
            var result = _controller.IsRequestorAttachmentCreator(0);

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public void IsRequestorAttachmentCreator_AttachmentWithIdGivenNotExists_ShouldReturnNotFound()
        {
            //Arrange
            InitAttachmentController("http://etravelwebapi.azurewebsites.net/api/attachments/1000/isCurrentUserAttachmentCreator", HttpMethod.Get, false, "orme@ait.gr");

            //Act
            var result = _controller.IsRequestorAttachmentCreator(1000);

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }

        [TestMethod]
        public void IsRequestorAttachmentCreator_AttachmentWithIdGivenExists_ShouldReturnOk()
        {
            //Arrange
            InitAttachmentController("http://etravelwebapi.azurewebsites.net/api/attachments/5/isCurrentUserAttachmentCreator", HttpMethod.Get, false, "orme@ait.gr");

            //Act
            var result = _controller.IsRequestorAttachmentCreator(5);

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public void DeleteAttachment_AttachmentIdLessOrEqualToZeroGiven_ShouldReturnBadRequest()
        {
            //Arrange
            InitAttachmentController("http://etravelwebapi.azurewebsites.net/api/attachments/0", HttpMethod.Delete, false, "orme@ait.gr");

            //Act
            var response = _controller.DeleteAttachment(0);

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.Result.StatusCode);
        }

        [TestMethod]
        public void DeleteAttachment_AttachmentIdNotExtistsGiven_ShouldReturnNotFound()
        {
            //Arrange
            InitAttachmentController("http://etravelwebapi.azurewebsites.net/api/attachments/1000", HttpMethod.Delete, false, "orme@ait.gr");

            //Act
            var response = _controller.DeleteAttachment(1000);

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.NotFound, response.Result.StatusCode);
        }

        [TestMethod]
        public void DeleteAttachment_AttachmentIdFromOtherUserGiven_ShouldReturnUnauthorized()
        {
            //Arrange
            InitAttachmentController("http://etravelwebapi.azurewebsites.net/api/attachments/4", HttpMethod.Delete, false, "orme@ait.gr");

            //Act
            var response = _controller.DeleteAttachment(4);

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.MethodNotAllowed, response.Result.StatusCode);
        }

        [TestMethod]
        public void DownloadAttachment_AttachmentIdLessOrEqualToZeroGiven_ShouldReturnBadRequest()
        {
            //Arrange
            InitAttachmentController("http://etravelwebapi.azurewebsites.net/api/attachments/0", HttpMethod.Get, false, "orme@ait.gr");

            //Act
            var response = _controller.DownloadAttachment(0);

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.Result.StatusCode);
        }

        [TestMethod]
        public void DownloadAttachment_AttachmentIdNotExtistsGiven_ShouldReturnNotFound()
        {
            //Arrange
            InitAttachmentController("http://etravelwebapi.azurewebsites.net/api/attachments/1000", HttpMethod.Get, false, "orme@ait.gr");

            //Act
            var response = _controller.DownloadAttachment(1000);

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.NotFound, response.Result.StatusCode);
        }

        [TestMethod]
        public void DownloadAttachment_AttachmentIdFromOtherUserGiven_ShouldReturnUnauthorized()
        {
            //Arrange
            InitAttachmentController("http://etravelwebapi.azurewebsites.net/api/attachments/4", HttpMethod.Get, false, "orme@ait.gr");

            //Act
            var response = _controller.DownloadAttachment(4);

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.MethodNotAllowed, response.Result.StatusCode);
        }

        [TestMethod]
        public void DownloadAttachment_ValidAttachmentIdGiven_ShouldReturnOk()
        {
            //Arrange
            InitAttachmentController("http://etravelwebapi.azurewebsites.net/api/attachments/5", HttpMethod.Get, false, "orme@ait.gr");

            //Act
            var response = _controller.DownloadAttachment(5);

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.OK, response.Result.StatusCode);
        }


    }
}
