using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TaskManager.Common.Storage
{
    public interface IRepository<TEntity, in TKey>
        where TEntity : IHasKey<TKey>
    {
        void Add(TEntity entity);
        void Add(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        TEntity Get(TKey key);
        void Delete(TKey key);
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetAll();
    }
}