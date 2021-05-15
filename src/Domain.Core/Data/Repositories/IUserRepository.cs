using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Domain.Core.Data.Repositories
{
    public interface IUserRepository : IRepository<Usuario, Guid>
    {
        Task<Usuario> LoginAsync(string email, string password);
    }
}