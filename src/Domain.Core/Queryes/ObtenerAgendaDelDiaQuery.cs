using Domain.Core.CqsModule.Query;
using Domain.Core.Data.Repositories;
using Domain.Core.Security;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.Queryes
{
    public class ObtenerAgendaDelDiaQuery : IQuery<ObtenerAgendaDelDiaResult>
    {
        public DateTimeOffset Fecha { get; }

        public ObtenerAgendaDelDiaQuery(DateTimeOffset? fecha = null)
        {
            Fecha = fecha ?? DateTimeOffset.UtcNow.Date;
        }
    }

    public class ObtenerAgendaDelDiaResult
    {
        public DateTimeOffset Fecha { get; set; }

        public IEnumerable<Turno> Turnos { get; set; }

        public ObtenerAgendaDelDiaResult(DateTimeOffset fecha, IEnumerable<Turno> turnos)
        {
            Fecha = fecha;
            Turnos = turnos;
        }

        public class Turno
        {
            public Guid TurnoId { get; set; }
            public TimeSpan HoraInicio { get; set; }
            public TimeSpan HoraFin { get; set; }
            public Guid PacienteId { get; set; }
            public string PacienteNombre { get; set; }
            public Guid ProfesionalId { get; set; }
            public string ProfesionalNombre { get; set; }
            public TurnoEstado Estado { get; set; }
        }        
    }

    public class ObtenerAgendaDelDiaQueryHandler : IQueryHandler<ObtenerAgendaDelDiaQuery, ObtenerAgendaDelDiaResult>
    {
        private readonly ITurnoRepository _turnoRepository;
        private readonly IAuthenticatedUser _authenticatedUser;

        public ObtenerAgendaDelDiaQueryHandler(
            ITurnoRepository turnoRepository,
            IAuthenticatedUser authenticatedUser)
        {
            _turnoRepository = turnoRepository ?? throw new ArgumentNullException(nameof(turnoRepository));
            _authenticatedUser = authenticatedUser ?? throw new ArgumentNullException(nameof(authenticatedUser));
        }

        public async Task<ObtenerAgendaDelDiaResult> ExecuteAsync(ObtenerAgendaDelDiaQuery query)
        {
            var turnos = (await _turnoRepository.GetAllAsync(query.Fecha))
                .Where(
                    !_authenticatedUser.Puede(Permiso.VerTodosTurno), 
                    x => x.ProfesionalId == _authenticatedUser.UserInfo.Id)
                .OrderBy(x => x.HoraInicio)
                .ToList();

            var result = new ObtenerAgendaDelDiaResult(query.Fecha, ToResult(turnos)); 

            return result;
        }

        private IEnumerable<ObtenerAgendaDelDiaResult.Turno> ToResult(IEnumerable<Turno> turnos)
        {
            foreach (var x in turnos)
            {
                yield return new ObtenerAgendaDelDiaResult.Turno
                {
                    TurnoId = x.Id,
                    HoraInicio = x.HoraInicio,
                    HoraFin = x.HoraFin,
                    PacienteId = x.PacienteId,
                    PacienteNombre = x.Paciente.Nombre,
                    ProfesionalId = x.ProfesionalId,
                    ProfesionalNombre = x.Profesional.Nombre,
                    Estado = x.Estado
                };
            }
        }
    }
}
