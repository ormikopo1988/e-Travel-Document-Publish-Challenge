namespace ETravel.BAL.Models
{
    public class AuthorizationModel
    {
        public long RequestorId { get; set; }

        public long TargetId { get; set; }

        public string TargetType { get; set; }

        public bool IsAllowed { get; set; }
    }
}
