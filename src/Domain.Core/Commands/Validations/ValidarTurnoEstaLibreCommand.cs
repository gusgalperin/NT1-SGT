using Domain.Core.CqsModule.Command;
using Domain.Core.Data.Repositories;
using Domain.Core.Exceptions;
using System;
using System.Threading.Tasks;

namespace Domain.Core.Commands.Validations
{
    public class ValidarTurnoEstaLibreCommand : _BaseTurnoCommand
    {
        public ValidarTurnoEstaLibreCommand(AgregarTurnoCommand agregarTurnoCommand)
            : base(agregarTurnoCommand)
        { }
        public ValidarTurnoEstaLibreCommand(Guid profesionalId, DateTime fecha, TimeSpan horaInicio) 
            : base(profesionalId, fecha, horaInicio)
        { }
    }

    public class ValidarTurnoEstaLibreCommandHandler : ICommandHandler<ValidarTurnoEstaLibreCommand>
    {
        private readonly ITurnoRepository _turnoRepository;

        public ValidarTurnoEstaLibreCommandHandler(ITurnoRepository turnoRepository)
        {
            _turnoRepository = turnoRepository ?? throw new ArgumentNullException(nameof(turnoRepository));
        }

        public async Task HandleAsync(ValidarTurnoEstaLibreCommand command)
        {
            var turno = await _turnoRepository.BuscarTurnoAsync(command.ProfesionalId, command.Fecha, command.HoraInicio);

            if (turno != null)
                throw new TurnoOcupadoException(command.Fecha, command.HoraInicio);
        }
    }
}