using Domain.Core.CqsModule.Command;
using Domain.Core.Exceptions;
using Domain.Core.Helpers;
using System;
using System.Threading.Tasks;

namespace Domain.Core.Commands.Validations
{
    public class ValidarFechaTurnoEsMayorAHoyCommand : _BaseTurnoCommand
    {
        public ValidarFechaTurnoEsMayorAHoyCommand(AgregarTurnoCommand agregarTurnoCommand) 
            : base(agregarTurnoCommand)
        { }

        public ValidarFechaTurnoEsMayorAHoyCommand(Guid profesionalId, DateTime fecha, TimeSpan horaInicio) 
            : base(profesionalId, fecha, horaInicio)
        { }
    }

    public class ValidarFechaTurnoEsMayorAHoyCommandHandler : ICommandHandler<ValidarFechaTurnoEsMayorAHoyCommand>
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public ValidarFechaTurnoEsMayorAHoyCommandHandler(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public Task HandleAsync(ValidarFechaTurnoEsMayorAHoyCommand command)
        {
            var fechaTurno = command.Fecha.Date;
            var fechaInicioTurno = fechaTurno.AddHours(command.HoraInicio.TotalHours);

            if (fechaInicioTurno <= _dateTimeProvider.Ahora())
                throw new UserException("La fecha debe ser mayor a 'ahora'");

            return Task.CompletedTask;
        }
    }
}