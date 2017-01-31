using Etravel.BAL.Models;
using ETravel.BAL.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ETravel.Common.Tests
{
    [TestClass]
    public class LoggingServiceTest
    {
        [TestMethod]
        public void WriteToFile_LogMultipleILoggableClasses_ShouldWriteILoggableClassesInfoToConsole()
        {
            //Arrange
            var changedItems = new List<ILoggable>();

            var userModel = new UserModel
            {
                Id = 1,
                Username = "info@test.gr",
                CreatedDateTime = DateTime.Now,
                UpdatedDateTime = DateTime.Now,
                Name = "Info Test",
                AttachmentSetId = 1,
                ShortBio = "Bio test",
                AvatarImage = ""
            };

            changedItems.Add(userModel);

            var snsMessageModel = new SNSMessageModel()
            {
                user_email = "orme@ait.gr",
                uploaded_at = "23/12/2016",
                uploaded_document_id = "1",
                uploaded_document_url = "test"
            };

            changedItems.Add(snsMessageModel);

            //Act
            LoggingService.WriteToConsole(changedItems);

            //Assert 
            //Nothing to assert
        }
    }
}
