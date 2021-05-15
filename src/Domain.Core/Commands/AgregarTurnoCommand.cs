﻿using Domain.Core.Commands.Validations;
using Domain.Core.CqsModule.Command;
using Domain.Core.Data;
using Domain.Core.Helpers;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Domain.Core.Commands
{
    public class AgregarTurnoCommand : _BaseTurnoCommand
    {
        public AgregarTurnoCommand(Guid idPaciente, Guid idProfesional, DateTime fecha, TimeSpan horaInicio)
            : base(idProfesional, fecha, horaInicio)
        {
            IdPaciente = idPaciente;
        }

        public Guid IdPaciente { get; }
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
            var profesional = await _unitOfWork.Profesionales.GetOneAsync(command.ProfesionalId);
            var horaFin = command.HoraInicio.Add(profesional.DuracionTurno);

            var turno = new Turno(command.ProfesionalId, command.IdPaciente, command.Fecha, command.HoraInicio, horaFin);

            await _unitOfWork.Turnos.AddAsync(turno);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ValidateAsync(AgregarTurnoCommand command)
        {
            await _commandProcessor.ProcessCommandAsync(new ValidarFechaTurnoEsMayorAHoyCommand(command));
            await _commandProcessor.ProcessCommandAsync(new ValidarProfesionalAtiendeEnFechaHoraCommand(command));
            await _commandProcessor.ProcessCommandAsync(new ValidarTurnoEstaLibreCommand(command));
        }
    }
}