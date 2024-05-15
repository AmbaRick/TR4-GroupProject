using System.Net;
using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

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
    
    public Functions(IAmazonSimpleNotificationService client)
    {
        this._client = client;
    }


    /// <summary>
    /// A Lambda function to respond to HTTP Get methods from API Gateway
    /// </summary>
    /// <remarks>
    /// This uses the <see href="https://github.com/aws/aws-lambda-dotnet/blob/master/Libraries/src/Amazon.Lambda.Annotations/README.md">Lambda Annotations</see> 
    /// programming model to bridge the gap between the Lambda programming model and a more idiomatic .NET model.
    /// 
    /// This automatically handles reading parameters from an APIGatewayProxyRequest
    /// as well as syncing the function definitions to serverless.template each time you build.
    /// 
    /// If you do not wish to use this model and need to manipulate the API Gateway 
    /// objects directly, see the accompanying Readme.md for instructions.
    /// </remarks>
    /// <param name="context">Information about the invocation, function, and execution environment</param>
    /// <returns>The response as an implicit <see cref="APIGatewayProxyResponse"/></returns>
    [LambdaFunction]
    [RestApi(LambdaHttpMethod.Get, "/")]
    public IHttpResult Get(ILambdaContext context)
    {
        context.Logger.LogInformation("Handling the 'Get' Request");
        return HttpResults.Ok("Hello AWS Serverless");
    }

    [LambdaFunction]
    [RestApi(LambdaHttpMethod.Post, template: "/")]
    public async Task<IHttpResult> Post([FromBody] EventBooking eventBooking, ILambdaContext context)
    {
        context.Logger.LogInformation(
            $"Publish Booking lambda called: {eventBooking.EventId} - {eventBooking.EventName} - {eventBooking.EmailAddress} - {eventBooking.Seats}");
        
        // Publish to SNS
        var topicArn = "arn:aws:sns:eu-west-2:730335382882:event-booking";
        var messageText = JsonSerializer.Serialize(eventBooking);
        context.Logger.LogInformation($"About to publish: {messageText}");
        await PublishToTopicAsync(_client, topicArn, messageText, context);
        
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
        var request = new PublishRequest
        {
            TopicArn = topicArn,
            Message = messageText,
        };

        var response = await client.PublishAsync(request);

        context.Logger.LogInformation($"Successfully published message ID: {response.MessageId}");
    }

    public class EventBooking
    {
        public string EventId { get; set; }
        public string EventName { get; set; }
        public string EmailAddress { get; set; }
        public int Seats { get; set; }
    }
}
