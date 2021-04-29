using Domain.Core.CqsModule.Command;
using Domain.Core.Data;
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

    public class AgregarTurnoCommandHandler : ICommandHandler<AgregarTurnoCommand>
    {
        private readonly ICommandProcessor _commandProcessor;
        private readonly IUnitOfWork _unitOfWork;

        public AgregarTurnoCommandHandler(
            ICommandProcessor commandProcessor,
            IUnitOfWork unitOfWork)
        {
            _commandProcessor = commandProcessor ?? throw new ArgumentNullException(nameof(commandProcessor));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task HandleAsync(AgregarTurnoCommand command)
        {
            var profesional = await _unitOfWork.Profesionales.GetOneAsync(command.IdProfesional);
            var horaFin = command.HoraInicio.Add(profesional.DuracionTurno);

            await _commandProcessor.ProcessCommandAsync(
                ValidarAgregarTurnoCommand.From(command, horaFin));

            var turno = new Turno(command.IdProfesional, command.IdPaciente, command.Fecha, command.HoraInicio, horaFin);

            await _unitOfWork.Turnos.AddAsync(turno);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}