using System;

namespace Domain.Core.Exceptions
{
    public class UserException : Exception
    {
        public UserException(string errorMessage) : base(errorMessage)
        { }

        public UserException(string errorMessage, Exception innerException) : base(errorMessage, innerException)
        { }
    }

}