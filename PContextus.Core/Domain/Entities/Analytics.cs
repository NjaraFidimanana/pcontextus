using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PContextus.Core.Domain.Entities
{
    public class Analytics
    {
        public float GaViews { get; set; }

        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public float GaTrialRating { get; set; }

        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public float GaRegistrationRating { get; set; }
    }
}
