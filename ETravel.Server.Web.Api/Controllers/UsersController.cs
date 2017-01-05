using ETravel.BAL;
using ETravel.BAL.Helpers;
using ETravel.BAL.Models;
using ETravel.BAL.Services;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace ETravel.Server.Web.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private IUnitOfWork uow;
        public UsersController()
        {
            uow = new UnitOfWork();
        }
        /*
         *  
         * USER ROUTES
         *
         */

        //GET ALL REGISTERED USERS - OK
        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetAllUsers()
        {
            using (var s = new UserService(uow))
            {
                var v = s.GetAllUsers();

                return Request.CreateResponse(HttpStatusCode.OK, v);
            }
        }

        //GET CURRENT USER INFO - OK
        [HttpGet]
        [Route("getCurrentUserInfo")]
        public HttpResponseMessage GetCurrentUserInfo()
        {
            var identity = User.Identity as ClaimsIdentity;
            long requestorUserId = UtilMethods.GetCurrentUserId(uow, identity.Name);

            using (var s = new UserService(uow))
            {
                var v = s.GetUser((int)requestorUserId);

                return Request.CreateResponse(HttpStatusCode.OK, v);
            }
        }

        //GET LAST 10 REGISTERED USERS - OK
        [AllowAnonymous]
        [HttpGet]
        [Route("lastTen")]
        public HttpResponseMessage GetLastTenRegisteredUsers()
        {
            using (var s = new UserService(uow))
            {
                var v = s.GetLastTenRegisteredUsers();

                return Request.CreateResponse(HttpStatusCode.OK, v);
            }
        }

        //GET SPECIFIC USER BY ID - OK
        [AllowAnonymous]
        [HttpGet]
        [Route("{userId}")]
        public HttpResponseMessage GetUser(int userId)
        {
            if (userId <= 0)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            using (var s = new UserService(uow))
            {
                var v = s.GetUser(userId);

                if (v == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK, v);
            }
        }

        //GET SPECIFIC USER BY USERNAME - OK
        [HttpPost]
        [Route("findByUsername")]
        public HttpResponseMessage GetUserByUsername(UserModel user)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            using (var s = new UserService(uow))
            {
                var v = s.GetUserByUsername(user.Username);

                if (v == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK, v);
            }
        }
        
        //UPDATE USER MAIN INFO - OK
        [HttpPut]
        [Route("")]
        public HttpResponseMessage UpdateUserMainInfo(UserModel user)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            var identity = User.Identity as ClaimsIdentity;

            using (var s = new UserService(uow))
            {
                var httpStatusCode = HttpStatusCode.OK;

                bool hasUpdated = s.UpdateUser(user, identity);

                switch (hasUpdated)
                {
                    //user not found
                    case false:
                        httpStatusCode = HttpStatusCode.NotFound;
                        break;
                        
                    //user updated ok
                    case true:
                        httpStatusCode = HttpStatusCode.OK;
                        break;
                }

                return Request.CreateResponse(httpStatusCode);
            }
        }

        //GET ALL USERS BY NAME - OK
        [AllowAnonymous]
        [HttpGet]
        [Route("getAllUsersByName/{searchTerm}")]
        public HttpResponseMessage GetAllUsersByName(string searchTerm)
        {
            if (searchTerm == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            using (var s = new UserService(uow))
            {
                var v = s.GetByName(searchTerm);

                return Request.CreateResponse(HttpStatusCode.OK, v);
            }
        }
    }
}
