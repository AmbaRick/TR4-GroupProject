using Moq;
using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using SendProcessingEmailLambda;

namespace SendProcessingEmailLambda.Test
{
    public class TestSendProcessingEmail
    {
        //private Mock<ISendProcessingEmail> _sendProcessingEmail = new Mock<ISendProcessingEmail>();
        private Mock<SNSEvent> _snSEvent = new Mock<SNSEvent>();
        private Mock<ILambdaContext> _lambdaContext = new Mock<ILambdaContext>();

        [Fact]
        public async Task TestProcessingEmail()
        {
            //  Arrange
            //var _sendProcessingEmail = new Mock<SendProcessingEmailLambda.>();
            



            //  Act
            //var response = await _sendProcessingEmail.SendProcessingEmailHandler()
            

            //  Assert


        }
    }
}