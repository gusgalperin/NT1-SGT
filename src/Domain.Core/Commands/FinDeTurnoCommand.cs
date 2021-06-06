using Domain.Core.Commands.Internals;
using Domain.Core.CqsModule.Command;
using Domain.Core.Data;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Domain.Core.Commands
{
    public class FinDeTurnoCommand : ICommand, ITurnoAccionable
    {
        public Guid TurnoId { get; }

        public Entities.TurnoAccion Accion => Entities.TurnoAccion.Fin;

        public FinDeTurnoCommand(Guid turnoId)
        {
            TurnoId = turnoId;
        }
    }

    public class FinDeTurnoCommandHandler : ICommandHandler<FinDeTurnoCommand>, ISecuredCommand
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommandProcessor _commandProcessor;

        public FinDeTurnoCommandHandler(
            IUnitOfWork unitOfWork,
            ICommandProcessor commandProcessor)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _commandProcessor = commandProcessor ?? throw new ArgumentNullException(nameof(commandProcessor));
        }

        public string PermisoRequerido => Permiso.FinTurno;

        public async Task HandleAsync(FinDeTurnoCommand command)
        {
            var turno = await _unitOfWork.Turnos.GetOneAsync(command.TurnoId);

            await _commandProcessor.ProcessCommandAsync(
                CambiarEstadoTurnoCommand.FromCommand(command, turno));

            await _unitOfWork.SaveChangesAsync();
        }
    }
}