using MongoDB.Driver;
using CheckEventCapacity.Lambda.Entities;

namespace CheckEventCapacity.Lambda.Data
{

    /// <summary>
    /// Sets up the database connection connection for MongoDB
    /// </summary>
    public class EventRepositorySetUp
    {
        public IMongoCollection<Event> Events
        {

            get;

        }

        public EventRepositorySetUp(EventRepositorySettings seventRepositorySettings)
        {
            EventRepositoryMapping.Instance
               .RegisterEventRepository<Event>(cm =>
               {
               });
            try
            {
               
                var mongoClient = new MongoClient(seventRepositorySettings.ConnectionString);
                var toDoDatabase = mongoClient.GetDatabase(seventRepositorySettings.DatabaseName);

                this.Events = toDoDatabase.GetCollection<Event>(seventRepositorySettings.CollectionName);
            }
            catch (Exception ex)
            {
                //TODO: add error handling for connection issues etc. would send to log. for not just throw .
                throw (ex);
            }

        }
    }
}
