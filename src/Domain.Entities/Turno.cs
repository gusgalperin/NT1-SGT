using System;

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

        public Profesional Profesional { get; private set; }
        public Paciente Paciente { get; private set; }

        public DateTime FechaHoraInicio => Fecha.Add(HoraFin);

        public void CheckedIn()
        {
            if (Estado != TurnoEstado.Pendiente)
                throw new InvalidOperationException();

            Estado = TurnoEstado.Encolado;
        }

        public void Atendiendo()
        {
            if (Estado != TurnoEstado.Encolado)
                throw new InvalidOperationException();

            Estado = TurnoEstado.EnAtencion;
        }

        public void FinalizaAtencion()
        {
            if (Estado != TurnoEstado.EnAtencion)
                throw new InvalidOperationException();

            Estado = TurnoEstado.Finalizado;
        }
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
        CheckIn,
        Llamar,
        Fin,
        Cancelar
    }

    public class TurnoEstadoMachine
    {
        private class Transicion
        {
            public TurnoEstado EstadoInicial { get; set; }
            public TurnoAccion EstadoAccion { get; set; }
            public TurnoEstado EstadoFinal { get; set; }
        }


    }
}