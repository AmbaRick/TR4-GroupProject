using Amazon;
using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Moq;
using Moq.Language.Flow;

namespace SendProcessingEmailLambda.Tests
{
    public class SendProcessingEmailTest
    {
        private AmazonSimpleEmailServiceClient _awsSESClient = new AmazonSimpleEmailServiceClient(RegionEndpoint.EUWest2);
        private SendEmailRequest _sendEmailRequest = new SendEmailRequest();
        private SendEmailResponse _sendEmailResponse = new SendEmailResponse();

        [Fact]
        public async Task TestShouldSendEmail()
        {
            _sendEmailRequest.Source = "pete.bisby.tr4-202405@northcoders.net";
            _sendEmailRequest.Destination = new Destination
            {
                ToAddresses = new List<string> { "pete.bisby.tr4-202405@northcoders.net" }
            };
            _sendEmailRequest.Message = new Message
            {
                Subject = new Content("Test message"),
                Body = new Body
                {
                    Html = new Content
                    {
                        Charset = "UTF-8",
                        Data = "Test message"
                    },
                    Text = new Content
                    {
                        Charset = "UTF-8",
                        Data = "Test message"
                    }
                }
            };

            //  Arrange

            var response = await _awsSESClient.SendEmailAsync(_sendEmailRequest);
            Assert.NotNull(response);

        }

    }
}