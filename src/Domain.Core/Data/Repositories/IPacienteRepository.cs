using Domain.Entities;
using System;

namespace Domain.Core.Data.Repositories
{
    public interface IPacienteRepository : IRepository<Turno, Guid>
    {
    }
}