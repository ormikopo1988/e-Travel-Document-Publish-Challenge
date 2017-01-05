using Amazon.SimpleNotificationService.Model;

namespace ETravel.BAL.Services
{
    public static class AmazonSnsService
    {
        private static readonly string _accessKeyId = "AKIAI5TEWNA26TMS5HOA";
        private static readonly string _secretKey = "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";

        private static readonly string _ETravel_FullStackDeveloperCaseStudy = "arn:aws:sns:eu-west-1:473424689532:ETravel_FullStackDeveloperCaseStudy";

        public static void PublishMessageToSNSTopic(string subject, string messageToSend)
        {
            using (var client = new Amazon.SimpleNotificationService.AmazonSimpleNotificationServiceClient(_accessKeyId, _secretKey, Amazon.RegionEndpoint.EUWest1))
            {
                PublishResponse publishResponse = client.Publish(
                    new PublishRequest
                    {
                        Subject = subject,
                        Message = messageToSend,
                        TopicArn = _ETravel_FullStackDeveloperCaseStudy
                    }
                );
            }
        }
    }
}
