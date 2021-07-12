using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class Turno : Entity<Guid>
    {
        protected Turno()
        { }

        public Turno(Guid idProfesional, Guid idPaciente, DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin, Guid usuarioId)
            : base(Guid.NewGuid())
        {
            ProfesionalId = idProfesional;
            PacienteId = idPaciente;
            Fecha = fecha;
            HoraInicio = horaInicio;
            HoraFin = horaFin;
            Estado = TurnoEstado.Pendiente;

            Historial = new List<TurnoHistorial> { new TurnoHistorial(null, TurnoAccion.Crear, Estado, usuarioId) };
        }

        public Guid ProfesionalId { get; private set; }
        public Guid PacienteId { get; private set; }

        public DateTime Fecha { get; private set; }
        public TimeSpan HoraInicio { get; private set; }
        public TimeSpan HoraFin { get; private set; }
        public TurnoEstado Estado { get; private set; }

        public DateTime FullFecha => Fecha.Add(HoraInicio);

        public Profesional Profesional { get; private set; }
        public Paciente Paciente { get; private set; }

        public ICollection<TurnoHistorial> Historial { get; private set; }

        public DateTime FechaHoraInicio => Fecha.Add(HoraFin);

        public IEnumerable<TurnoAccion> AccionesPosibles 
            => TurnoEstadoMachine.ObtenerPosiblesAcciones(Estado);

        public void CambiarEstado(TurnoAccion accion, Guid usuarioId)
        {
            var estadoAcutal = Estado;
            Estado = TurnoEstadoMachine.ObtenerProximoEstado(Estado, accion);
            Historial.Add(new TurnoHistorial(estadoAcutal, accion, Estado, usuarioId));
        }

        public bool SePuede(TurnoAccion accion)
            => TurnoEstadoMachine.TransicionPosible(Estado, accion);
    }

    public enum TurnoEstado
    {
        [NiceString("Pendiente")]
        Pendiente,
        
        [NiceString("En espera")]
        Encolado,
        
        [NiceString("En atención")]
        EnAtencion,
        
        [NiceString("Finalizado")]
        Finalizado,

        [NiceString("Cancelado")]
        Cancelado,
    }

    public enum TurnoAccion
    {
        [NiceString("Check-In")]
        [RequierePermiso("turno.checkin")]
        CheckIn,
        
        [NiceString("Llamar")]
        [RequierePermiso("turno.llamar")]
        Llamar,
        
        [NiceString("Fin de atención")]
        [RequierePermiso("turno.fin")]
        Fin,
        
        [NiceString("Cancelar")]
        [RequierePermiso("turno.cancelar")]
        Cancelar,

        [NiceString("Crear")]
        [RequierePermiso("turno.crear")]
        Crear
    }
}