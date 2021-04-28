using Domain.Core.Data;
using Domain.Core.Data.Repositories;
using Infrastructure.Data.Context;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _db;

        public IPacienteRepository Pacientes { get; }

        public IProfesionalRepository Profesionales { get; }

        public ITurnoRepository Turnos { get; }

        public UnitOfWork(
            IPacienteRepository pacienteRepository,
            IProfesionalRepository profesionalRepository,
            ITurnoRepository turnoRepository,
            DataContext db)
        {
            Pacientes = pacienteRepository ?? throw new ArgumentNullException(nameof(pacienteRepository));
            Profesionales = profesionalRepository ?? throw new ArgumentNullException(nameof(profesionalRepository));
            Turnos = turnoRepository ?? throw new ArgumentNullException(nameof(turnoRepository));
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}