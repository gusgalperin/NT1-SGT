using Domain.Core.CqsModule.Command;
using Domain.Core.Data;
using Domain.Core.Exceptions;
using Domain.Core.Security;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Domain.Core.Commands.Internals
{
    internal class CambiarEstadoTurnoCommand : ICommand
    {
        private CambiarEstadoTurnoCommand(Turno turno, TurnoAccion accion, bool saveChanges = false)
        {
            Turno = turno;
            Accion = accion;
            SaveChanges = saveChanges;
        }

        public static CambiarEstadoTurnoCommand FromCommand<TCommand>(TCommand command, Turno turno, bool saveChanges = false)
            where TCommand : ICommand, ITurnoAccionable
        {
            TurnoAccion accion = ((ITurnoAccionable)command).Accion;

            return new CambiarEstadoTurnoCommand(turno, accion, saveChanges);
        }

        public Turno Turno { get; }
        public TurnoAccion Accion { get; }
        public bool SaveChanges { get; }
    }

    internal class CambiarEstadoTurnoCommandHandler : ICommandHandler<CambiarEstadoTurnoCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticatedUser _authenticatedUser;

        public CambiarEstadoTurnoCommandHandler(
            IUnitOfWork unitOfWork,
            IAuthenticatedUser authenticatedUser)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _authenticatedUser = authenticatedUser ?? throw new ArgumentNullException(nameof(authenticatedUser));
        }

        public async Task HandleAsync(CambiarEstadoTurnoCommand command)
        {
            _authenticatedUser.ValidarPermiso(command.Accion.Permiso());
            
            if (!command.Turno.SePuede(command.Accion))
                throw new UserException("No se puede ejecutar la acción sobre el turno");

            command.Turno.CambiarEstado(command.Accion);

            await _unitOfWork.Turnos.UpdateAsync(command.Turno);

            if (command.SaveChanges)
                await _unitOfWork.SaveChangesAsync();
        }
    }
}