using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon.Lambda.Core;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using ThirdParty.Json.LitJson;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace PublishBooking;

public class Functions
{
    private IAmazonSimpleNotificationService _client;
    
    /// <summary>
    /// Default constructor that Lambda will invoke.
    /// </summary>
    public Functions()
    {
        this._client = new AmazonSimpleNotificationServiceClient();
    }
    
    /// <summary>
    /// Constructor that unit tests will invoke.
    /// </summary>
    public Functions(IAmazonSimpleNotificationService client)
    {
        this._client = client;
    }

    [LambdaFunction]
    [RestApi(LambdaHttpMethod.Post, template: "/")]
    public async Task<IHttpResult> Post([FromBody] EventBooking eventBooking, ILambdaContext context)
    {
        context.Logger.LogInformation(
            $"Publish Booking lambda called: {eventBooking.eventId} - {eventBooking.eventName} - {eventBooking.emailAddress} - {eventBooking.seats}");
        
        // Publish to SNS
        //var topicArn = "arn:aws:sns:eu-west-2:730335382882:event-booking";
        string topicArn = Environment.GetEnvironmentVariable("SNS_ARN") ?? "arn:aws:sns:eu-west-2:339712718524:Event-Publish";
        await PublishToTopicAsync(_client, 
            topicArn, 
            JsonSerializer.Serialize(eventBooking), 
            context);
        
        return HttpResults.Accepted("Booking request has been accepted");
    }

    /// <summary>
    /// Publishes a message to an Amazon SNS topic.
    /// </summary>
    /// <param name="client">The initialized client object used to publish
    ///     to the Amazon SNS topic.</param>
    /// <param name="topicArn">The ARN of the topic.</param>
    /// <param name="messageText">The text of the message.</param>
    /// <param name="context"></param>
    public static async Task PublishToTopicAsync(IAmazonSimpleNotificationService client,
        string topicArn,
        string messageText, ILambdaContext context)
    {
        context.Logger.LogInformation($"Publishing: {messageText}");
        
        var response = await client.PublishAsync(new PublishRequest
        {
            TopicArn = topicArn,
            Message = messageText,
        });

        context.Logger.LogInformation($"Successfully published message ID: {response.MessageId}");
    }

    public class EventBooking
    {
        [JsonPropertyName("eventId")]
        public string? eventId { get; set; }
        
        [JsonPropertyName("eventName")]
        public string? eventName { get; set; }
        
        [JsonPropertyName("emailAddress")]
        public string? emailAddress { get; set; }
        
        [JsonPropertyName("seats")]
        public int seats { get; set; }
    }
}
