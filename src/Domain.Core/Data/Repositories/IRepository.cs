using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Core.Data.Repositories
{
    public interface IRepository<TEntity, TId>
        where TEntity : Entity<TId>
        where TId : IEquatable<TId>
    {
        Task AddAsync(TEntity e);

        Task UpdateAsync(TEntity e);

        Task<TEntity> GetOneAsync(TId id);

        Task<IEnumerable<TEntity>> GetAllAsync();
    }
}