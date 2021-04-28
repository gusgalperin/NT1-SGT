using Domain.Core.Data.Repositories;
using Domain.Entities;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public abstract class EfRepository<TEntity, TId> : IRepository<TEntity, TId>
        where TEntity : Entity<TId>
        where TId: IEquatable<TId>
    {
        protected readonly DataContext _db;

        private DbSet<TEntity> Set => _db.Set<TEntity>();

        public EfRepository(DataContext dataContext)
        {
            _db = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public async virtual Task AddAsync(TEntity e)
        {
            await Set.AddAsync(e);
        }

        public async virtual Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Set.ToListAsync();
        }

        public async virtual Task<TEntity> GetOneAsync(TId id)
        {
            return await Set.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public virtual Task UpdateAsync(TEntity e)
        {
            Set.Update(e);

            return Task.CompletedTask;
        }
    }
}