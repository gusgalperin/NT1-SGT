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

            OperationType = Entities.OperationType.Added;
        }

        public ProfesionalCola(TimeSpan horaLlegada, Turno turno, int orden) 
            : this(horaLlegada, turno.Id, orden)
        {
            Turno = turno;
        }

        public TimeSpan HoraLlegada { get; private set; }
        public Guid TurnoId { get; private set; }
        public Turno Turno { get; private set; }
        public int Orden { get; private set; }

        public Guid ProfesionalId { get; private set; }
        public Profesional Profesional { get; private set; }

        public OperationType? OperationType { get; private set; }

        public void NuevoOrden(int orden)
        {
            Orden = orden;

            OperationType = Entities.OperationType.Updated;
        }
    }
}