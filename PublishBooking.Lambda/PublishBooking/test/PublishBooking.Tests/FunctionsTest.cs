using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.TestUtilities;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Moq;
using Xunit;

namespace PublishBooking.Tests;

public class FunctionTest
{
    private int testSeats;

    public FunctionTest()
    {
    }

    [Fact]
    public async Task TestPostMethod()
    {
        // Arrange
        var context = new TestLambdaContext();
        var mockSnsService = new Mock<IAmazonSimpleNotificationService>();
        var functions = new Functions(mockSnsService.Object);
        
        const string testUuid = "6cf0f64e-ab02-45e6-92bf-9f70a7dc76a9";
        const string testEvent = "Test Event";
        const string testEmail = "test@email.com";
        const int testSeats = 4;
        var eventBookingRequest = new Functions.EventBooking
        {
            EventId = testUuid,
            EventName = testEvent,
            EmailAddress = testEmail,
            Seats = testSeats
        };
        
        var snsResponse = new PublishResponse
        {
            MessageId = testUuid
        };
        mockSnsService.Setup(x => x.PublishAsync(It.IsAny<PublishRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(snsResponse);

        // Act
        var response = await functions.Post(eventBookingRequest, context);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Accepted, response.StatusCode);

        var serializationOptions = new HttpResultSerializationOptions { Format = HttpResultSerializationOptions.ProtocolFormat.RestApi };
        var apiGatewayResponse = new StreamReader(response.Serialize(serializationOptions)).ReadToEnd();
        Assert.Contains("Booking request has been accepted", apiGatewayResponse);
    }
}
