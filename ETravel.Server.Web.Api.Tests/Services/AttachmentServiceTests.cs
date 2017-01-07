using ETravel.BAL;
using ETravel.BAL.Services;
using ETravel.DAL;
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
        public void TestMethod1()
        {
        }
    }
}
