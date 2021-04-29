using Domain.Core.CqsModule.Command;
using Domain.Core.Data;
using System;
using System.Threading.Tasks;

namespace Domain.Core.Commands
{
    public class PacienteCheckInCommand : ICommand
    {
        public PacienteCheckInCommand(Guid turnoId)
        {
            TurnoId = turnoId;
        }

        public Guid TurnoId { get; }
    }

    public class PacienteCheckInCommandHandler : ICommandHandler<PacienteCheckInCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PacienteCheckInCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task HandleAsync(PacienteCheckInCommand command)
        {
            var turno = await _unitOfWork.Turnos.GetOneAsync(command.TurnoId);

            turno.CheckedIn();

            await _unitOfWork.Turnos.UpdateAsync(turno);


        }
    }
}