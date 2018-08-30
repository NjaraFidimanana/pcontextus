namespace PContextus.Infrastructure.MongoDb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Contextus.Core.Domain;
    using MongoDB.Driver;
    using PContextus.Core.Domain.Entities;

    public sealed class Repository : IRepository
    {
        private readonly IMongoContext context;

        public Repository(IMongoContext context)
        {
            this.context = context;
        }
        async Task<IEnumerable<T>> IRepository.FindAsync<T>(FilterDefinition<T> filterDefinition, Func<IEnumerable<T>, IOrderedEnumerable<T>> orderBy, int? skip, int? take) {

            IEnumerable<T> query = await context.GetCollection<T>().Find(filterDefinition).ToListAsync();

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query;
        }
        async Task<IEnumerable<T>> IRepository.GetAllAsync<T>(Func<T, bool> filter, Func<IEnumerable<T>, IOrderedEnumerable<T>> orderBy, int? skip, int? take)
        {
            IEnumerable<T> query = await context.GetCollection<T>().Find(f => true).ToListAsync();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query;
        }

        async Task<T> IRepository.FindAsync<T>(Expression<Func<T, bool>> filter)
        {
            return await context.GetCollection<T>().Find(filter).FirstOrDefaultAsync();
        }

        async Task<T> IRepository.GetAsync<T>(object id)
        {
            return await context.GetCollection<T>().Find(e => e.Id.Equals(id)).SingleAsync();
        }

        async Task<bool> IRepository.ExistsAsync<T>(Expression<Func<T, bool>> filter)
        {
            return await context.GetCollection<T>().Find(filter).AnyAsync();
        }

        async Task<long> IRepository.CountAsync<T>(Expression<Func<T, bool>> filter)
        {
            return await context.GetCollection<T>().CountAsync(filter);
        }

        async Task IRepository.InsertAsync<T>(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await context.GetCollection<T>().InsertOneAsync(entity);
        }

        async Task IRepository.InsertManyAsync<T>(List<T> entity)
        {
            if (entity == null || entity.Count == 0)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await context.GetCollection<T>().InsertManyAsync(entity);
        }

        async Task IRepository.UpdateAsync<T>(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await context.GetCollection<T>().ReplaceOneAsync(e => e.Id.Equals(entity.Id),
                entity,
                new UpdateOptions
                {
                    IsUpsert = false
                });
        }

        async Task IRepository.UpdateOneAsync<T>(T entity, KeyValuePair<string, float> keyValue,FilterDefinition<T> filter)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var updateDef = Builders<T>.Update.Set(keyValue.Key, keyValue.Value);

            await context.GetCollection<T>().UpdateOneAsync(filter,
                  updateDef,
                   new UpdateOptions
                   {
                       IsUpsert = true,
                   }
                  );  
          
        }

        async Task IRepository.UpdateOneAsync<T>(T entity, UpdateDefinition<T> update, FilterDefinition<T> filter)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await context.GetCollection<T>().UpdateOneAsync(filter,
                  update,
                   new UpdateOptions
                   {
                       IsUpsert = true,
                   }
                  );

        }


        async Task IRepository.DeleteAsync<T>(object id)
        {
            await context.GetCollection<T>().DeleteOneAsync(e => e.Id.Equals(id));
        }

        async Task IRepository.DeleteManyAsync<T>(Expression<Func<T, bool>> filter)
        {
            await context.GetCollection<T>().DeleteManyAsync(filter);
        }
    }
}
