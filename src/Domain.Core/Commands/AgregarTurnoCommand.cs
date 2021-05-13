using Domain.Core.CqsModule.Command;
using Domain.Core.Data;
using Domain.Core.Exceptions;
using Domain.Core.Helpers;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Domain.Core.Commands
{
    public class AgregarTurnoCommand : ICommand
    {
        public AgregarTurnoCommand(Guid idPaciente, Guid idProfesional, DateTime fecha, TimeSpan horaInicio)
        {
            IdPaciente = idPaciente;
            IdProfesional = idProfesional;
            Fecha = fecha;
            HoraInicio = horaInicio;
        }

        public Guid IdPaciente { get; }
        public Guid IdProfesional { get; }
        public DateTime Fecha { get; }
        public TimeSpan HoraInicio { get; }
    }

    public class AgregarTurnoCommandHandler : ICommandHandler<AgregarTurnoCommand>, IValidatable<AgregarTurnoCommand>
    {
        private readonly ICommandProcessor _commandProcessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AgregarTurnoCommandHandler(
            ICommandProcessor commandProcessor,
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider)
        {
            _commandProcessor = commandProcessor ?? throw new ArgumentNullException(nameof(commandProcessor));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public async Task HandleAsync(AgregarTurnoCommand command)
        {
            var profesional = await _unitOfWork.Profesionales.GetOneAsync(command.IdProfesional);
            var horaFin = command.HoraInicio.Add(profesional.DuracionTurno);

            var turno = new Turno(command.IdProfesional, command.IdPaciente, command.Fecha, command.HoraInicio, horaFin);

            await _unitOfWork.Turnos.AddAsync(turno);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ValidateAsync(AgregarTurnoCommand command)
        {
            var horaInicioTurno = command.Fecha.AddHours(command.HoraInicio.TotalHours);

            if (horaInicioTurno <= _dateTimeProvider.Ahora())
                throw new UserException("La fecha debe ser mayor a 'ahora'");

            await ValidarProfesionalAtiende(command);
            await ValidarTurnoLibre(command);
        }

        private async Task ValidarProfesionalAtiende(AgregarTurnoCommand command)
        {
            var profesional = await _unitOfWork.Profesionales.GetOneAsync(command.IdProfesional);

            if (!profesional.Atiende(command.Fecha, command.HoraInicio))
                throw new ProfesionalNoAtiendeException(profesional.Nombre, command.Fecha, command.HoraInicio);
        }

        private async Task ValidarTurnoLibre(AgregarTurnoCommand command)
        {
            var turno = await _unitOfWork.Turnos.BuscarTurnoAsync(command.IdProfesional, command.Fecha, command.HoraInicio);

            if (turno != null)
                throw new TurnoOcupadoException(command.Fecha, command.HoraInicio);
        }
    }
}