using ETravel.Server.Web.Api.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Claims;
using System.Security.Principal;

namespace ETravel.Tests.Controllers.Api
{
    [TestClass]
    public class UsersControllerTests
    {
        private UsersController _usersController;

        public UsersControllerTests()
        {
            var identity = new GenericIdentity("user1@domain.com");

            identity.AddClaim(
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "user1@domain.com")
            );
            identity.AddClaim(
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "1")
            );

            var principal = new GenericPrincipal(identity, null);

            //var mockUnitOfWork = new Mock<IUnitOfWork>();

            _usersController = new UsersController();

        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
