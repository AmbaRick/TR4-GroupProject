using Amazon;
using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System.Text.Json;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SendProcessingEmailLambda
{
    public class SendProcessingEmail
    {
        // This address must be verified with Amazon SES.
        static readonly string senderAddress = "pete.bisby.tr4-202405@northcoders.net";

        // If your account is still in the sandbox, this address must be verified.
        static readonly string receiverAddress = "pete.bisby.tr4-202405@northcoders.net";

        // The configuration set to use for this email. If you do not want to use a
        // configuration set, comment out the following property and the
        // ConfigurationSetName = configSet argument below. 
        static readonly string configSet = "my-first-configuration-set";

        // The subject line for the email.
        static readonly string subject = "Event Booking: Processing your request";

        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public SendProcessingEmail()
        {

        }

        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an SNS event object and can be used 
        /// to respond to SNS messages.
        /// </summary>
        /// <param name="evnt">The event for the Lambda function handler to process.</param>
        /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
        /// <returns></returns>
        public async Task SendProcessingEmailHandler(SNSEvent evnt, ILambdaContext context)
        {
            //var eventJson = JsonSerializer.Serialize<SNSEvent>(evnt);
            //context.Logger.LogInformation($"SNSEvent info: {eventJson}");

            foreach (var record in evnt.Records)
            {
                await ProcessRecordAsync(record, context);
            }
        }

        private async Task ProcessRecordAsync(SNSEvent.SNSRecord record, ILambdaContext context)
        {
            context.Logger.LogInformation($"Processing record {record.Sns.Message}");

            var _eventBooking = new EventBooking();
            _eventBooking = JsonSerializer.Deserialize<EventBooking>(record.Sns.Message);

            var _emailAddress = _eventBooking?.EmailAddress ?? receiverAddress;

            Task<string> _textBodyResult = FormatMessageBodyAsync(_eventBooking, "text");
            string _textBody = await _textBodyResult;

            Task<string> _htmlBodyResult = FormatMessageBodyAsync(_eventBooking, "html");
            string _htmlBody = await _htmlBodyResult;

            // TODO: Do interesting work based on the new message
            using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.EUWest2))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = senderAddress,
                    Destination = new Destination
                    {
                        ToAddresses =
                            new List<string> { _emailAddress }
                    },
                    Message = new Message
                    {
                        Subject = new Content(subject),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = _htmlBody
                                //Data = await FormatMessageBodyAsync(_eventBooking, "text");
                            },
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = _textBody
                                //Data = FormatMessageBody(_eventBooking, "text");
                            }
                        }
                    },
                    // If you are not using a configuration set, comment
                    // or remove the following line 
                    ConfigurationSetName = configSet
                };

                try
                {
                    context.Logger.LogInformation("Sending email using Amazon SES...");
                    var response = client.SendEmailAsync(sendRequest).Result;
                    context.Logger.LogInformation("The email was sent successfully.");

                }
                catch (Exception ex)
                {
                    context.Logger.LogInformation("The email was not sent. Error message: " + ex.Message);

                }
            }

            await Task.CompletedTask;
        }

        private static Task<string> FormatMessageBodyAsync(EventBooking? _eventBooking, string _format)
        {
            string _emailBody = "";

            if (_eventBooking is null)
            {
                return Task.FromResult(_emailBody);
            }

            switch (_format)
            {
                case "text":
                    _emailBody = "Thanks for placing a booking for one of our events.\r\n"
                        + "We are currently processing your request and will notify "
                        + "you again once the booking has been completed\r\n\r\n"
                        + "Event: " + _eventBooking.EventName + "\r\n"
                        + "Seats: " + _eventBooking.Seats;
                    break;
                case "html":
                    var _emailText = @"<html>
<head></head>
<body>
  <h1>Thanks for placing a booking</h1>
  <p>We are currently processing your request and will notify<br />
you again once the booking has been completed.</p>
Event: {0}<br />
Seats: {1}<br />
</body>
</html>";
                    _emailBody = string.Format(_emailText, _eventBooking.EventName, _eventBooking.Seats);
                    break;
                default:
                    _emailBody = "";
                    break;

            }

            return Task.FromResult(_emailBody);

        }

    }
}
