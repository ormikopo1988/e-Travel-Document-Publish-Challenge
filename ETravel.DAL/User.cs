//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ETravel.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public long Id { get; set; }
        public bool IsDeleted { get; set; }
        public string Username { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public System.DateTime UpdatedDateTime { get; set; }
        public string Name { get; set; }
        public long AttachmentSetId { get; set; }
        public string ShortBio { get; set; }
        public string AvatarImage { get; set; }
    
        public virtual AttachmentSet AttachmentSet { get; set; }
    }
}
