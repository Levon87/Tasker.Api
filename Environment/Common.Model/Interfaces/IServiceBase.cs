using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.Model.Interfaces
{
    public interface IServiceBase
    {
        IEnumerable<TEntity> GetAll<TEntity>()
            where TEntity : class;

        IEnumerable<TEntity> Find<TEntity>(
            Expression<Func<TEntity, bool>> predicate)
            where TEntity : class;

        Task<IEnumerable<TEntity>> FindAsync<TEntity>(
            Expression<Func<TEntity, bool>> predicate)
            where TEntity : class;

        TEntity Add<TEntity>(
            TEntity entity)
            where TEntity : class;

        void AddRange<TEntity>(
            IEnumerable<TEntity> entity)
            where TEntity : class;

        void Delete<TEntity>(
            Guid id)
            where TEntity : class;

        void Delete<TEntity>(
            TEntity entity)
            where TEntity : class;

        TEntity Get<TEntity>(
            Guid id)
            where TEntity : class;

        TEntity Update<TEntity>(
            TEntity entity)
            where TEntity : class;
    }
}