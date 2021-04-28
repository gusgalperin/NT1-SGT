using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Core.Data.Repositories
{
    public interface IPacienteRepository : IRepository<Paciente, Guid>
    {
        Task<IEnumerable<Paciente>> FindFromQueryAsync(string query);
    }
}