using Domain.Core.Commands;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Web.Models.Turnos
{
    public class CrearTurnoViewModel
    {
        public IEnumerable<Profesional> Profesionales { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Fecha { get; set; }

        [Display(Name = "Hora de inicio")]
        public TimeSpan HoraInicio { get; set; }

        [Display(Name = "Profesional")]
        public Guid ProfesionalId { get; set; }

        [Display(Name = "Paciente")]
        public Guid PacienteId { get; set; }

        public string ExceptionMessage { get; set; }

        public CrearTurnoViewModel()
        {
            Fecha = DateTime.Now.Date;
        }

        public AgregarTurnoCommand ToCommand()
        {
            return new AgregarTurnoCommand(PacienteId, ProfesionalId, Fecha, HoraInicio);
        }
    }
}