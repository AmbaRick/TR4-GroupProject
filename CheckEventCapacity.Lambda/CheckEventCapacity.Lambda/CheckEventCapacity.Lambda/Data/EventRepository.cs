using CheckEventCapacity.Lambda.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckEventCapacity.Lambda.Entities;

namespace CheckEventCapacity.Lambda.Data
{
    public class EventRepository : IEventRepository
    {
        public readonly IMongoCollection<Event> eventList;
        public EventRepository()
        {
            EventRepositorySetUp EventRepository = new EventRepositorySetUp();
            eventList = EventRepository.Events;
        }


        //TODO: sepearte the methods in to their own classes to keep the code clean

        //TODO: add exception handling throughout all these functions


        public async Task<Event?> Get(string id)
        {

            return await eventList.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

    }
}
