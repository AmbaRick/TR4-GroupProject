using Amazon.Lambda.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using Amazon.Lambda.SNSEvents;
using SharpCompress.Common;
using MongoDB.Bson.IO;
using System.Text.Json.Serialization;
using System.Text.Json;
using MongoDB.Bson.Serialization.Attributes;
using CheckEventCapacity.Lambda.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using CheckEventCapacity.Lambda.Data;
using System.Runtime.CompilerServices;
using Amazon.Lambda.Annotations;
using CheckEventCapacity.Lambda.Entities;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace CheckEventCapacity.Lambda;



public class Function()
{
    private static IEventRepository repo;
  

    static Function()
    {
        repo = new EventRepository();
    }




    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public async Task FunctionHandler(SNSEvent evnt, ILambdaContext context)
    {
        //TODO: add TraceID $event.traceId

        //Go through each notification
        foreach (var record in evnt.Records)
        {
            //TODO: check tickets against capacity
            EventBooking eventBooking = JsonSerializer.Deserialize<EventBooking>(record.Sns.Message);

            //Check Capacity of event per booking
            bool eventAvailable = await GetEvent(eventBooking);
            if (!eventAvailable)
            {

                //TODO:implement process to cancel order and send out relevant notifications
                context.Logger.LogInformation($"Event Booking Error: Capacity not available for {eventBooking.eventName} for email address:{eventBooking.emailAddress}  ");
            }
            else 
            { 
                //TODO: implement process to confirm order and send out relevant notifications
      
            }

            
        }

    }

    private async Task<bool> GetEvent(EventBooking eventBooking) {

        Event eventRequested  = await repo.Get(eventBooking.eventId);
        return await IsCapacityPerSeats(eventBooking.seats, eventRequested.capacity);
    }

    private async Task<bool> IsCapacityPerSeats(int seats, int capacity)
    {
        if (seats <= capacity) { return true; }
        else { return false; }
    }

}
