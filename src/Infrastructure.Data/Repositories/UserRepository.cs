using Domain.Core.Data.Repositories;
using Domain.Core.Exceptions;
using Domain.Entities;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class UserRepository : EfRepository<Usuario, Guid>, IUserRepository
    {
        public UserRepository(DataContext dataContext)
            : base(dataContext)
        { }

        public async Task<Usuario> LoginAsync(string email, string password)
        {
            return (await _db.Usuarios
                .Where(x => x.Email == email.ToLower())
                .Where(x => x.Password == password)
                .Include(x => x.Rol)
                .Include(x => x.Rol.Permisos)
                .Include("Rol.Permisos.Permiso")
                .FirstOrDefaultAsync()) ?? throw new EntityNotFoundException<Usuario>(email);
        }
    }
}