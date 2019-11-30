using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LiteDB;

namespace Bot.Telegram.Common.Storage
{
    public class LiteDbRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : IHasKey<TKey>
    {
        private readonly LiteCollection<TEntity> collection;

        public LiteDbRepository(string dbName)
        {
            collection = new LiteDatabase(dbName)
                .GetCollection<TEntity>();
        }

        public void Add(TEntity entity)
        {
            collection.Insert(entity);
        }

        public void Add(IEnumerable<TEntity> entities)
        {
            collection.Insert(entities);
        }

        public void Update(TEntity entity)
        {
            collection.Update(entity);
        }

        public TEntity Get(TKey key)
        {
            return Get(x => x.Key.Equals(key));
        }

        public void Delete(TKey key)
        {
            Delete(x => x.Key.Equals(key));
        }

        private TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return collection.FindOne(predicate);
        }

        private void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            collection.DeleteMany(predicate);
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return collection.Find(predicate);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return collection.FindAll();
        }
    }
}