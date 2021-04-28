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
    public class TurnoRepository : EfRepository<Turno, Guid>, ITurnoRepository
    {
        public TurnoRepository(DataContext dataContext)
            : base(dataContext)
        { }

        public async Task<Turno> BuscarTurnoAsync(Guid idProfesional, DateTimeOffset fecha, TimeSpan horaInicio, TimeSpan horaFin)
        {
            return await _db.Turnos
                .Where(x => x.ProfesionalId == idProfesional)
                .Where(x => x.Fecha == fecha)
                .Where(x => x.HoraFin <= horaInicio && x.HoraFin >= horaInicio)
                .Where(x => x.HoraFin <= horaFin && x.HoraFin >= horaFin)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Turno>> GetAllAsync(DateTimeOffset fecha)
        {
            return await _db.Turnos
                .Where(x => x.Fecha == fecha.Date)
                .Include(x => x.Paciente)
                .Include(x => x.Profesional)
                .ToListAsync();
        }
    }
}