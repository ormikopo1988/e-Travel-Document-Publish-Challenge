using System.Data.Entity;

namespace ETravel.DAL
{
    public interface IEtravelEntities
    {
        DbSet<User> Users { get; set; }
        DbSet<AttachmentSet> AttachmentSets { get; set; }
        DbSet<Attachment> Attachments { get; set; }
    }
}
