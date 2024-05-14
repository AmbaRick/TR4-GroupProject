using MongoDB.Driver;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckEventCapacity.Lambda.Entities;

namespace CheckEventCapacity.Lambda.Data
{
    public class EventRepositorySetUp
    {
        public IMongoCollection<Event> Events
        {

            get;

        }

        public EventRepositorySetUp()
        {
            EventRepositoryMapping.Instance
               .RegisterEventRepository<Event>(cm =>
               {
               });
            try
            {
               
                var mongoClient = new MongoClient(Environment.GetEnvironmentVariable("CONNECTIONSTRING"));
                var toDoDatabase = mongoClient.GetDatabase(Environment.GetEnvironmentVariable("DATABASENAME"));

                this.Events = toDoDatabase.GetCollection<Event>(Environment.GetEnvironmentVariable("COLLECTIONNAME"));
            }
            catch (Exception ex)
            {
                //TODO: add error handling for connection issues etc. would send to log. for not just throw .
                throw (ex);
            }

        }
    }
}
