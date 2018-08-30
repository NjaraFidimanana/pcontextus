namespace Contextus.Core.Domain
{
    using MongoDB.Driver;
    using PContextus.Core.Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IRepository
    {
        Task<IEnumerable<T>> GetAllAsync<T>(
            Func<T, bool> filter = null,
            Func<IEnumerable<T>, IOrderedEnumerable<T>> orderBy = null,
            int? skip = null,
            int? take = null) where T : class, IEntity;

        Task<IEnumerable<T>> FindAsync<T>(
            FilterDefinition<T> filterDefinition,
            Func<IEnumerable<T>, IOrderedEnumerable<T>> orderBy =null, int? skip =null, int? take =null)
            where T : class, IEntity;

        Task<T> FindAsync<T>(Expression<Func<T, bool>> filter) where T : class, IEntity;

        Task<T> GetAsync<T>(object id) where T : class, IEntity;

        Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> filter) where T : class, IEntity;

        Task<long> CountAsync<T>(Expression<Func<T, bool>> filter = null) where T : class, IEntity;

        Task InsertAsync<T>(T entity) where T : class, IEntity;

        Task InsertManyAsync<T>(List<T> entity) where T : class, IEntity;

        Task UpdateAsync<T>(T entity) where T : class, IEntity;

        Task DeleteAsync<T>(object id) where T : class, IEntity;

        Task DeleteManyAsync<T>(Expression<Func<T, bool>> filter = null) where T : class, IEntity;

        Task UpdateOneAsync<T>(T entity, KeyValuePair<string, float> keyValue, FilterDefinition<T> filter) where T : class, IEntity;

        Task UpdateOneAsync<T>(T entity, UpdateDefinition<T> update, FilterDefinition<T> filter) where T : class, IEntity;
    }
}
