using Domain.Core.CqsModule.Command;
using Domain.Core.Data.Repositories;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Domain.Core.Commands
{
    public class AgregarTurnoCommand : ICommand
    {
        public AgregarTurnoCommand(Guid idPaciente, Guid idProfesional, DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin)
        {
            IdPaciente = idPaciente;
            IdProfesional = idProfesional;
            Fecha = fecha;
            HoraInicio = horaInicio;
            HoraFin = horaFin;
        }

        public Guid IdPaciente { get; }
        public Guid IdProfesional { get; }
        public DateTime Fecha { get; }
        public TimeSpan HoraInicio { get; }
        public TimeSpan HoraFin { get; }
    }

    public class AgregarTurnoCommandHandler : ICommandHandler<AgregarTurnoCommand>
    {
        private readonly ICommandProcessor _commandProcessor;
        private readonly ITurnoRepository _turnoRepository;

        public AgregarTurnoCommandHandler(
            ICommandProcessor commandProcessor,
            ITurnoRepository turnoRepository)
        {
            _commandProcessor = commandProcessor ?? throw new ArgumentNullException(nameof(commandProcessor));
            _turnoRepository = turnoRepository ?? throw new ArgumentNullException(nameof(turnoRepository));
        }

        public async Task HandleAsync(AgregarTurnoCommand command)
        {
            await _commandProcessor.ProcessCommandAsync(
                ValidarAgregarTurnoCommand.From(command));

            var turno = new Turno(command.IdProfesional, command.IdPaciente, command.Fecha, command.HoraInicio, command.HoraFin);

            await _turnoRepository.AddAsync(turno);
        }
    }
}