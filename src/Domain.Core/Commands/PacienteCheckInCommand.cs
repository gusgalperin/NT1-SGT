using Domain.Core.Commands.Internals;
using Domain.Core.Commands.Validations;
using Domain.Core.CqsModule.Command;
using Domain.Core.Data;
using Domain.Core.Helpers;
using Domain.Core.Options;
using Domain.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Domain.Core.Commands
{
    public class PacienteCheckInCommand : ICommand, ITurnoAccionable
    {
        public PacienteCheckInCommand(Guid turnoId)
        {
            TurnoId = turnoId;
        }

        public Guid TurnoId { get; }

        public TurnoAccion Accion => TurnoAccion.CheckIn;
    }

    public class PacienteCheckInCommandHandler : ICommandHandler<PacienteCheckInCommand>, ISecuredCommand
    {
        private readonly ICommandProcessor _commandProcessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly TurnoOptions _options;

        public PacienteCheckInCommandHandler(
            ICommandProcessor commandProcessor,
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider,
            IOptions<TurnoOptions> options)
        {
            _commandProcessor = commandProcessor ?? throw new ArgumentNullException(nameof(commandProcessor));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public string PermisoRequerido => Permiso.CheckinTurno;

        public async Task HandleAsync(PacienteCheckInCommand command)
        {
            var turno = await _unitOfWork.Turnos.GetOneAsync(command.TurnoId);

            await _commandProcessor.ProcessCommandAsync(
                CambiarEstadoTurnoCommand.FromCommand(command, turno));

            var profesional = await _unitOfWork.Profesionales.GetOneAsync(turno.ProfesionalId);

            profesional.EncolarPaciente(turno, _dateTimeProvider.Ahora(), TimeSpan.FromMinutes(_options.TiempoDeToleranciaEnMinutos));

            await _unitOfWork.Profesionales.UpdateColaAsync(profesional);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}