namespace PContextus.Infrastructure.MongoDb
{
    using System.Text;

    using Humanizer;
    using MongoDB.Driver;
    using MongoDB.Driver.GridFS;
    using Core.Configuration;

    public sealed class MongoContext : IMongoContext
    {
        private readonly DatabaseConfiguration configuration;

        public IGridFSBucket GridFsBucket { get; }

        public MongoContext(DatabaseConfiguration configuration)
        {
            this.configuration = configuration;

            var connectionString = BuildConnection(this.configuration);

            var url = new MongoUrl(connectionString);

            var client = new MongoClient(url);

            Database = client.GetDatabase(url.DatabaseName);

            GridFsBucket = new GridFSBucket(Database);

            MongoConvention.RegisterConventions();
        }

        public IMongoDatabase Database { get; private set; }

        public IMongoCollection<T> GetCollection<T>() where T : class
        {
            return Database.GetCollection<T>(typeof(T).Name.Pluralize().ToLowerInvariant());
        }

        private static string BuildConnection(DatabaseConfiguration configuration)
        {
            var builder = new StringBuilder();

            builder.Append("mongodb://");

            if (!(string.IsNullOrEmpty(configuration.User) && string.IsNullOrEmpty(configuration.Password)))
            {
                builder.Append($"{configuration.User}:{configuration.Password}@");
            }

            builder.Append($"{configuration.Host}:{configuration.Port}/{configuration.DatabaseName}");

            return builder.ToString();
        }
    }
}
