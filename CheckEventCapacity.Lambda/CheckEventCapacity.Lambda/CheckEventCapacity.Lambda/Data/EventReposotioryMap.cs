using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using CheckEventCapacity.Lambda.Entities;

namespace CheckEventCapacity.Lambda.Data
{

    /// <summary>
    /// Maps the objectId to mongoDB otherwise would need to add attributes to the Event Class
    /// </summary>
    public class EventRepositoryMapping
    {

        private static EventRepositoryMapping instance = null;

        private static readonly object _lock = new object();

        public static EventRepositoryMapping Instance
        {
            get
            {
                //check a instance exists othersise causes an error
                if (instance == null)
                {
                    instance = new EventRepositoryMapping();
                }
                return instance;
            }
        }

        public EventRepositoryMapping RegisterEventRepository<T>(Action<BsonClassMap<Event>> classMapInitializer)
        {

            //This ensures locks the instance of async tasks until the mapping has completed
            lock (_lock)
            {
                if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
                {

                    BsonClassMap.RegisterClassMap<Event>(cm =>
                    {
                        cm.AutoMap();
                        cm.MapIdMember(p => p.Id)
                            .SetIdGenerator(StringObjectIdGenerator.Instance)
                            .SetSerializer(new StringSerializer(BsonType.ObjectId));


                    });

                }
            }
            return this;
        }
    }
}

