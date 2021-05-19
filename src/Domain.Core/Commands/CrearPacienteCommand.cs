using Domain.Core.CqsModule.Command;
using Domain.Core.Data;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Domain.Core.Commands
{
    public class CrearPacienteCommand : ICommand
    {
        public CrearPacienteCommand(string dni, string nombre, DateTime fechaNacimiento)
        {
            Dni = dni;
            Nombre = nombre;
            FechaNacimiento = fechaNacimiento;
        }

        public string Dni { get; }
        public string Nombre { get; }
        public DateTime FechaNacimiento { get; }
    }

    public class CrearPacienteCommandHandler : ICommandHandler<CrearPacienteCommand, Paciente>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CrearPacienteCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Paciente> HandleAsync(CrearPacienteCommand command)
        {
            var paciente = new Paciente(command.Nombre, command.Dni, command.FechaNacimiento);

            await _unitOfWork.Pacientes.AddAsync(paciente);

            await _unitOfWork.SaveChangesAsync();

            return paciente;
        }
    }
}