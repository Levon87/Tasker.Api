using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Auth.Service.DataRepository
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly DbContext Context;

        public Repository(DbContext context)
        {
            Context = context;
        }

        public TEntity Get(Guid id)
        {
            return Context.Set<TEntity>()
                .Find(id);
        }

        public Task<TEntity> GetAsync(Guid id)
        {
            return Context.Set<TEntity>()
                .FindAsync(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>()
                .ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>()
                .ToListAsync();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>()
                .Where(predicate);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>()
                .Where(predicate)
                .ToListAsync();
        }

        public TEntity Add(TEntity entity)
        {
            return Context.Set<TEntity>()
                .Add(entity)
                .Entity;
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>()
                .AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>()
                .Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>()
                .RemoveRange(entities);
        }

        public TEntity Update(TEntity entity)
        {
            return Context.Set<TEntity>()
                .Update(entity)
                .Entity;
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>()
                .UpdateRange(entities);
        }
    }
}