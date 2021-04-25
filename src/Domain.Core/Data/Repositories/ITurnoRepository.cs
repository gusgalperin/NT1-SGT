using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Domain.Core.Data.Repositories
{
    public interface ITurnoRepository : IRepository<Turno, Guid>
    {
        Task<Turno> BuscarTurnoAsync(Guid idProfesional, DateTimeOffset fecha, TimeSpan horaInicio, TimeSpan horaFin);
    }
}