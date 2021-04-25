using System;

namespace Domain.Core.Exceptions
{
    public class UserException : Exception
    {
        public UserException(string message)
            : base(message)
        { }
    }
}