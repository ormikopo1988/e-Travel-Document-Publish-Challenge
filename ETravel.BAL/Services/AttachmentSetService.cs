using ETravel.DAL;
using System;

namespace ETravel.BAL.Services
{
    public class AttachmentSetService : IDisposable
    {
        protected IUnitOfWork uow;

        public AttachmentSetService(IUnitOfWork _uow)
        {
            if (_uow == null)
                uow = new UnitOfWork();
            else
                uow = _uow;
        }

        public long CreateAttachmentSet()
        {
            try
            {
                var _attachmentSet = new AttachmentSet()
                {
                    CreatedDateTime = DateTime.Now
                };

                uow.AttachmentSetRepository.Insert(_attachmentSet, true);
                
                return _attachmentSet.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose() {
            uow.Dispose();
        }
    }
}
