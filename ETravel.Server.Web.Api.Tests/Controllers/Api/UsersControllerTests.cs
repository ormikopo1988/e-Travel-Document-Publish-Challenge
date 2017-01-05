using ETravel.BAL;
using ETravel.DAL;
using ETravel.Server.Web.Api.Controllers;
using ETravel.Server.Web.Api.Tests.Extensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Http.Results;

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
        public void GetUser_NoUserWithGivenIdExists_ShouldReturnNotFound()
        {
            var result = _usersController.GetUser(155);

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
