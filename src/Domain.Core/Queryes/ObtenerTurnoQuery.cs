using Domain.Core.CqsModule.Query;
using Domain.Core.Data.Repositories;
using Domain.Core.Security;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public ObtenerTurnoQueryResult(Guid id, string profesionalNombre, Guid profesionalId, string pacienteNombre, Guid pacienteId, DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin, TurnoEstado estado, IEnumerable<TurnoAccion> acciones, IEnumerable<TurnoHistorial> historial)
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
            Historial = historial;
        }

        public Guid Id { get; }

        [Display(Name = "Profesional")]
        public string ProfesionalNombre { get; }
        public Guid ProfesionalId { get; }

        [Display(Name = "Paciente")]
        public string PacienteNombre { get; }
        public Guid PacienteId { get; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Fecha { get; }
        
        [Display(Name = "Hora de inicio")]
        public TimeSpan HoraInicio { get; }

        [Display(Name = "Hora de fin")]
        public TimeSpan HoraFin { get; }
        public TurnoEstado Estado { get; }
        public IEnumerable<TurnoAccion> Acciones { get; }

        public IEnumerable<TurnoHistorial> Historial { get; }
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
                turno.Id, turno.Profesional.Nombre, turno.ProfesionalId, turno.Paciente.Nombre, turno.PacienteId, turno.Fecha, turno.HoraInicio, turno.HoraFin, turno.Estado, accionesPermitidas, turno.Historial);
        }
    }
}
