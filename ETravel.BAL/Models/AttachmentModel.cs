using System;
using System.ComponentModel.DataAnnotations;

namespace ETravel.BAL.Models
{
    public class AttachmentModel
    {
        public long? Id { get; set; }

        public long? AttachmentSetId { get; set; }

        [Required(ErrorMessage = "Name for the attachment is required")]
        public string Name { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [Required(ErrorMessage = "Order number for the attachment is required")]
        public int OrderNo { get; set; }

        public string FileId { get; set; }

        public string Url { get; set; }

        [Required(ErrorMessage = "Mime type for the attachment is required")]
        public string MimeType { get; set; }

        public string Caption { get; set; }
        
        public string HtmlCode { get; set; }

        [Required]
        public string content { get; set; }
    }
}
