using System;

namespace Domain.Entities
{
    public class ProfesionalCola : Entity<Guid>
    {
        public ProfesionalCola(TimeSpan horaLlegada, Guid idTurno, int orden)
            : base(Guid.NewGuid())
        {
            HoraLlegada = horaLlegada;
            IdTurno = idTurno;
            Orden = orden;
        }

        public TimeSpan HoraLlegada { get; private set; }
        public Guid IdTurno { get; private set; }
        public int Orden { get; private set; }
    }
}