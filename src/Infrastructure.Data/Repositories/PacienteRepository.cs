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
    public class PacienteRepository : EfRepository<Paciente, Guid>, IPacienteRepository
    {
        public PacienteRepository(DataContext dataContext)
            : base(dataContext)
        { }

        public async Task<IEnumerable<Paciente>> FindFromQueryAsync(string query)
        {
            return await _db.Pacientes
                .Where(x => x.Nombre.StartsWith(query) || x.Dni.StartsWith(query))
                .ToListAsync();
        }
    }
}