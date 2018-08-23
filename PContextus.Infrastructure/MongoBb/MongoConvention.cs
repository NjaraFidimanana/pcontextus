namespace PContextus.Infrastructure.MongoDb
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Conventions;

    internal sealed class MongoConvention
    {
        internal static void RegisterConventions()
        {
            var pack = new ConventionPack
            {
                new EnumRepresentationConvention(BsonType.String),
                new IgnoreExtraElementsConvention(true)
            };

            ConventionRegistry.Register("EnumStringConvention", pack, t => true);
        }
    }
}
