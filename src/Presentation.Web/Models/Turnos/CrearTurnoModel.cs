using Domain.Core.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Web.Models.Turnos
{
    public class CrearTurnoModel
    {
        public DateTime Fecha { get; set; }

        [Display(Name = "Hora de inicio")]
        public TimeSpan HoraInicio { get; set; }
        //public TimeSpan HoraFin { get; set; }
        
        [Display(Name = "Profesional")]
        public Guid ProfesionalId { get; set; }

        [Display(Name = "Paciente")]
        public Guid PacienteId { get; set; }


        public CrearTurnoModel()
        {
            Fecha = DateTime.Now.Date;
        }

        public AgregarTurnoCommand ToCommand()
        {
            return new AgregarTurnoCommand(PacienteId, ProfesionalId, Fecha, HoraInicio, HoraInicio.Add(TimeSpan.FromMinutes(30)));
        }
    }
}