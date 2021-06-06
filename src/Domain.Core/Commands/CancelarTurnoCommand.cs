using Domain.Core.Commands.Internals;
using Domain.Core.Commands.Validations;
using Domain.Core.CqsModule.Command;
using Domain.Core.Data;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Domain.Core.Commands
{
    public class CancelarTurnoCommand : ICommand, ITurnoAccionable
    {
        public CancelarTurnoCommand(Guid turnoId)
        {
            TurnoId = turnoId;
        }

        public Guid TurnoId { get; }

        public Entities.TurnoAccion Accion => Entities.TurnoAccion.Cancelar;
    }

    public class CancelarTurnoCommandHandler : ICommandHandler<CancelarTurnoCommand>, IValidatable<CancelarTurnoCommand>, ISecuredCommand
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommandProcessor _commandProcessor;

        public CancelarTurnoCommandHandler(
            IUnitOfWork unitOfWork,
            ICommandProcessor commandProcessor)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _commandProcessor = commandProcessor ?? throw new ArgumentNullException(nameof(commandProcessor));
        }

        public string PermisoRequerido => Permiso.CancerlarTurno;

        public async Task HandleAsync(CancelarTurnoCommand command)
        {
            var turno = await _unitOfWork.Turnos.GetOneAsync(command.TurnoId);

            var turnoEncolado = turno.Estado == Entities.TurnoEstado.Encolado;

            await _commandProcessor.ProcessCommandAsync(
                CambiarEstadoTurnoCommand.FromCommand(command, turno));

            if (turnoEncolado)
            {
                turno.Profesional.DesencolarPaciente(command.TurnoId);

                await _unitOfWork.Profesionales.UpdateColaAsync(turno.Profesional);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ValidateAsync(CancelarTurnoCommand command)
        {
            await _commandProcessor.ProcessCommandAsync(new ValidarCancelarTurnoCommand(
                await _unitOfWork.Turnos.GetOneAsync(command.TurnoId)));
        }
    }
}