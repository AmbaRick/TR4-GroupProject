using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using Moq;
using SendProcessingEmailLambda;

namespace SendProcessingEmailLambda.Tests
{
    public class SendProcessingEmailTest
    {
        [Fact]
        public async Task TestShouldSendEmail()
        {
            Mock<SNSEvent> _snsEvent = new Mock<SNSEvent>();
            Mock<ILambdaContext> _lambdaContext = new Mock<ILambdaContext>();
            
            //  Arrange
            
            var funcSendProcessingEmail = new SendProcessingEmail();

            //  Act
            await funcSendProcessingEmail.SendProcessingEmailHandler(_snsEvent.Object, _lambdaContext.Object);

            //  Assert
        }
    }
}