using Domain.Core.CqsModule.Command;
using Domain.Core.Data.Repositories;
using Domain.Core.Exceptions;
using System;
using System.Threading.Tasks;

namespace Domain.Core.Commands.Validations
{
    public class ValidarProfesionalAtiendeEnFechaHoraCommand : _BaseTurnoCommand
    {
        public ValidarProfesionalAtiendeEnFechaHoraCommand(AgregarTurnoCommand agregarTurnoCommand)
            : base(agregarTurnoCommand)
        { }
        public ValidarProfesionalAtiendeEnFechaHoraCommand(Guid profesionalId, DateTime fecha, TimeSpan horaInicio) 
            : base(profesionalId, fecha, horaInicio)
        { }
    }

    public class ValidarProfesionalAtiendeEnFechaHoraCommandHandler : ICommandHandler<ValidarProfesionalAtiendeEnFechaHoraCommand>
    {
        private readonly IProfesionalRepository _profesionalRepository;

        public ValidarProfesionalAtiendeEnFechaHoraCommandHandler(IProfesionalRepository profesionalRepository)
        {
            _profesionalRepository = profesionalRepository ?? throw new ArgumentNullException(nameof(profesionalRepository));
        }

        public async Task HandleAsync(ValidarProfesionalAtiendeEnFechaHoraCommand command)
        {
            var profesional = await _profesionalRepository.GetOneAsync(command.ProfesionalId);

            if (!profesional.Atiende(command.Fecha, command.HoraInicio))
                throw new ProfesionalNoAtiendeException(profesional.Nombre, command.Fecha, command.HoraInicio);
        }
    }
}