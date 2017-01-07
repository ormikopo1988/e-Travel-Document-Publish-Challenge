using Etravel.BAL.Models;
using ETravel.BAL.Helpers;
using ETravel.BAL.Models;
using ETravel.DAL;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETravel.BAL.Services
{
    public class AttachmentService : IDisposable
    {
        protected IUnitOfWork uow;

        public AttachmentService(IUnitOfWork _uow)
        {
            if (_uow == null)
                uow = new UnitOfWork();
            else
                uow = _uow;
        }

        public async Task<long> SaveAttachment(string username, AttachmentModel newAttachment)
        {
            try
            {
                var currentUser = uow.UserRepository.SearchFor(e => e.Username == username).FirstOrDefault();
                
                var attachmentSetId = currentUser.AttachmentSetId;

                //STEP 1 - Save to external aws service
                byte[] attachmentContentToBytes = Encoding.ASCII.GetBytes(newAttachment.content);
                
                TransactionResult task = await UploadAttachment(newAttachment.MimeType, Convert.ToBase64String(attachmentContentToBytes));

                //STEP 2 - Save attachment to DB
                var _newAttachment = new Attachment()
                {
                    AttachmentSetId = attachmentSetId,
                    Name = newAttachment.Name,
                    CreatedDateTime = DateTime.Now,
                    OrderNo = newAttachment.OrderNo,
                    FileId = task.Id,
                    Url = task.Url,
                    MimeType = newAttachment.MimeType,
                    Caption = newAttachment.Caption,
                    HtmlCode = newAttachment.HtmlCode
                };
                
                uow.AttachmentRepository.Insert(_newAttachment, false);
                
                await uow.SaveChangesAsync();

                //STEP 3 - Publish a message to an AWS SNS topic
                var messageToSend = new SNSMessageModel
                {
                    user_email = currentUser.Username,
                    uploaded_at = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssK"),
                    uploaded_document_id = task.Id,
                    uploaded_document_url = task.Url
                };

                AmazonSnsService.PublishMessageToSNSTopic("New Document Upload", JsonConvert.SerializeObject(messageToSend));

                return _newAttachment.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        
        public IList<AttachmentModel> GetUserAttachments(string username)
        {
            try
            {
                User _user;

                try
                {
                    _user = uow.UserRepository
                               .SearchFor(e => e.Username == username)
                               .SingleOrDefault();
                }
                catch (InvalidOperationException ex)
                {
                    throw new InvalidOperationException("User lookup failed", ex);
                }

                var attachmentList = uow.AttachmentRepository
                                        .SearchFor(e => e.AttachmentSetId == _user.AttachmentSetId)
                                        .Select(e => new AttachmentModel()
                                        {
                                            Id = e.Id,
                                            AttachmentSetId = e.AttachmentSetId,
                                            Name = e.Name,
                                            CreatedDateTime = e.CreatedDateTime,
                                            OrderNo = e.OrderNo,
                                            FileId = e.FileId,
                                            Url = e.Url,
                                            MimeType = e.MimeType,
                                            Caption = e.Caption,
                                            HtmlCode = e.HtmlCode
                                        })
                                        .OrderBy(g => g.OrderNo)
                                        .ThenBy(g => g.CreatedDateTime)
                                        .ToList();

                return attachmentList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UtilMethods.StatusCodes> DownloadAttachment(string username, long attachmentId)
        {
            try
            {
                long requestorUserId = UtilMethods.GetCurrentUserId(uow, username);

                var attachmentToDownload = uow.AttachmentRepository.FindById(attachmentId);

                if (attachmentToDownload != null)
                {
                    var userOwner = uow
                                        .UserRepository
                                        .SearchFor(e => e.AttachmentSetId == attachmentToDownload.AttachmentSetId)
                                        .FirstOrDefault();

                    if (userOwner != null && requestorUserId == userOwner.Id)
                    {
                        var document = await DownloadAttachmentFromService(attachmentToDownload);
                        return UtilMethods.StatusCodes.OK;
                    }
                    else
                    {
                        return UtilMethods.StatusCodes.NOT_AUTHORIZED;
                    }
                }
                else
                {
                    return UtilMethods.StatusCodes.NOT_FOUND;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UtilMethods.StatusCodes> DeleteAttachment(string username , long attachmentId)
        {
            try
            {
                long requestorUserId = UtilMethods.GetCurrentUserId(uow, username);
                
                var attachmentToDelete = uow.AttachmentRepository.FindById(attachmentId);

                if(attachmentToDelete != null)
                {
                    var userOwner = uow
                                        .UserRepository
                                        .SearchFor(e => e.AttachmentSetId == attachmentToDelete.AttachmentSetId)
                                        .FirstOrDefault();

                    if (userOwner != null && requestorUserId == userOwner.Id)
                    {
                        await uow.AttachmentRepository.DeleteAsync(attachmentToDelete ,true);
                        return UtilMethods.StatusCodes.OK;
                    }
                    else
                    {
                        return UtilMethods.StatusCodes.NOT_AUTHORIZED;
                    }
                }
                else
                {
                    return UtilMethods.StatusCodes.NOT_FOUND;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        // OK
        public AuthorizationModel IsCurrentUserAuthorized(int targetId, string targetType, string username)
        {
            long requestorUserId = UtilMethods.GetCurrentUserId(uow, username);

            var currentUser = uow.UserRepository.FindById(requestorUserId);

            try
            {
                AuthorizationModel _authModel = new AuthorizationModel();

                switch (targetType)
                {
                    case "ATTACHMENT":
                        var attachment = uow.AttachmentRepository
                                            .SearchFor(e => e.Id == targetId)
                                            .SingleOrDefault();

                        if(attachment == null)
                        {
                            return null;
                        }

                        _authModel.RequestorId = requestorUserId;
                        _authModel.TargetId = targetId;
                        _authModel.TargetType = "ATTACHMENT";

                        if (attachment.AttachmentSet.Id == currentUser.AttachmentSetId)
                        {
                            _authModel.IsAllowed = true;
                        }
                        else
                        {
                            _authModel.IsAllowed = false;
                        }

                        break;
                }

                return _authModel;

            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Lookup failed", ex);
            }
        }

        private static async Task<TransactionResult> UploadAttachment(string contentType, string content)
        {
            if (contentType == null || content == null)
                return null;

            var cl = new RestClient("https://ehx1j8ife1.execute-api.eu-west-1.amazonaws.com/casestudy/");

            var request = new RestRequest("upload", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };

            request.AddParameter("content_type", contentType);
            request.AddParameter("content", content);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            var response = await cl.ExecuteTaskAsync<TransactionResult>(request);

            return response.ResponseStatus == ResponseStatus.Completed &&
                response.StatusCode == System.Net.HttpStatusCode.OK ? response.Data : null;
        }

        private static async Task<TransactionResult> DownloadAttachmentFromService(Attachment attachment)
        {
            if (attachment == null)
                return null;

            var cl = new RestClient("https://ehx1j8ife1.execute-api.eu-west-1.amazonaws.com/casestudy/");

            var request = new RestRequest("retrieve/{id}", Method.GET);

            request.AddUrlSegment("id", attachment.FileId);
            
            var response = await cl.ExecuteTaskAsync<TransactionResult>(request);

            return response.ResponseStatus == ResponseStatus.Completed &&
                response.StatusCode == System.Net.HttpStatusCode.OK ? response.Data : null;
        }

        public void Dispose() {}
    }
    
    
}