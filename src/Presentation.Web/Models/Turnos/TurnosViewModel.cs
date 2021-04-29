using Domain.Core.Queryes;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Presentation.Web.Models.Turnos
{
    public class TurnosViewModel
    {
        public DateTimeOffset Fecha { get; set; }

        public IEnumerable<Turno> Turnos { get; set; }

        public TurnosViewModel()
        {
            Turnos = new List<Turno>();
        }

        public static TurnosViewModel FromQuery(ObtenerAgendaDelDiaResult queryResult)
        {
            return new TurnosViewModel
            {
                Fecha = queryResult.Fecha,
                Turnos = queryResult.Turnos
                    .Select(x => new Turno
                    {
                        HoraFin = x.HoraFin,
                        HoraInicio = x.HoraInicio,
                        PacienteId = x.PacienteId,
                        PacienteNombre = x.PacienteNombre,
                        ProfesionalId = x.ProfesionalId,
                        ProfesionalNombre = x.ProfesionalNombre,
                        TurnoId = x.TurnoId,
                        Estado = x.Estado
                    })
                    .ToList()
            };
        }

        public class Turno
        {
            public Guid TurnoId { get; set; }
            public TimeSpan HoraInicio { get; set; }
            public TimeSpan HoraFin { get; set; }
            public Guid PacienteId { get; set; }
            public string PacienteNombre { get; set; }
            public Guid ProfesionalId { get; set; }
            public string ProfesionalNombre { get; set; }
            public TurnoEstado Estado { get; set; }
        }
    }
}