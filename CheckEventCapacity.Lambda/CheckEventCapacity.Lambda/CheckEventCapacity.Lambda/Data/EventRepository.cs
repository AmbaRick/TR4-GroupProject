using CheckEventCapacity.Lambda.Interfaces;
using MongoDB.Driver;
using CheckEventCapacity.Lambda.Entities;

namespace CheckEventCapacity.Lambda.Data
{
    /// <summary>
    /// Class to manage connection and methods through MongoDb for Events
    /// </summary>
    public class EventRepository : IEventRepository
    {
        public readonly IMongoCollection<Event> eventList;
        public EventRepository(EventRepositorySettings eventRepositorySettings)
        {
            EventRepositorySetUp EventRepository = new EventRepositorySetUp(eventRepositorySettings);
            eventList = EventRepository.Events;
        }


        /// <summary>
        /// Gets the event details based on ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Event?> Get(string id)
        {

            return await eventList.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

    }
}
