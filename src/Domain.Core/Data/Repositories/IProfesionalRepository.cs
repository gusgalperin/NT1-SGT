using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Core.Data.Repositories
{
    public interface IProfesionalRepository : IRepository<Profesional, Guid>
    {
        Task UpdateColaAsync(Profesional profesional);

        Task<IEnumerable<ProfesionalCola>> GetColaAsync(Guid profesionalId);
    }
}