using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class Profesional : Usuario
    {
        public ICollection<ProfesionalEspecialidad> Especialidades { get; private set; }
        public ICollection<DiaHorario> DiasQueAtiende { get; private set; }
        public ICollection<ProfesionalCola> Cola { get; private set; }
        public ICollection<Turno> Turnos { get; private set; }

        public TimeSpan DuracionTurno => TimeSpan.FromMinutes(30);

        protected Profesional() { }

        public Profesional(string nombre, string email, string password, IEnumerable<Especialidad> especialidades, ICollection<DiaHorario> diasQueAtiende)
            : base (nombre, email, password)
        {
            if (especialidades == null || !especialidades.Any())
            {
                throw new ArgumentNullException(nameof(especialidades));
            }

            if (diasQueAtiende == null || !diasQueAtiende.Any())
            {
                throw new ArgumentNullException(nameof(diasQueAtiende));
            }

            Especialidades = especialidades.Select(x => new ProfesionalEspecialidad(x.Id)).ToList();
            DiasQueAtiende = diasQueAtiende;
        }

        public bool Atiende(DateTimeOffset fecha, TimeSpan horaDesde, TimeSpan horaHasta)
        {
            return DiasQueAtiende
                .Any(x => x.Dia == fecha.DayOfWeek 
                    && horaDesde >= x.HoraDesde
                    && horaHasta <= x.HoraHasta
                    && horaDesde <= x.HoraHasta
                    && horaHasta >= x.HoraDesde);
        }
    }

    public class Especialidad : Entity<Guid>
    {
        public string Descripcion { get; private set; }

        public ICollection<ProfesionalEspecialidad> Profesionales { get; private set; }

        public Especialidad(string descripcion)
            : base (Guid.NewGuid())
        {
            if (string.IsNullOrEmpty(descripcion))
            {
                throw new ArgumentException($"'{nameof(descripcion)}' cannot be null or empty.", nameof(descripcion));
            }

            Descripcion = descripcion;
        }
    }

    public class ProfesionalEspecialidad
    {
        public ProfesionalEspecialidad(Guid especialidadId)
        {
            EspecialidadId = especialidadId;
        }

        public Guid ProfesionalId { get; private set; }
        public Profesional Profesional { get; private set; }

        public Guid EspecialidadId { get; private set; }
        public Especialidad Especialidad { get; private set; }
    }

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