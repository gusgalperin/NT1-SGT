using Domain.Core.Commands.Internals;
using Domain.Core.CqsModule.Command;
using Domain.Core.Data;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Domain.Core.Commands
{
    public class LlamarPacienteCommand : ICommand, ITurnoAccionable
    {
        public LlamarPacienteCommand(Guid turnoId)
        {
            TurnoId = turnoId;
        }

        public Guid TurnoId { get; }

        public Entities.TurnoAccion Accion => Entities.TurnoAccion.Llamar;
    }

    public class LlamarPacienteCommandHandler : ICommandHandler<LlamarPacienteCommand>, ISecuredCommand
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommandProcessor _commandProcessor;

        public LlamarPacienteCommandHandler(
            IUnitOfWork unitOfWork,
            ICommandProcessor commandProcessor)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _commandProcessor = commandProcessor ?? throw new ArgumentNullException(nameof(commandProcessor));
        }

        public string PermisoRequerido => Permiso.LlamarTurno;

        public async Task HandleAsync(LlamarPacienteCommand command)
        {
            var turno = await _unitOfWork.Turnos.GetOneAsync(command.TurnoId);

            await _commandProcessor.ProcessCommandAsync(
                CambiarEstadoTurnoCommand.FromCommand(command, turno));

            turno.Profesional.DesencolarPaciente(command.TurnoId);

            await _unitOfWork.Profesionales.UpdateColaAsync(turno.Profesional);

            await _unitOfWork.SaveChangesAsync();
        }

        public Task ValidateAsync(LlamarPacienteCommand command)
        {
            return Task.CompletedTask;
        }
    }
}