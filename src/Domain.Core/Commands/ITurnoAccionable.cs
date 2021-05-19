using Domain.Entities;

namespace Domain.Core.Commands
{
    public interface ITurnoAccionable
    {
        TurnoAccion Accion { get; }
    }
}