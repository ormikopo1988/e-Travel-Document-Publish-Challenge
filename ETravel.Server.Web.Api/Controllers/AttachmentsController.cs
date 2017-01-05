using ETravel.BAL;
using ETravel.BAL.Helpers;
using ETravel.BAL.Models;
using ETravel.BAL.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace ETravel.Server.Web.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/attachments")]
    public class AttachmentsController : ApiController
    {
        protected IUnitOfWork uow;

        public AttachmentsController()
        {
            uow = new UnitOfWork();
        }

        //GET CURRENT LOGGED IN USER UPLOADED ATTACHMENTS - OK
        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetCurrentLoggedInUserAttachments()
        {
            var identity = User.Identity as ClaimsIdentity;

            using (var s = new AttachmentService(uow))
            {
                var v = s.GetUserAttachments(identity);

                return Request.CreateResponse(HttpStatusCode.OK, v);
            }
        }

        //UPLOAD NEW ATTACHMENT - OK
        [HttpPost]
        [Route("")]
        public async Task<HttpResponseMessage> UploadAttachment(AttachmentModel attachment)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            var identity = User.Identity as ClaimsIdentity;

            using (var s = new AttachmentService(uow))
            {
                long newAttachmentId = await s.SaveAttachment(identity.Name, attachment);
                
                return Request.CreateResponse(HttpStatusCode.Created, newAttachmentId);
            }
        }

        [HttpGet]
        [Route("{attachmentId}")]
        public async Task<HttpResponseMessage> DownloadAttachment(long attachmentId)
        {
            if (attachmentId <= 0)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            try
            {
                var identity = User.Identity as ClaimsIdentity;

                var result = UtilMethods.StatusCodes.OK;

                using (var repo = new AttachmentService(uow))
                {
                    result = await repo.DownloadAttachment(identity.Name, attachmentId);
                }

                var httpStatusCode = HttpStatusCode.OK;

                switch (result)
                {
                    case UtilMethods.StatusCodes.NOT_AUTHORIZED:
                        httpStatusCode = HttpStatusCode.MethodNotAllowed;
                        break;
                    case UtilMethods.StatusCodes.NOT_FOUND:
                        httpStatusCode = HttpStatusCode.NotFound;
                        break;
                    case UtilMethods.StatusCodes.OK:
                        httpStatusCode = HttpStatusCode.OK;
                        break;
                }

                return Request.CreateResponse(httpStatusCode);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("{attachmentId}")]
        [HttpDelete] 
        public async Task<HttpResponseMessage> DeleteAttachment(long attachmentId)
        {
            if(attachmentId <= 0)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            try
            {
                var identity = User.Identity as ClaimsIdentity;

                var result = UtilMethods.StatusCodes.OK;

                using (var repo = new AttachmentService(uow))
                {
                    result = await repo.DeleteAttachment(identity.Name, attachmentId);
                }

                var httpStatusCode = HttpStatusCode.NoContent;

                switch (result)
                {
                    case UtilMethods.StatusCodes.NOT_AUTHORIZED:
                        httpStatusCode = HttpStatusCode.MethodNotAllowed;
                        break;
                    case UtilMethods.StatusCodes.NOT_FOUND:
                        httpStatusCode = HttpStatusCode.NotFound;
                        break;
                    case UtilMethods.StatusCodes.OK:
                        httpStatusCode = HttpStatusCode.NoContent;
                        break;
                }

                return Request.CreateResponse(httpStatusCode);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError , ex.Message);
            }
        }

        //CHECK IF CURRENT LOGGED IN USER HAS UPLOADED THIS ATTACHMENT - OK
        [HttpGet]
        [Route("{attachmentId}/isCurrentUserAttachmentCreator")]
        public HttpResponseMessage IsRequestorAttachmentCreator(int attachmentId)
        {
            if (attachmentId <= 0)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            var identity = User.Identity as ClaimsIdentity;

            using (var pr = new AttachmentService(uow))
            {
                var v = pr.IsCurrentUserAuthorized(attachmentId, "ATTACHMENT", identity);

                if (v == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, v);
            }
        }
    }
}
