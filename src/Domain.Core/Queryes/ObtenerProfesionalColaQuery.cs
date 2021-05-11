using Domain.Core.CqsModule.Query;
using Domain.Core.Data.Repositories;
using Domain.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.Queryes
{
    public class ObtenerProfesionalColaQuery : IQuery<IEnumerable<ObtenerProfesionalColaQueryResult>>
    {
        public Guid ProfesionalId { get; }

        public ObtenerProfesionalColaQuery(Guid profesionalId)
        {
            ProfesionalId = profesionalId;
        }
    }

    public class ObtenerProfesionalColaQueryResult
    {
        public Guid Id { get; set; }
        public string Paciente { get; set; }
        public string HoraTurno { get; set; }
    }

    public class ObtenerProfesionalColaQueryHandler : IQueryHandler<ObtenerProfesionalColaQuery, IEnumerable<ObtenerProfesionalColaQueryResult>>
    {
        private readonly IProfesionalRepository _profesionalRepository;

        public ObtenerProfesionalColaQueryHandler(IProfesionalRepository profesionalRepository)
        {
            _profesionalRepository = profesionalRepository ?? throw new ArgumentNullException(nameof(profesionalRepository));
        }

        public async Task<IEnumerable<ObtenerProfesionalColaQueryResult>> ExecuteAsync(ObtenerProfesionalColaQuery query)
        {
            var cola = await _profesionalRepository.GetColaAsync(query.ProfesionalId);

            return cola
                .OrderBy(x => x.Orden)
                .Select(x => new ObtenerProfesionalColaQueryResult
                {
                    Id = x.Id,
                    HoraTurno = $"{x.Turno.HoraInicio.ToLegibleString()} {x.Turno.HoraFin.ToLegibleString()}",
                    Paciente = x.Turno.Paciente.Nombre
                })
                .ToList();
        }
    }
}
