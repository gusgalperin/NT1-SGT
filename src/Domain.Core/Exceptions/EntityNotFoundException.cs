using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.Exceptions
{
    public class EntityNotFoundException : UserException
    {
        public EntityNotFoundException(string entity)
            : base($"{entity} was not found") { }

        public EntityNotFoundException(string entity, object reference)
            : base($"{entity} '{reference}' was not found") { }

        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    public class EntityNotFoundException<TEntity> : EntityNotFoundException
    {
        public EntityNotFoundException()
            : base($"{typeof(TEntity).Name}") { }

        public EntityNotFoundException(object reference)
            : base($"{typeof(TEntity).Name}", $"{reference}") { }

        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }

}
