namespace PContextus.Infrastructure.MongoDb
{
    using MongoDB.Driver;
    using MongoDB.Driver.GridFS;

    public interface IMongoContext
    {
        IMongoDatabase Database { get; }

        IGridFSBucket GridFsBucket { get; }

        IMongoCollection<T> GetCollection<T>() where T : class;
    }
}
