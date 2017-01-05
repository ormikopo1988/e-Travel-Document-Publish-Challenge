using System.ComponentModel.DataAnnotations;

namespace Etravel.BAL.Models
{
    public class SNSMessageModel
    {
        [Required]
        public string user_email { get; set; }

        [Required]
        public string uploaded_at { get; set; }

        [Required]
        public string uploaded_document_id { get; set; }

        [Required]
        public string uploaded_document_url { get; set; }
    }
}
