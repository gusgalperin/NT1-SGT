using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Turno : Entity<Guid>
    {
        protected Turno()
        { }

        public Turno(Guid idProfesional, Guid idPaciente, DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin)
            : base(Guid.NewGuid())
        {
            ProfesionalId = idProfesional;
            PacienteId = idPaciente;
            Fecha = fecha;
            HoraInicio = horaInicio;
            HoraFin = horaFin;
            Estado = TurnoEstado.Pendiente;
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

        public DateTime FechaHoraInicio => Fecha.Add(HoraFin);

        public IEnumerable<TurnoAccion> AccionesPosibles 
            => TurnoEstadoMachine.ObtenerPosiblesAcciones(Estado);

        public void CambiarEstado(TurnoAccion accion)
            => Estado = TurnoEstadoMachine.ObtenerProximoEstado(Estado, accion);

        public bool SePuede(TurnoAccion accion)
            => TurnoEstadoMachine.TransicionPosible(Estado, accion);
    }

    public enum TurnoEstado
    {
        Pendiente,
        Encolado,
        EnAtencion,
        Finalizado,
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
        Cancelar
    }
}