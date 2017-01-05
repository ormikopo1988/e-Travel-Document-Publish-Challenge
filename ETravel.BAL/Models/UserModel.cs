using System;
using System.ComponentModel.DataAnnotations;

namespace ETravel.BAL.Models
{
    public class UserModel
    {
        public long? Id { get; set; }

        [Required(ErrorMessage = "User email is required")]
        public string Username { get; set; }
        
        public DateTime CreatedDateTime { get; set; }

        public DateTime UpdatedDateTime { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        public string Name { get; set; }

        public long AttachmentSetId { get; set; }

        public string ShortBio { get; set; }

        public string AvatarImage { get; set; }
    }
}
