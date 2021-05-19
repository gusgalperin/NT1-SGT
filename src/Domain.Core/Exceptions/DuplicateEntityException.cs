namespace Domain.Core.Exceptions
{
    public class DuplicateEntityException : UserException
    {
        public DuplicateEntityException()
            : base($"Entidad duplicada")
        { }
    }
}