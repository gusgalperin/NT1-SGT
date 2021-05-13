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
    public class ObtenerProfesionalColaQuery : IQuery<ObtenerProfesionalColaQueryResult>
    {
        public Guid ProfesionalId { get; }

        public ObtenerProfesionalColaQuery(Guid profesionalId)
        {
            ProfesionalId = profesionalId;
        }
    }

    public class ObtenerProfesionalColaQueryResult
    {
        public IEnumerable<ObtenerProfesionalColaQueryResultItem> Cola { get; set; }

        public string Profesional { get; set; }
        public Guid ProfesionalId { get; set; }

        public class ObtenerProfesionalColaQueryResultItem
        {
            public Guid Id { get; set; }
            public Guid TurnoId { get; set; }
            public string Paciente { get; set; }
            public string HoraTurno { get; set; }
            public string HoraLlegada { get; set; }
        }
    }

    public class ObtenerProfesionalColaQueryHandler : IQueryHandler<ObtenerProfesionalColaQuery, ObtenerProfesionalColaQueryResult>
    {
        private readonly IProfesionalRepository _profesionalRepository;

        public ObtenerProfesionalColaQueryHandler(IProfesionalRepository profesionalRepository)
        {
            _profesionalRepository = profesionalRepository ?? throw new ArgumentNullException(nameof(profesionalRepository));
        }

        public async Task<ObtenerProfesionalColaQueryResult> ExecuteAsync(ObtenerProfesionalColaQuery query)
        {
            var profesional = await _profesionalRepository.GetOneAsync(query.ProfesionalId);

            var result = new ObtenerProfesionalColaQueryResult
            {
                Profesional = profesional.Nombre,
                ProfesionalId = profesional.Id,
                Cola = profesional.Cola
                    .OrderBy(x => x.Orden)
                    .Select(x => new ObtenerProfesionalColaQueryResult.ObtenerProfesionalColaQueryResultItem
                    {
                        Id = x.Id,
                        TurnoId = x.Turno.Id,
                        HoraTurno = $"{x.Turno.HoraInicio.ToLegibleString()} {x.Turno.HoraFin.ToLegibleString()}",
                        Paciente = x.Turno.Paciente.Nombre,
                        HoraLlegada = x.HoraLlegada.ToLegibleString()
                    })
                    .ToList()
            };

            return result;
        }
    }
}
