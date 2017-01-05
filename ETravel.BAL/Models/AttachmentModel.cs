using System;
using System.ComponentModel.DataAnnotations;

namespace ETravel.BAL.Models
{
    public class AttachmentModel
    {
        public long? Id { get; set; }

        public long? AttachmentSetId { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        [Required]
        public int OrderNo { get; set; }

        public string FileId { get; set; }

        public string Url { get; set; }

        [Required]
        public string MimeType { get; set; }

        public string Caption { get; set; }
        
        public string HtmlCode { get; set; }

        [Required]
        public string content { get; set; }
    }
}
