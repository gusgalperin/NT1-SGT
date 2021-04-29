using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class DiaHorario : Entity<Guid>
    {
        public DiaHorario(DayOfWeek dia, TimeSpan horaDesde, TimeSpan horaHasta)
            : base(Guid.NewGuid())
        {
            Dia = dia;
            HoraDesde = horaDesde;
            HoraHasta = horaHasta;
        }

        public DayOfWeek Dia { get; private set; }
        public TimeSpan HoraDesde { get; private set; }
        public TimeSpan HoraHasta { get; private set; }
        public Guid ProfesionalId { get; private set; }
        public Profesional Profesional { get; private set; }

        public static ICollection<DiaHorario> DefaultTodaLaSemana()
        {
            var nueve = new TimeSpan(9, 0, 0);
            var dieciocho = new TimeSpan(18, 0, 0);

            return new List<DiaHorario>
            {
                new DiaHorario(DayOfWeek.Monday, nueve, dieciocho),
                new DiaHorario(DayOfWeek.Tuesday, nueve, dieciocho),
                new DiaHorario(DayOfWeek.Wednesday, nueve, dieciocho),
                new DiaHorario(DayOfWeek.Thursday, nueve, dieciocho),
                new DiaHorario(DayOfWeek.Friday, nueve, dieciocho),
            };
        }
    }
}