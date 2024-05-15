namespace CheckEventCapacity.Lambda.Data
{

    /// <summary>
    /// sets up properties required to connect to MongoDB
    /// </summary>
    public class EventRepositorySettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CollectionName { get; set; } = null!;

        public EventRepositorySettings(string connectionString, string databaseName, string collectionName)
        {
            ConnectionString = connectionString;
            DatabaseName = databaseName;
            CollectionName = collectionName;
        }
    }
}
