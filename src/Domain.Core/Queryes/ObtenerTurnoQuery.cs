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
    public class ObtenerTurnoQuery : IQuery<ObtenerTurnoQueryResult>
    {
        public ObtenerTurnoQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }

    public class ObtenerTurnoQueryResult
    {
        public ObtenerTurnoQueryResult(Guid id, string profesionalNombre, Guid profesionalId, string pacienteNombre, Guid pacienteId, DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin, TurnoEstado estado, IEnumerable<TurnoAccion> acciones)
        {
            Id = id;
            ProfesionalNombre = profesionalNombre;
            ProfesionalId = profesionalId;
            PacienteNombre = pacienteNombre;
            PacienteId = pacienteId;
            Fecha = fecha;
            HoraInicio = horaInicio;
            HoraFin = horaFin;
            Estado = estado;
            Acciones = acciones;
        }

        public Guid Id { get; }
        public string ProfesionalNombre { get; }
        public Guid ProfesionalId { get; }
        public string PacienteNombre { get; }
        public Guid PacienteId { get; }
        public DateTime Fecha { get; }
        public TimeSpan HoraInicio { get; }
        public TimeSpan HoraFin { get; }
        public TurnoEstado Estado { get; }
        public IEnumerable<TurnoAccion> Acciones { get; }
    }

    public class ObtenerTurnoQueryHandler : IQueryHandler<ObtenerTurnoQuery, ObtenerTurnoQueryResult>
    {
        private readonly ITurnoRepository _turnoRepository;
        private readonly IAuthenticatedUser _authenticatedUser;

        public ObtenerTurnoQueryHandler(
            ITurnoRepository turnoRepository,
            IAuthenticatedUser authenticatedUser)
        {
            _turnoRepository = turnoRepository ?? throw new ArgumentNullException(nameof(turnoRepository));
            _authenticatedUser = authenticatedUser ?? throw new ArgumentNullException(nameof(authenticatedUser));
        }

        public async Task<ObtenerTurnoQueryResult> ExecuteAsync(ObtenerTurnoQuery query)
        {
            var turno = await _turnoRepository.GetOneAsync(query.Id);

            var accionesPermitidas = turno.AccionesPosibles
                .Where(x => x.EstaHabilitado(_authenticatedUser.UserInfo.Permisos))
                .ToList();

            return new ObtenerTurnoQueryResult(
                turno.Id, turno.Profesional.Nombre, turno.ProfesionalId, turno.Paciente.Nombre, turno.PacienteId, turno.Fecha, turno.HoraInicio, turno.HoraFin, turno.Estado, accionesPermitidas);
        }
    }
}
