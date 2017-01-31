using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;

namespace ETravel.IntegrationTests.Extensions
{
    public static class ApiControllerExtensions
    {
        public static ClaimsIdentity MockCurrentApiUser(this ApiController controller, string userId, string username)
        {
            var identity = new GenericIdentity(username);

            identity.AddClaim(
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", username)
            );
            identity.AddClaim(
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userId)
            );

            return identity;
        }
    }
}
