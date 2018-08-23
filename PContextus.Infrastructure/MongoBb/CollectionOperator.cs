namespace PContextus.Infrastructure.MongoDb
{
    using System.Collections.Generic;

    public class CollectionOperator : Collection
    {
        public string Operator { get; set; }

        public string Comparer { get; set; }
        
    }

    public class Collections
    {
        public List<CollectionOperator> Collection { get; set; }
        
    }
}
