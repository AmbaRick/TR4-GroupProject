using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.TestUtilities;
using Xunit;

namespace PublishBooking.Tests;

public class FunctionTest
{
    private int testSeats;

    public FunctionTest()
    {
    }

    [Fact]
    public void TestGetMethod()
    {
        var context = new TestLambdaContext();
        var functions = new Functions();

        var response = functions.Get(context);

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var serializationOptions = new HttpResultSerializationOptions { Format = HttpResultSerializationOptions.ProtocolFormat.RestApi };
        var apiGatewayResponse = new StreamReader(response.Serialize(serializationOptions)).ReadToEnd();
        Assert.Contains("Hello AWS Serverless", apiGatewayResponse);
    }

    [Fact]
    public void TestPostMethod()
    {
        var context = new TestLambdaContext();
        var functions = new Functions();
        
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

        var response = functions.Post(eventBookingRequest, context);

        Assert.Equal(System.Net.HttpStatusCode.Accepted, response.StatusCode);

        var serializationOptions = new HttpResultSerializationOptions { Format = HttpResultSerializationOptions.ProtocolFormat.RestApi };
        var apiGatewayResponse = new StreamReader(response.Serialize(serializationOptions)).ReadToEnd();
        Assert.Contains("Booking request has been accepted", apiGatewayResponse);
    }
}
