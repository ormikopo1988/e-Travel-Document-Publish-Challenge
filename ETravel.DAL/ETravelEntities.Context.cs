﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ETravel.DAL
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class ETravelEntities : DbContext
    {
        public ETravelEntities()
            : base("name=ETravelEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<AttachmentSet> AttachmentSets { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
