using Amazon.Lambda.Annotations;
using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using CheckEventCapacity.Lambda.Data;
using CheckEventCapacity.Lambda.Entities;
using CheckEventCapacity.Lambda.Interfaces;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace CheckEventCapacity.Lambda
{

    /// <summary>
    /// Lambda function to check the avaiablity of seats against the capacity of an event
    /// </summary>
    public class Function
    {

        /// <summary>
        /// Sets up DI for repository
        /// </summary>
        private readonly IEventRepository _repo;


        public Function() : this(new EventRepository(new EventRepositorySettings(
            Environment.GetEnvironmentVariable("CONNECTIONSTRING"),
            Environment.GetEnvironmentVariable("DATABASENAME"),
            Environment.GetEnvironmentVariable("COLLECTIONNAME"))))
        {
        }

        public Function(IEventRepository repo)
        {
            _repo = repo;
        }


        /// <summary>
        /// The main function called by Lambda function to check if cpacity for requested event
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [LambdaFunction]
        public async Task FunctionHandler(SNSEvent evnt, ILambdaContext context)
        {
   
            foreach (var record in evnt.Records)
            {
                EventBooking eventBooking = null;
                try
                {
                    eventBooking = JsonSerializer.Deserialize<EventBooking>(record.Sns.Message);
                    //TODO:validate the fields in the eventBooking

                }
                catch (JsonException jsonException)
                {
                    context.Logger.LogInformation($"JSON Deserialize ERROR: For booking request with email address:{eventBooking.emailAddress}");

                }


                if (eventBooking != null)
                {
                    bool eventAvailable = await CheckEventCapacityAvailable(eventBooking, context);
                    if (!eventAvailable)
                    {
                        context.Logger.LogInformation($"Event Booking Error: Capacity not available for {eventBooking.eventName} for email address:{eventBooking.emailAddress}");
                    }
                    else
                    {
                        context.Logger.LogInformation($"Event Booking Confirmed: Capacity available for {eventBooking.eventName} for email address:{eventBooking.emailAddress}");
                    }
                }
            }
        }


        /// <summary>
        /// Gets the event details based on ID
        /// </summary>
        /// <param name="eventBooking"></param>
        /// <returns></returns>
        private async Task<bool> CheckEventCapacityAvailable(EventBooking eventBooking, ILambdaContext context)
        {
            Event eventRequested = await _repo.Get(eventBooking.eventId);

            if (eventRequested == null)
            {//if there is an issue returning an event. log the error and return no capacity.
                context.Logger.LogInformation($"Error retrieving event: For {eventBooking.eventName} for email address:{eventBooking.emailAddress}");
                return false;
            }
            return await IsCapacityPerSeats(eventBooking.seats, eventRequested.capacity);
        }


        /// <summary>
        /// method to return a boolean if capcity for requested event
        /// </summary>
        /// <param name="seats"></param>
        /// <param name="capacity"></param>
        /// <returns></returns>
        private Task<bool> IsCapacityPerSeats(int seats, int capacity)
        {
            return Task.FromResult(seats <= capacity);
        }
    }
}
