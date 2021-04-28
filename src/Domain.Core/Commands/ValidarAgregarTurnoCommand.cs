using Domain.Core.CqsModule.Command;
using Domain.Core.Data.Repositories;
using Domain.Core.Exceptions;
using System;
using System.Threading.Tasks;

namespace Domain.Core.Commands
{
    public class ValidarAgregarTurnoCommand : ICommand
    {
        public ValidarAgregarTurnoCommand(Guid idPaciente, Guid idProfesional, DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin)
        {
            IdPaciente = idPaciente;
            IdProfesional = idProfesional;
            Fecha = fecha;
            HoraInicio = horaInicio;
            HoraFin = horaFin;
        }

        public Guid IdPaciente { get; }
        public Guid IdProfesional { get; }
        public DateTime Fecha { get; }
        public TimeSpan HoraInicio { get; }
        public TimeSpan HoraFin { get; }

        public static ValidarAgregarTurnoCommand From(AgregarTurnoCommand fromCommand)
        {
            return new ValidarAgregarTurnoCommand(fromCommand.IdPaciente, fromCommand.IdProfesional, fromCommand.Fecha, fromCommand.HoraInicio, fromCommand.HoraFin);
        }
    }

    public class ValidarAgregarTurnoCommandHandler : ICommandHandler<ValidarAgregarTurnoCommand>
    {
        private readonly ITurnoRepository _turnoRepository;
        private readonly IProfesionalRepository _profesionalRepository;

        public ValidarAgregarTurnoCommandHandler(
            ITurnoRepository turnoRepository,
            IProfesionalRepository profesionalRepository)
        {
            _turnoRepository = turnoRepository ?? throw new ArgumentNullException(nameof(turnoRepository));
            _profesionalRepository = profesionalRepository ?? throw new ArgumentNullException(nameof(profesionalRepository));
        }

        public async Task HandleAsync(ValidarAgregarTurnoCommand command)
        {
            if (command.HoraFin <= command.HoraInicio)
                throw new UserException("La hora de fin debe ser posterior a la de incicio");

            if (command.Fecha <= DateTime.Now.Date)
                throw new UserException("La fecha debe ser mayor a hoy");

            await ValidarProfesionalAtiende(command);
            await ValidarTurnoLibre(command);
        }

        private async Task ValidarProfesionalAtiende(ValidarAgregarTurnoCommand command)
        {
            var profesional = await _profesionalRepository.GetOneAsync(command.IdProfesional);

            if (!profesional.Atiende(command.Fecha, command.HoraInicio, command.HoraFin))
                throw new ProfesionalNoAtiendeException(profesional.Nombre, command.Fecha, command.HoraInicio, command.HoraFin);
        }

        private async Task ValidarTurnoLibre(ValidarAgregarTurnoCommand command)
        {
            var turno = await _turnoRepository.BuscarTurnoAsync(command.IdProfesional, command.Fecha, command.HoraInicio, command.HoraFin);

            if (turno != null)
                throw new TurnoOcupadoException(command.Fecha, command.HoraInicio, command.HoraFin);
        }
    }
}