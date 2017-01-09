using ETravel.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ETravel.BAL.Models
{
    public class UserModel : ILoggable
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

        public string Log()
        {
            var logString = new StringBuilder();

            logString.Append("Id: " + this.Id + "\n");
            logString.Append("Username: " + this.Username + "\n");
            logString.Append("CreatedDateTime: " + this.CreatedDateTime + "\n");
            logString.Append("UpdatedDateTime: " + this.UpdatedDateTime + "\n");
            logString.Append("Name: " + this.Name + "\n");
            logString.Append("AttachmentSetId: " + this.AttachmentSetId + "\n");
            logString.Append("ShortBio: " + this.ShortBio + "\n");
            logString.Append("AvatarImage: " + this.AvatarImage + "\n");

            return logString.ToString();
        }
    }
}
