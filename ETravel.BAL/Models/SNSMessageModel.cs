using ETravel.Common;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Etravel.BAL.Models
{
    public class SNSMessageModel : ILoggable
    {
        [Required]
        public string user_email { get; set; }

        [Required]
        public string uploaded_at { get; set; }

        [Required]
        public string uploaded_document_id { get; set; }

        [Required]
        public string uploaded_document_url { get; set; }

        public string Log()
        {
            var logString = new StringBuilder();

            logString.Append("user_email: " + this.user_email + "\n");
            logString.Append("uploaded_at: " + this.uploaded_at + "\n");
            logString.Append("uploaded_document_id: " + this.uploaded_document_id + "\n");
            logString.Append("uploaded_document_url: " + this.uploaded_document_url + "\n");

            return logString.ToString();
        }
    }
}
