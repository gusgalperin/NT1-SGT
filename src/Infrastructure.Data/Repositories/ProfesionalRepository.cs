using Domain.Core.Data.Repositories;
using Domain.Entities;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class ProfesionalRepository : EfRepository<Profesional, Guid>, IProfesionalRepository
    {
        public ProfesionalRepository(DataContext dataContext)
            : base(dataContext)
        {
        }

        public override async Task<IEnumerable<Profesional>> GetAllAsync()
        {
            return await _db.Profesionales
                .Include(x => x.DiasQueAtiende)
                .ToListAsync();
        }

        public override async Task<Profesional> GetOneAsync(Guid id)
        {
            return await _db.Profesionales
                .Include(x => x.DiasQueAtiende)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
    }
}