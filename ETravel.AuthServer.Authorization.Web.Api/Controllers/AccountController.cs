using ETravel.AuthServer.Authorization.Web.Api.Models;
using ETravel.BAL.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace ETravel.AuthServer.Authorization.Web.Api.Controllers
{

    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private AuthRepository _repo = null;

        public AccountController() {}

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(UserModel userModel)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ETravel.BAL.IUnitOfWork uow = new ETravel.BAL.UnitOfWork();

            using (var us = new UserService(uow))
            {
                var user = new ETravel.BAL.Models.UserModel();
                user.Username = userModel.UserName;
                user.Name = userModel.Name;

                try {
                    us.CreateUser(user);
                } 
                catch(Exception e)
                {
                    throw e;
                }

            }

            using (var _repo = new AuthRepository())
            {
                try { 
                    IdentityResult result = await _repo.RegisterUser(userModel);
                    IHttpActionResult errorResult = GetErrorResult(result);

                    if (errorResult != null)
                    {
                        return errorResult;
                    }
                }
                catch(Exception e)
                {
                    throw e;
                }
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo?.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }

}
