using Domain.Core.CqsModule.Command;
using Domain.Core.Data;
using System;
using System.Threading.Tasks;

namespace Domain.Core.Commands
{
    public class LlamarPacienteCommand : ICommand
    {
        public LlamarPacienteCommand(Guid turnoId)
        {
            TurnoId = turnoId;
        }

        public Guid TurnoId { get; }
    }

    public class LlamarPacienteCommandHandler : ICommandHandler<LlamarPacienteCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public LlamarPacienteCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task HandleAsync(LlamarPacienteCommand command)
        {
            var turno = await _unitOfWork.Turnos.GetOneAsync(command.TurnoId);

            turno.Atendiendo();

            turno.Profesional.DesencolarPaciente(command.TurnoId);

            await _unitOfWork.Turnos.UpdateAsync(turno);
            await _unitOfWork.Profesionales.UpdateColaAsync(turno.Profesional);

            await _unitOfWork.SaveChangesAsync();
        }

        public Task ValidateAsync(LlamarPacienteCommand command)
        {
            return Task.CompletedTask;
        }
    }
}