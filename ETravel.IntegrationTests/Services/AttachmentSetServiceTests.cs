using ETravel.BAL;
using ETravel.BAL.Services;
using ETravel.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ETravel.IntegrationTests.Services
{
    [TestClass]
    public class AttachmentSetServiceTests
    {
        private ETravelEntities _context;
        private AttachmentSetService _attachmentSetService;

        [TestInitialize]
        public void TestInitialize()
        {
            _context = new ETravelEntities();

            _attachmentSetService = new AttachmentSetService(new UnitOfWork());
        }
    
        [TestMethod]
        public void CreateAttachmentSet_WhenCalled_ShouldCreateNewAttachmentSetToTheDb()
        {
            // Arrange

            // Act
            var noOfAttachmentSetsBeforeCreation = _context.AttachmentSets.Count();

            _attachmentSetService.CreateAttachmentSet();

            var noOfAttachmentSetsAfterCreation = _context.AttachmentSets.Count();

            // Assert
            Assert.AreEqual(noOfAttachmentSetsBeforeCreation + 1, noOfAttachmentSetsAfterCreation);
        }
    }
}
