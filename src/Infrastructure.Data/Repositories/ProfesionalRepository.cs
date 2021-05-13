using Domain.Core.Data.Repositories;
using Domain.Entities;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
                .Include(x => x.Cola)
                .Include("Cola.Turno")
                .Include("Cola.Turno.Paciente")
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task UpdateColaAsync(Profesional profesional)
        {
            var dbSet = _db.Set<ProfesionalCola>();

            foreach (var c in profesional.Cola)
            {
                if (!c.OperationType.HasValue)
                    continue;

                switch (c.OperationType.Value)
                {
                    case OperationType.Added:
                        await dbSet.AddAsync(c);
                        break;
                    case OperationType.Updated:
                        dbSet.Update(c);
                        break;
                    case OperationType.Deleted:
                        dbSet.Remove(c);
                        break;
                }
            }
        }
        
        public async Task<IEnumerable<ProfesionalCola>> GetColaAsync(Guid profesionalId)
        {
            var dbSet = _db.Set<ProfesionalCola>();

            return await dbSet.AsQueryable()
                .Where(x => x.ProfesionalId == profesionalId)
                .Include(x => x.Turno)
                .Include(x => x.Turno.Paciente)
                .ToListAsync();
        }
    }
}