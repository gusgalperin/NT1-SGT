using Domain.Core.Exceptions;

namespace Domain.Core.Security
{
    public class IncorrectUserOrPasswordException : UserException
    {
        public IncorrectUserOrPasswordException() 
            : base("Usuario y/o contraseña incorrectos")
        { }
    }
}