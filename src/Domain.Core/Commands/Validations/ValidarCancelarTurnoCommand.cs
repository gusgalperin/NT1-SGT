using Domain.Core.CqsModule.Command;
using Domain.Core.Exceptions;
using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Core.Commands.Validations
{
    public class ValidarCancelarTurnoCommand : ICommand
    {
        public ValidarCancelarTurnoCommand(Turno turno)
        {
            Turno = turno;
        }

        public Turno Turno { get; }
    }

    public class ValidarCancelarTurnoCommandHandler : ICommandHandler<ValidarCancelarTurnoCommand>
    {
        public Task HandleAsync(ValidarCancelarTurnoCommand command)
        {
            if (!command.Turno.SePuede(TurnoAccion.Cancelar))
            {
                throw new UserException($"El turno no se puede cancelar porque está en estado {command.Turno.Estado}");
            }

            return Task.CompletedTask;
        }
    }
}