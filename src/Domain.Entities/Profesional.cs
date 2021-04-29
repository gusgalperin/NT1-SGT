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

        public void EncolarPaciente(Turno turno, TimeSpan horaActual, TimeSpan tolerancia)
        {
            /*
             * si la cola esta vacía, 
                --> el orden es 1
             * si el paciente llega despues del horario de su turno - teniendo en cuenta una tolerancia de 15 min - 
                --> se lo ubica al final de la cola (max orden + 1)
             * si el paciente llega antes del horario de su turno o dentro de la tolerancia (y la cola no esta vacía)
                --> se lo ubica posterior el ultimo paciente encolado cuyo turno comience antes de la hora del turno del paciente que esta haciendo checkin
             */

            if (Cola == null || !Cola.Any())
            {
                Cola = new List<ProfesionalCola> { new ProfesionalCola(horaActual, turno, 1) };
                return;
            }

            if (horaActual > turno.HoraInicio.Add(tolerancia))
            {
                Cola.Add(new ProfesionalCola(horaActual, turno, Cola.Count() + 1));
                return;
            }

            Cola.Add(new ProfesionalCola(horaActual, turno, Cola.Count() + 1));

            var cola = Cola.OrderBy(x => x.Turno.HoraInicio).ToList();

            for (int i = 0; i < Cola.Count(); i++)
            {
                cola[i].NuevoOrden(i + 1);
            }

            Cola = cola;
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
}