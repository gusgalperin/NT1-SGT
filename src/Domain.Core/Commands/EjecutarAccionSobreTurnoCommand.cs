using Domain.Core.CqsModule.Command;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Domain.Core.Commands
{
    public class EjecutarAccionSobreTurnoResolverCommand : ICommand
    {

        public EjecutarAccionSobreTurnoResolverCommand(Guid turnoId, TurnoAccion accion)
        {
            TurnoId = turnoId;
            Accion = accion;
        }

        public Guid TurnoId { get; }
        public TurnoAccion Accion { get; }

        public static ICommand CreateCommand(Guid turnoId, TurnoAccion accion)
        {
            switch (accion)
            {
                case TurnoAccion.CheckIn:
                    return new PacienteCheckInCommand(turnoId);

                case TurnoAccion.Llamar:
                    return new LlamarPacienteCommand(turnoId);

                case TurnoAccion.Fin:
                    return new FinDeTurnoCommand(turnoId);

                case TurnoAccion.Cancelar:
                    return new CancelarTurnoCommand(turnoId);

                default:
                    throw new NotImplementedException();
            }
        }
    }

    public class EjecutarAccionSobreTurnoResolverCommandHandler : ICommandHandler<EjecutarAccionSobreTurnoResolverCommand>
    {
        private readonly ICommandProcessor _commandProcessor;

        public EjecutarAccionSobreTurnoResolverCommandHandler(ICommandProcessor commandProcessor)
        {
            _commandProcessor = commandProcessor ?? throw new ArgumentNullException(nameof(commandProcessor));
        }

        public async Task HandleAsync(EjecutarAccionSobreTurnoResolverCommand command)
        {
            switch (command.Accion)
            {
                case TurnoAccion.CheckIn:
                    await _commandProcessor.ProcessCommandAsync(new PacienteCheckInCommand(command.TurnoId));
                    break;

                case TurnoAccion.Llamar:
                    await _commandProcessor.ProcessCommandAsync(new LlamarPacienteCommand(command.TurnoId));
                    break;

                case TurnoAccion.Fin:
                    await _commandProcessor.ProcessCommandAsync(new FinDeTurnoCommand(command.TurnoId));
                    break;

                case TurnoAccion.Cancelar:
                    await _commandProcessor.ProcessCommandAsync(new CancelarTurnoCommand(command.TurnoId));
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}