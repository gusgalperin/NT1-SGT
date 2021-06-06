using Domain.Core.Exceptions;

namespace Domain.Core.Security
{
    public class NoTienePermisosException : UserException
    {
        public NoTienePermisosException()
            : base("El usuario no tiene permisos para la acción solicitada")
        { }
    }
}