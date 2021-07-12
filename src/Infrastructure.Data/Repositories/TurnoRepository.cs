using Domain.Core.Data.Repositories;
using Domain.Entities;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Core;

namespace Infrastructure.Data.Repositories
{
    public class TurnoRepository : EfRepository<Turno, Guid>, ITurnoRepository
    {
        public TurnoRepository(DataContext dataContext)
            : base(dataContext)
        { }

        public async Task<Turno> BuscarTurnoAsync(Guid idProfesional, DateTimeOffset fecha, TimeSpan horaInicio)
        {
            return await _db.Turnos
                .Where(x => x.ProfesionalId == idProfesional)
                .Where(x => x.Fecha == fecha.Date)
                .Where(x => x.HoraInicio == horaInicio)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Turno>> GetAllAsync(DateTimeOffset fecha)
        {
            return await GetAllQuery()
                .Where(x => x.Fecha == fecha.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Turno>> GetAllNoCanceladosAsync(DateTimeOffset fecha, Guid? profesionalId = null)
        {
            var query = GetAllQuery()
                .Where(x => x.Fecha == fecha.Date)
                .Where(x => x.Estado != TurnoEstado.Cancelado);

            if (profesionalId.HasValue)
                query = query.Where(x => profesionalId.HasValue && x.ProfesionalId == profesionalId.Value);
                
            return await query.ToListAsync();
        }

        public override async Task<Turno> GetOneAsync(Guid id)
        {
            return await _db.Turnos
                .Include(x => x.Paciente)
                .Include(x => x.Profesional)
                .Include(x => x.Profesional.Cola)
                .Include(x => x.Historial)
                .Include("Historial.Usuario")
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        private IQueryable<Turno> GetAllQuery()
        {
            return _db.Turnos
               .Include(x => x.Paciente)
               .Include(x => x.Profesional)
               .Include(x => x.Profesional.DiasQueAtiende);
        }
    }
}