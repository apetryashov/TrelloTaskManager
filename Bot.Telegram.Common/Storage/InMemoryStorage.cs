using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Bot.Telegram.Common.Storage
{
    public class InMemoryStorage<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : IHasKey<TKey>, new()
    {
        private readonly Dictionary<TKey, TEntity> keyValueStorage = new Dictionary<TKey, TEntity>();

        public void Add(TEntity entity)
        {
            keyValueStorage.Add(entity.Key, entity);
        }

        public void Add(IEnumerable<TEntity> entities)
        {
            foreach (var hasKey in entities)
            {
                keyValueStorage.Add(hasKey.Key, hasKey);
            }
        }

        public void Update(TEntity entity)
        {
            keyValueStorage[entity.Key] = entity;
        }

        public TEntity Get(TKey key)
        {
            if (keyValueStorage.TryGetValue(key, out var task))
                return task;

            task = new TEntity
            {
                Key = key
            };

            keyValueStorage.Add(key, task);
            return task;
        }

        public void Delete(TKey key)
        {
            keyValueStorage.Remove(key);
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            var func = predicate.Compile();
            return keyValueStorage
                .Where(x => func(x.Value))
                .Select(x => x.Value);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return keyValueStorage.Values;
        }
    }
}