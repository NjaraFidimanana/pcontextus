namespace PContextus.Core.Domain.Entities
{
    using System;

    public interface IEntity
    {
        object Id { get; }

        DateTime CreatedAt { get; set; }

        DateTime? UpdatedAt { get; set; }

        string CreatedBy { get; set; }

        string UpdatedBy { get; set; }
    }

    public interface IEntity<T> : IEntity
    {
        new T Id { get; }
    }
}
