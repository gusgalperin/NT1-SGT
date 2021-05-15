using Domain.Core.CqsModule.Command;
using System;

namespace Domain.Core.Commands
{
    public abstract class _BaseTurnoCommand : ICommand
    {
        protected _BaseTurnoCommand(Guid profesionalId, DateTime fecha, TimeSpan horaInicio)
        {
            ProfesionalId = profesionalId;
            Fecha = fecha;
            HoraInicio = horaInicio;
        }

        public _BaseTurnoCommand (AgregarTurnoCommand agregarTurnoCommand)
            : this(agregarTurnoCommand.ProfesionalId, agregarTurnoCommand.Fecha, agregarTurnoCommand.HoraInicio) { }

        public Guid ProfesionalId { get; }
        public DateTime Fecha { get; }
        public TimeSpan HoraInicio { get; set; }
    }
}