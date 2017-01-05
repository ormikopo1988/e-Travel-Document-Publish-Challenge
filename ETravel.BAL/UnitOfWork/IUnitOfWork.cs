using ETravel.DAL;
using System;
using System.Threading.Tasks;

namespace ETravel.BAL
{
    public interface IUnitOfWork : IDisposable
    {
        
        IRepository<User> UserRepository { get; set; }
        IRepository<Attachment> AttachmentRepository { get; set; }
        IRepository<AttachmentSet> AttachmentSetRepository { get; set; }

        ETravelEntities dbContext { get; set; }

        void SaveChanges(bool save = true);
        Task SaveChangesAsync(bool save = true);

    }
}
