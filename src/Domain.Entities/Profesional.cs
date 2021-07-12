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

        public TimeSpan DuracionTurno { get; private set; }

        protected Profesional() { }

        public Profesional(string nombre, string email, string password, IEnumerable<Especialidad> especialidades, ICollection<DiaHorario> diasQueAtiende, TimeSpan? duractionTurno = null)
            : base (Rol.Profesional(), nombre, email, password)
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
            DuracionTurno = duractionTurno ?? TimeSpan.FromMinutes(30);
        }

        public bool Atiende(DateTimeOffset fecha, TimeSpan hora)
        {
            return DiasQueAtiende
                .Where(x => x.Dia == fecha.DayOfWeek)
                .Where(x => hora >= x.HoraDesde)
                .Where(x => hora <= x.HoraHasta)
                .Any();
        }

        public bool Atiende(DateTimeOffset fecha)
        {
            return DiasQueAtiende.Any(x => x.Dia == fecha.DayOfWeek);
        }

        public void EncolarPaciente(Turno turno, DateTime horaActual, TimeSpan tolerancia)
        {
            /*
             * si la cola esta vacía, 
                --> el orden es 1
             */

            if (Cola == null || !Cola.Any())
            {
                Cola = new List<ProfesionalCola> { new ProfesionalCola(horaActual.TimeOfDay, turno, 1) };
                return;
            }


            /*
             * si el paciente llega despues del horario de su turno - teniendo en cuenta una tolerancia de 15 min (config) - 
                --> se lo ubica al final de la cola (max orden + 1)
             */

            if (horaActual > turno.FechaHoraInicio.Add(tolerancia))
            {
                Cola.Add(new ProfesionalCola(horaActual.TimeOfDay, turno, Cola.Count() + 1));
                return;
            }

            /*
             * si el paciente llega antes del horario de su turno o dentro de la tolerancia (y la cola no esta vacía)
                --> se lo ubica posterior el ultimo paciente encolado cuyo turno comience antes de la hora del turno del paciente que esta haciendo checkin
             */

            Cola.Add(new ProfesionalCola(horaActual.TimeOfDay, turno, Cola.Count() + 1));

            var cola = Cola.OrderBy(x => x.Turno.FechaHoraInicio).ToList();

            for (int i = 0; i < Cola.Count(); i++)
            {
                cola[i].NuevoOrden(i + 1);
            }

            Cola = cola;
        }

        public void DesencolarPaciente(Guid turnoId)
        {
            var desencolado = false;

            foreach (var turno in Cola)
            {
                if(turno.TurnoId == turnoId)
                {
                    turno.Delete();
                    desencolado = true;
                    continue;
                }

                if (desencolado)
                {
                    turno.NuevoOrden(turno.Orden - 1);
                }
            }
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