using Domain.Core.CqsModule.Command;
using Domain.Core.Data;
using Domain.Core.Helpers;
using Domain.Core.Options;
using Microsoft.Extensions.Options;
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

    public class PacienteCheckInCommandHandler : ICommandHandler<PacienteCheckInCommand>, IValidatable<PacienteCheckInCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly TurnoOptions _options;

        public PacienteCheckInCommandHandler(
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider,
            IOptions<TurnoOptions> options)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task HandleAsync(PacienteCheckInCommand command)
        {
            var turno = await _unitOfWork.Turnos.GetOneAsync(command.TurnoId);

            turno.CheckedIn();

            await _unitOfWork.Turnos.UpdateAsync(turno);

            var profesional = await _unitOfWork.Profesionales.GetOneAsync(turno.ProfesionalId);

            profesional.EncolarPaciente(turno, _dateTimeProvider.Ahora(), TimeSpan.FromMinutes(_options.TiempoDeToleranciaEnMinutos));

            await _unitOfWork.Profesionales.UpdateColaAsync(profesional);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ValidateAsync(PacienteCheckInCommand command)
        {
        }
    }
}