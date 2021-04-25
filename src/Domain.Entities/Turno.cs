using System;

namespace Domain.Entities
{
    public class Turno : Entity<Guid>
    {
        public Turno(Guid idProfesional, Guid idPaciente, DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin)
            : base(Guid.NewGuid())
        {
            IdProfesional = idProfesional;
            IdPaciente = idPaciente;
            Fecha = fecha;
            HoraInicio = horaInicio;
            HoraFin = horaFin;
            Estado = TurnoEstado.Pendiente;
        }

        public Guid IdProfesional { get; private set; }
        public Guid IdPaciente { get; private set; }
        public DateTimeOffset Fecha { get; private set; }
        public TimeSpan HoraInicio { get; private set; }
        public TimeSpan HoraFin { get; private set; }
        public TurnoEstado Estado { get; private set; }
    }

    public enum TurnoEstado
    {
        Pendiente,
        EnProceso,
        Finalizado,
        PacienteAusente,
        Cancelado,
    }
}