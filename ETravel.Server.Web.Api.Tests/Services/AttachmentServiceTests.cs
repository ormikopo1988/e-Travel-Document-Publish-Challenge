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
    public class AttachmentServiceTests
    {
        private AttachmentService _attachmentService;

        private Mock<DbSet<Attachment>> _mockAttachments;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockAttachments = new Mock<DbSet<Attachment>>();

            _attachmentService = new AttachmentService(new UnitOfWork());
        }

        [TestMethod]
        public void GetUserAttachments_UserNotFound_ShouldReturnNullOrEmpty()
        {
            //Arrange

            //Act
            var attachments = _attachmentService.GetUserAttachments("111111");

            attachments.Should().HaveCount(c => c == 0);
            
        }

        [TestMethod]
        public void GetUserAttachments_UserFound_ShouldReturnNotNullAndAttachmentListWithZeroOrMoreElements()
        {
            //Arrange

            //Act
            var attachments = _attachmentService.GetUserAttachments("orme@ait.gr");

            attachments.Should().NotBeNull().And.HaveCount(c => c >= 0).And.OnlyHaveUniqueItems();

        }

        [TestMethod]
        public void IsCurrentUserAuthorized_UserNotFound_ShouldReturnNull()
        {
            var isAuthorized = _attachmentService.IsCurrentUserAuthorized(2, "ATTACHMENT", "cdsc");

            isAuthorized.Should().BeNull();
        }

        [TestMethod]
        public void IsCurrentUserAuthorized_UserFoundAttachmentNotFound_ShouldReturnNull()
        {
            var isAuthorized = _attachmentService.IsCurrentUserAuthorized(0, "ATTACHMENT", "orme@ait.gr");

            isAuthorized.Should().BeNull();
        }

        [TestMethod]
        public void IsCurrentUserAuthorized_UserFoundAttachmentFound_ShouldReturnNotNull()
        {
            var isAuthorized = _attachmentService.IsCurrentUserAuthorized(2, "ATTACHMENT", "orme@ait.gr");

            isAuthorized.Should().NotBeNull();
        }
    }
}
