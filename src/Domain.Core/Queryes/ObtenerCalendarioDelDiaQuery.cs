using Domain.Core.CqsModule.Query;
using Domain.Core.Data.Repositories;
using Domain.Core.Security;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E = Domain.Entities;

namespace Domain.Core.Queryes
{
    public class ObtenerCalendarioDelDiaQuery : IQuery<ObtenerCalendarioDelDiaQueryResult>
    {
        public DateTime Fecha { get; }

        public ObtenerCalendarioDelDiaQuery(DateTime fecha)
        {
            Fecha = fecha;
        }
    }

    public class ObtenerCalendarioDelDiaQueryResult
    {
        public IEnumerable<Profesional> Profesionales { get; set; }
        public DateTime Fecha { get; set; }

        public class Profesional
        {
            public Guid Id { get; set; }
            public IEnumerable<Horario> Horarios { get; set; }

            public string ProfesionalNombre { get; set; }

            public bool Atiende { get; set; }

            public TimeSpan DuracionTurno { get; set; }

            public IEnumerable<DiaHorario> DiasQueAtiende { get; set; }

            public Profesional(DateTime fecha, IAuthenticatedUser authenticatedUser, E.Profesional profesional, IEnumerable<E.Turno> turnos)
            {
                Id = profesional.Id;
                ProfesionalNombre = profesional.Nombre;
                Atiende = profesional.Atiende(fecha);
                Horarios = new List<Horario>();
                DuracionTurno = profesional.DuracionTurno;
                DiasQueAtiende = profesional.DiasQueAtiende.OrderBy(x => x.Dia).ToList();

                if (Atiende)
                {
                    var diaQueAtiende = profesional.DiasQueAtiende.First(x => x.Dia == fecha.DayOfWeek);
                    var horaTurno = diaQueAtiende.HoraDesde;

                    while(horaTurno <= diaQueAtiende.HoraHasta)
                    {
                        var turno = turnos.FirstOrDefault(x => x.HoraInicio == horaTurno);

                        Horarios = Horarios.Add(new Horario 
                        { 
                            HoraInicio = horaTurno, 
                            Turno = turno != default 
                                ? new Turno(turno, authenticatedUser) 
                                : null,
                            SePuedeAsigar = turno == default && (horaTurno > DateTime.Now.TimeOfDay || fecha > DateTime.Now)
                        });

                        horaTurno = horaTurno.Add(profesional.DuracionTurno);
                    }

                    //chequeo si todos los turnos fueron asignados a la lista de horarios
                    //si alguno no fue puede ser porque cambio la duracion del turno del profesional, la hora de inicio o es un sobreturno (proximamente)

                    var turnosNoAsignados = turnos.Where(x => !Horarios.Where(x => x.Turno != null).Select(x => x.Turno.TurnoId).ToArray().Contains(x.Id)).ToList();

                    foreach (var turnoNoAsignado in turnosNoAsignados)
                    {
                        Horarios = Horarios.Add(new Horario { HoraInicio = turnoNoAsignado.HoraInicio, Turno = new Turno(turnoNoAsignado, authenticatedUser) });
                    }

                    Horarios = Horarios.OrderBy(x => x.HoraInicio);
                }
            }
        }

        public class Horario
        {
            public TimeSpan HoraInicio { get; set; }
            public Turno Turno { get; set; }
            public bool SePuedeAsigar { get; set; }
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
            public E.TurnoEstado Estado { get; set; }
            public IEnumerable<E.TurnoAccion> Acciones { get; set; }

            public Turno(E.Turno turno, IAuthenticatedUser authenticatedUser)
            {
                TurnoId = turno.Id;
                PacienteId = turno.PacienteId;
                PacienteNombre = turno.Paciente.Nombre;
                Estado = turno.Estado;
                Acciones = turno.AccionesPosibles
                    .Where(x => x.EstaHabilitado(authenticatedUser.UserInfo.Permisos))
                    .ToList();
            }
        }
    }

    public class ObtenerCalendarioDelDiaQueryHandler : IQueryHandler<ObtenerCalendarioDelDiaQuery, ObtenerCalendarioDelDiaQueryResult>
    {
        private readonly ITurnoRepository _turnoRepository;
        private readonly IProfesionalRepository _profesionalRepository;
        private readonly IAuthenticatedUser _authenticatedUser;

        public ObtenerCalendarioDelDiaQueryHandler(
            ITurnoRepository turnoRepository,
            IProfesionalRepository profesionalRepository,
            IAuthenticatedUser authenticatedUser)
        {
            _turnoRepository = turnoRepository ?? throw new ArgumentNullException(nameof(turnoRepository));
            _profesionalRepository = profesionalRepository ?? throw new ArgumentNullException(nameof(profesionalRepository));
            _authenticatedUser = authenticatedUser ?? throw new ArgumentNullException(nameof(authenticatedUser));
        }

        public async Task<ObtenerCalendarioDelDiaQueryResult> ExecuteAsync(ObtenerCalendarioDelDiaQuery query)
        {
            var profesionales = Enumerable.Empty<E.Profesional>();
            var turnos = Enumerable.Empty<E.Turno>();

            var puedeVerTodosLosTurnos = _authenticatedUser.Puede(Permiso.VerTodosTurno);

            profesionales = puedeVerTodosLosTurnos
                ? await _profesionalRepository.GetAllAsync()
                : new List<Profesional> { await _profesionalRepository.GetOneAsync(_authenticatedUser.UserInfo.Id) };

            turnos = await _turnoRepository.GetAllNoCanceladosAsync(query.Fecha, puedeVerTodosLosTurnos ? null : _authenticatedUser.UserInfo.Id);

            return new ObtenerCalendarioDelDiaQueryResult
            {
                Profesionales = profesionales
                    .Select(p => new ObtenerCalendarioDelDiaQueryResult.Profesional(
                        query.Fecha,
                        _authenticatedUser,
                        p,
                        turnos
                            .Where(t => t.ProfesionalId == p.Id)
                            .ToList()))
                    .OrderBy(x => x.ProfesionalNombre)
                    .ToList(),
                Fecha = query.Fecha
            };
        }
    }
}