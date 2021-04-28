using Domain.Core.Data.Repositories;
using System.Threading.Tasks;

namespace Domain.Core.Data
{
    public interface IUnitOfWork
    {
        IPacienteRepository Pacientes { get; }
        IProfesionalRepository Profesionales { get; }
        ITurnoRepository Turnos { get; }

        Task SaveChangesAsync();
    }
}