using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace PContextus.Core.Domain.Entities
{
    using System;

    public abstract class EntityBase<T> : IEntity<T>
    {
        private DateTime? createdAt;

        [BsonRepresentation(BsonType.ObjectId)]
        public T Id { get; set; }

        public DateTime CreatedAt
        {
            get { return createdAt ?? DateTime.UtcNow; }
            set { createdAt = value; }
        }

        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        object IEntity.Id => Id;

        public override bool Equals(object obj)
        {
            var entity = obj as EntityBase<T>;

            if (entity == null)
            {
                return false;
            }

            if (ReferenceEquals(this, entity))
            {
                return true;
            }

            if (ReferenceEquals(null, entity))
            {
                return false;
            }

            return Id.Equals(entity.Id);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public static bool operator ==(EntityBase<T> a, EntityBase<T> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            {
                return true;
            }

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(EntityBase<T> a, EntityBase<T> b)
        {
            return !(a == b);
        }
    }
}
