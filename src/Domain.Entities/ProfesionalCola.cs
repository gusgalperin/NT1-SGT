using System;

namespace Domain.Entities
{
    public class ProfesionalCola : Entity<Guid>
    {
        protected ProfesionalCola() { }

        public ProfesionalCola(TimeSpan horaLlegada, Guid idTurno, int orden)
            : base(Guid.NewGuid())
        {
            HoraLlegada = horaLlegada;
            TurnoId = idTurno;
            Orden = orden;
        }

        public TimeSpan HoraLlegada { get; private set; }
        public Guid TurnoId { get; private set; }
        public Turno Turno { get; private set; }
        public int Orden { get; private set; }

        public Guid ProfesionalId { get; private set; }
        public Profesional Profesional { get; private set; }
    }
}